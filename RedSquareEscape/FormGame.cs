using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using RedSquareEscape.Classes;

namespace RedSquareEscape
{
    public partial class FormGame : Form
    {
        private Player player;
        private int currentLevel;
        private string stageName;

        private List<Enemy> enemies = new List<Enemy>();
        private List<Bullet> playerBullets = new List<Bullet>();
        private List<Bullet> enemyBullets = new List<Bullet>();

        private DateTime lastUpdateTime;
        private float gameTime;
        private bool isPaused;
        private bool isGameOver;

        private Label lblHealth;
        private Label lblScore;
        private Label lblCoins;
        private Label lblLevel;

        private Button btnPause;
        private Button btnInventory;
        private Button btnShop;

        private ColorTheme currentTheme;
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();

        private FormInventory inventoryForm;
        private FormShop shopForm;
        
        private List<Bullet> bullets = new List<Bullet>();


        public FormGame(Player player)
        {
            this.player = player;
            InitializeComponents();
            InitializeGame();

            this.DoubleBuffered = true;
            this.KeyPreview = true;
        }

        private void InitializeComponents()
        {
            // تنظیمات فرم
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(30, 30, 30);

            // ایجاد UI
            lblScore = new Label()
            {
                Text = $"Score: {player.Score}",
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblCoins = new Label()
            {
                Text = $"Coins: {player.Coins}",
                ForeColor = Color.White,
                Location = new Point(20, 50),
                AutoSize = true
            };

            lblHealth = new Label()
            {
                Text = $"Health: {player.Health}/{player.MaxHealth}",
                ForeColor = Color.White,
                Location = new Point(20, 80),
                AutoSize = true
            };

            btnPause = new Button()
            {
                Text = "II",
                Size = new Size(40, 40),
                Location = new Point(this.ClientSize.Width - 60, 20),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White
            };
            btnPause.Click += (s, e) => TogglePause();

            this.Controls.Add(lblScore);
            this.Controls.Add(lblCoins);
            this.Controls.Add(lblHealth);
            this.Controls.Add(btnPause);

            // تایمر بازی
            var gameTimer = new Timer() { Interval = 16 }; // ~60 FPS
            gameTimer.Tick += (s, e) => UpdateGame();
            gameTimer.Start();
        }

        private void InitializeGame()
        {
            // موقعیت اولیه بازیکن
            player.Position = new PointF(
                this.ClientSize.Width / 2,
                this.ClientSize.Height - 100);

            // ایجاد دشمنان اولیه
            SpawnEnemies(5);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isPaused || isGameOver) return;

            float deltaTime = (float)(DateTime.Now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = DateTime.Now;
            gameTime += deltaTime;

            UpdateGame(deltaTime);
            this.Invalidate(); // Trigger paint
        }

        private void UpdateGame()
        {
            if (isPaused) return;

            float deltaTime = 0.016f; // تقریبا 16ms

            // به‌روزرسانی بازی
            UpdatePlayer(deltaTime);
            UpdateBullets(deltaTime);
            UpdateEnemies(deltaTime);
            CheckCollisions();

            // به‌روزرسانی UI
            UpdateUI();

            this.Invalidate(); // باعث فراخوانی OnPaint می‌شود
        }

        private void UpdatePlayerPosition(float deltaTime)
        {
            float moveSpeed = player.Speed * deltaTime;

            // Check for sprint key (Shift)
            bool isSprinting = keyStates.ContainsKey(Keys.ShiftKey) && keyStates[Keys.ShiftKey];
            if (isSprinting)
                moveSpeed *= 1.5f; // Sprint

            // Movement controls
            if ((keyStates.ContainsKey(Keys.W) && keyStates[Keys.W]) ||
                (keyStates.ContainsKey(Keys.Up) && keyStates[Keys.Up]))
                player.Move(0, -moveSpeed);

            if ((keyStates.ContainsKey(Keys.S) && keyStates[Keys.S]) ||
                (keyStates.ContainsKey(Keys.Down) && keyStates[Keys.Down]))
                player.Move(0, moveSpeed);

            if ((keyStates.ContainsKey(Keys.A) && keyStates[Keys.A]) ||
                (keyStates.ContainsKey(Keys.Left) && keyStates[Keys.Left]))
                player.Move(-moveSpeed, 0);

            if ((keyStates.ContainsKey(Keys.D) && keyStates[Keys.D]) ||
                (keyStates.ContainsKey(Keys.Right) && keyStates[Keys.Right]))
                player.Move(moveSpeed, 0);

            // Boundary check (replacement for Math.Clamp)
            float minX = player.Size / 2;
            float maxX = this.ClientSize.Width - player.Size / 2;
            float minY = player.Size / 2;
            float maxY = this.ClientSize.Height - player.Size / 2;

            // ایجاد یک کپی از Position فعلی
            PointF newPosition = player.Position;

            // اعمال تغییرات روی کپی
            newPosition.X = (newPosition.X < minX) ? minX :
                           (newPosition.X > maxX) ? maxX : newPosition.X;

            newPosition.Y = (newPosition.Y < minY) ? minY :
                           (newPosition.Y > maxY) ? maxY : newPosition.Y;

            // اختصاص مقدار جدید به Position
            player.Position = newPosition;
        }

        private void UpdateBullets(float deltaTime)
        {
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                playerBullets[i].Update(deltaTime);

                // حذف اگر از صفحه خارج شد
                if (IsOutOfBounds(playerBullets[i].Position))
                {
                    playerBullets.RemoveAt(i);
                    continue;
                }

                // بررسی برخورد با دشمنان
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (CheckCollision(playerBullets[i], enemies[j]))
                    {
                        enemies[j].Health -= playerBullets[i].Damage;
                        playerBullets.RemoveAt(i);

                        if (enemies[j].Health <= 0)
                        {
                            enemies.RemoveAt(j);
                        }
                        break;
                    }
                }
            }
        }

        private void CheckCollisions()
        {
            // Player bullets vs enemies
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    if (CheckCollision(playerBullets[i], enemies[j]))
                    {
                        enemies[j].Health -= playerBullets[i].Damage;
                        playerBullets.RemoveAt(i);

                        if (enemies[j].Health <= 0)
                        {
                            player.Score += 20;
                            player.Coins += 5;
                            enemies.RemoveAt(j);
                        }
                        break;
                    }
                }
            }

            // Enemy bullets vs player
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                if (CheckCollision(enemyBullets[i], player))
                {
                    player.TakeDamage(enemyBullets[i].Damage);
                    enemyBullets.RemoveAt(i);

                    if (player.Health <= 0)
                    {
                        GameOver();
                    }
                }
            }

            // Enemy vs player collision
            foreach (var enemy in enemies)
            {
                if (CheckCollision(enemy, player))
                {
                    player.TakeDamage(1);
                    if (player.Health <= 0)
                    {
                        GameOver();
                    }
                }
            }
        }

        private void UpdateUI()
        {
            lblHealth.Text = $"Health: {player.Health}/{player.MaxHealth}";
            lblScore.Text = $"Score: {player.Score}";
            lblCoins.Text = $"Coins: {player.Coins}";
            lblLevel.Text = $"{stageName}";
        }

        private void LevelCompleted()
        {
            isPaused = true;

            // Reward player
            player.Coins += currentLevel * 10;

            // Every 3 levels, extra rewards
            if (currentLevel % 3 == 0)
            {
                player.Health = Math.Min(player.MaxHealth, player.Health + 1);
                player.Coins *= 2;
            }

            var result = MessageBox.Show(
                $"Level {currentLevel} Completed!\n\n" +
                $"You earned {currentLevel * 10} coins.\n" +
                $"Continue to next level?",
                "Level Complete",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                currentLevel++;
                stageName = $"Level {currentLevel}";
                InitializeGame();
                isPaused = false;
            }
            else
            {
                this.Close();
                new FormMenu().Show();
            }
        }

        private void GameOver()
        {
            isGameOver = true;

            var result = MessageBox.Show(
                $"Game Over!\n\n" +
                $"Score: {player.Score}\n" +
                $"Coins: {player.Coins}\n\n" +
                $"Try again?",
                "Game Over",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Restart current level
                player.Health = player.MaxHealth;
                InitializeGame();
                isGameOver = false;
            }
            else
            {
                this.Close();
                new FormMenu().Show();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // رسم بازیکن
            player.Draw(e.Graphics);

            // رسم دشمنان
            foreach (var enemy in enemies)
            {
                enemy.Draw(e.Graphics);
            }

            // رسم تیرها
            foreach (var bullet in bullets)
            {
                bullet.Draw(e.Graphics);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            keyStates[e.KeyCode] = true;

            switch (e.KeyCode)
            {
                case Keys.B:
                    Shoot();
                    break;
                case Keys.Escape:
                    BtnPause_Click(null, null);
                    break;
                case Keys.D1:
                    UseItem("Bomb");
                    break;
                case Keys.D2:
                    UseItem("Freeze");
                    break;
                    // Add more item keys as needed
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            keyStates[e.KeyCode] = false;
        }
        private Enemy FindNearestEnemy(PointF playerPosition)
        {
            Enemy nearest = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float distance = DistanceBetween(playerPosition, enemy.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }
            }

            return nearest;
        }

        private float DistanceBetween(PointF a, PointF b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        private void Shoot()
        {
            Enemy nearestEnemy = FindNearestEnemy(player.Position);
            if (nearestEnemy == null) return;

            // تیر اصلی
            playerBullets.Add(new Bullet(
                player.Position,
                CalculateDirection(player.Position, nearestEnemy.Position),
                600f,  // سرعت
                player.Damage
            ));

            // شلیک چندتیری
            if (player.MultiShot > 1)
            {
                FireMultiShot(nearestEnemy);
            }
        }
        private PointF CalculateDirection(PointF from, PointF to)
        {
            float dx = to.X - from.X;
            float dy = to.Y - from.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            // نرمالایز کردن جهت
            if (length > 0)
            {
                dx /= length;
                dy /= length;
            }

            return new PointF(dx, dy);
        }
        private void FireMultiShot(Enemy mainTarget)
        {
            int additionalShots = player.MultiShot - 1;
            float angleStep = MathHelper.Pi / 8 / additionalShots; // پخش 22.5 درجه‌ای

            for (int i = 1; i <= additionalShots; i++)
            {
                // محاسبه جهت‌های پخش شده
                PointF spreadDir1 = RotateVector(
                    CalculateDirection(player.Position, mainTarget.Position),
                    angleStep * i
                );

                PointF spreadDir2 = RotateVector(
                    CalculateDirection(player.Position, mainTarget.Position),
                    -angleStep * i
                );

                // تیرهای پخش شده می‌توانند هدایت‌شونده باشند
                playerBullets.Add(new Bullet(
                    player.Position,
                    mainTarget,
                    500f,  // سرعت کمتر
                    player.Damage,
                    0.1f   // قدرت هدایت
                ));
            }
        }
        private PointF RotateVector(PointF vector, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new PointF(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }
        private void UseItem(string itemName)
        {
            if (player.Inventory.ContainsKey(itemName) && player.Inventory[itemName] > 0)
            {
                switch (itemName)
                {
                    case "Bomb":
                        enemies.Clear();
                        player.Inventory[itemName]--;
                        break;

                    case "Freeze":
                        foreach (var enemy in enemies)
                        {
                            enemy.Freeze(5f);
                        }
                        player.Inventory[itemName]--;
                        break;

                    case "HealthPotion":
                        player.Health = Math.Min(player.MaxHealth, player.Health + 1);
                        player.Inventory[itemName]--;
                        break;
                }

                // Update inventory form if open
                inventoryForm?.RefreshInventory();
            }
        }

        private bool CheckCollision(Bullet bullet, Enemy enemy)
        {
            float dx = bullet.Position.X - enemy.Position.X;
            float dy = bullet.Position.Y - enemy.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance < (bullet.Size + enemy.Size) / 2f;
        }

        private bool CheckCollision(Bullet bullet, Player player)
        {
            float dx = bullet.Position.X - player.Position.X;
            float dy = bullet.Position.Y - player.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance < (bullet.Size + player.Size) / 2f;
        }

        private bool CheckCollision(Enemy enemy, Player player)
        {
            float dx = enemy.Position.X - player.Position.X;
            float dy = enemy.Position.Y - player.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            return distance < (enemy.Size + player.Size) / 2f;
        }

        private bool IsOutOfBounds(PointF position)
        {
            return position.X < 0 || position.X > this.ClientSize.Width ||
                   position.Y < 0 || position.Y > this.ClientSize.Height;
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            isPaused = true;

            var pauseMenu = new FormPause();
            if (pauseMenu.ShowDialog() == DialogResult.OK)
            {
                isPaused = false;
                lastUpdateTime = DateTime.Now; // Reset timer to avoid large delta
            }
            else
            {
                this.Close();
                new FormMenu().Show();
            }
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            isPaused = true;
            inventoryForm = new FormInventory(player);
            inventoryForm.FormClosed += (s, args) =>
            {
                isPaused = false;
                inventoryForm = null;
                lastUpdateTime = DateTime.Now;
            };
            inventoryForm.Show();
        }

        private void BtnShop_Click(object sender, EventArgs e)
        {
            isPaused = true;
            shopForm = new FormShop(player);
            shopForm.FormClosed += (s, args) =>
            {
                isPaused = false;
                shopForm = null;
                lastUpdateTime = DateTime.Now;
                UpdateUI(); // Refresh coin display
            };
            shopForm.Show();
        }
    }
}
