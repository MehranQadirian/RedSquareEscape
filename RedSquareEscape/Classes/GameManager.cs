using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedSquareEscape.Classes
{
    public class GameManager
    {
        private Size gameAreaSize;
        public Player Player { get; set; }
        public List<Enemy> Enemies { get; set; } = new List<Enemy>();
        public List<Bullet> Bullets { get; set; } = new List<Bullet>();
        public List<Item> Items { get; set; } = new List<Item>();
        public int CurrentLevel { get; set; } = 1;
        public int Score { get; set; } = 0;
        public bool IsPaused { get; set; } = false;
        public bool IsGameOver { get; set; } = false;

        private Timer gameTimer;
        private Random random = new Random();
        private int enemySpawnTimer = 0;
        private int itemSpawnTimer = 0;

        public event Action OnGameOver;
        public event Action OnLevelComplete;
        private ParticleSystem particleSystem;
        private Action<string> showComboText;

        public GameManager(ParticleSystem particleSystem, Action<string> comboTextHandler, Player player, Size gameAreaSize)
        {
            this.particleSystem = particleSystem;
            this.showComboText = comboTextHandler;
            this.Player = player;
            this.gameAreaSize = gameAreaSize;
        }

        private void InitializeGame()
        {
            gameTimer = new Timer { Interval = 16 }; // ~60 FPS
            gameTimer.Tick += UpdateGame;
            gameTimer.Start();
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            if (IsPaused || IsGameOver) return;

            // Update player
            // (Player movement is handled by input)

            // Update enemies
            foreach (var enemy in Enemies)
            {
                enemy.Update(Player.Position);

                // Check enemy-player collision
                if (IsColliding(enemy.Position, Player.Position, 30))
                {
                    Player.TakeDamage(enemy.Damage);
                }
            }

            // Update bullets
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Update();

                // بررسی خروج از صفحه
                if (IsOutOfBounds(Bullets[i].Position))
                {
                    Bullets.RemoveAt(i);
                    continue; // به گلوله بعدی برو
                }

                // بررسی برخورد با دشمنان
                for (int j = Enemies.Count - 1; j >= 0; j--)
                {
                    if (CheckCollision(Bullets[i].Position, Enemies[j].Position, Bullets[i].Size, Enemies[j].Size))
                    {
                        Enemies[j].TakeDamage(Bullets[i].Damage);
                        Bullets.RemoveAt(i);

                        if (Enemies[j].Health <= 0)
                        {
                            Player.Score += 20;
                            Enemies.RemoveAt(j);
                        }

                        break; // از حلقه دشمنان خارج شو
                    }
                }
            }

            // Spawn enemies
            enemySpawnTimer++;
            if (enemySpawnTimer >= 120 - (CurrentLevel * 5)) // Faster spawning as level increases
            {
                SpawnEnemy();
                enemySpawnTimer = 0;
            }

            // Spawn items
            itemSpawnTimer++;
            if (itemSpawnTimer >= 300) // Every 5 seconds at 60 FPS
            {
                SpawnItem();
                itemSpawnTimer = 0;
            }

            // Check level completion
            if (Score >= CurrentLevel * 100)
            {
                CompleteLevel();
            }
        }
        private void CheckGameOver()
        {
            if (Player.Health <= 0)
            {
                OnGameOver?.Invoke(); // فراخوانی رویداد
            }
        }
        private void SpawnEnemy()
        {
            PointF spawnPosition;

            // Spawn at edges of screen
            if (random.Next(2) == 0)
            {
                // Left or right edge
                float x = random.Next(2) == 0 ? -30 : 800;
                float y = random.Next(600);
                spawnPosition = new PointF(x, y);
            }
            else
            {
                // Top or bottom edge
                float x = random.Next(800);
                float y = random.Next(2) == 0 ? -30 : 600;
                spawnPosition = new PointF(x, y);
            }

            Enemy enemy;
            int enemyType = random.Next(100);

            if (enemyType < 60 - CurrentLevel) // Normal enemy
            {
                enemy = new Enemy(spawnPosition, 30, 1, 2f, Color.Red);
            }
            else if (enemyType < 90 - CurrentLevel) // Fast enemy
            {
                enemy = new Enemy(spawnPosition, 20, 0.5f, 4f, Color.Yellow);
            }
            else // Tank enemy
            {
                enemy = new Enemy(spawnPosition, 60, 2, 1f, Color.Blue);
            }

            Enemies.Add(enemy);
        }
        public void DefeatEnemies(int damage, bool applyEffects = false)
        {
            int enemiesDefeated = 0;

            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                Enemies[i].TakeDamage(damage);

                if (applyEffects)
                {
                    ApplySpecialEffects(Enemies[i]);
                }

                if (Enemies[i].Health <= 0)
                {
                    enemiesDefeated++;
                    Enemies.RemoveAt(i);
                }
            }

            // امتیاز مضاعف برای شکست چند دشمن با یک حمله
            int bonus = (int)(enemiesDefeated * 0.5f);
            Player.Score += 20 * enemiesDefeated + bonus;

            // افزایش سکه برای هر 3 دشمن شکست داده شده
            if (enemiesDefeated >= 3)
            {
                Player.Coins += enemiesDefeated / 3;
            }
            if (enemiesDefeated > 0)
            {
                // نمایش انیمیشن شکست دشمنان
                ShowDefeatAnimation(enemiesDefeated);

                // پخش صدای مناسب
                //PlaySound(enemiesDefeated > 3 ? SoundType.MultiKill : SoundType.EnemyDefeated);
            }
        }
        private void ShowDefeatAnimation(int count)
        {
            if (particleSystem != null)
            {
                particleSystem.CreateExplosionEffect();
            }

            if (showComboText != null && count > 1)
            {
                showComboText(count + " KILLS!");
            }
        }
        private void ApplySpecialEffects(Enemy enemy)
        {
            // اینجا می‌توانید اثرات ویژه مانند انفجار، فریز کردن و... را پیاده‌سازی کنید
            enemy.ApplyEffect(EffectType.Explosion); // مثال
        }
        private bool CheckCollision(PointF pos1, PointF pos2, float size1, float size2)
        {
            float dx = pos1.X - pos2.X;
            float dy = pos1.Y - pos2.Y;
            float distanceSquared = dx * dx + dy * dy;
            float minDistance = (size1 + size2) / 2;
            return distanceSquared < minDistance * minDistance;
        }
        private void HandleCollisions()
        {
            foreach (var enemy in Enemies)
            {
                if (CheckCollision(Player.Position, enemy.Position, 15, 15))
                {
                    PointF pushDirection = new PointF(
                        Player.Position.X - enemy.Position.X,
                        Player.Position.Y - enemy.Position.Y
                    );

                    float distance = (float)Math.Sqrt(pushDirection.X * pushDirection.X +
                                                    pushDirection.Y * pushDirection.Y);

                    if (distance > 0)
                    {
                        float pushForce = 5f;
                        // حالا دیگر به velocity دسترسی داریم
                        Player.velocity.X += pushDirection.X / distance * pushForce;
                        Player.velocity.Y += pushDirection.Y / distance * pushForce;

                        Player.TakeDamage(enemy.Damage);
                    }
                }
            }
        }
        private void SpawnItem()
        {
            PointF position = new PointF(random.Next(50, 750), random.Next(50, 550));
            Item item;

            switch (random.Next(4))
            {
                case 0:
                    item = new Item(position, ItemType.HealthPotion, 20);
                    break;
                case 1:
                    item = new Item(position, ItemType.ShieldPotion, 15);
                    break;
                case 2:
                    item = new Item(position, ItemType.Bomb, 0);
                    break;
                default:
                    item = new Item(position, ItemType.Freeze, 0);
                    break;
            }

            Items.Add(item);
        }

        private bool IsColliding(PointF pos1, PointF pos2, float distance)
        {
            float dx = pos1.X - pos2.X;
            float dy = pos1.Y - pos2.Y;
            return (dx * dx + dy * dy) < (distance * distance);
        }

        private bool IsOutOfBounds(PointF position)
        {
            return position.X < -50 || position.X > gameAreaSize.Width + 50 ||
                   position.Y < -50 || position.Y > gameAreaSize.Height + 50;
        }

        private void CompleteLevel()
        {
            CurrentLevel++;
            OnLevelComplete?.Invoke();

            // Every 3 levels, spawn a boss
            if (CurrentLevel % 3 == 0)
            {
                SpawnBoss();
            }
        }

        private void SpawnBoss()
        {
            PointF position = new PointF(400, -50);
            Enemy boss = new Enemy(position, 200, 3, 1.5f, Color.Purple);
            boss.IsBoss = true;
            Enemies.Add(boss);
        }

        public void PauseGame()
        {
            IsPaused = true;
            gameTimer.Stop();
        }

        public void ResumeGame()
        {
            IsPaused = false;
            gameTimer.Start();
        }

        public void SaveGame(string saveName)
        {
            GameState gameState = new GameState
            {
                Player = this.Player,
                CurrentLevel = this.CurrentLevel,
                Score = this.Score,
                Coins = this.Player.Coins,
                SaveTime = DateTime.Now,
                StageName = $"Level {CurrentLevel}"
            };

            SaveManager.SaveGame(gameState, saveName);
        }

        public void LoadGame(string saveName)
        {
            GameState gameState = SaveManager.LoadGame(saveName);
            if (gameState != null)
            {
                this.Player = gameState.Player;
                this.CurrentLevel = gameState.CurrentLevel;
                this.Score = gameState.Score;
                this.Player.Coins = gameState.Coins;
            }
        }
    }
}
