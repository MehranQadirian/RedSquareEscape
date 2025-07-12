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
        private GameManager gameManager;
        private Timer gameTimer;
        private Button btnPause;
        private Button btnShop;
        private Button btnInventory;
        private Label lblScore;
        private Label lblHealth;
        private Label lblLevel;
        private Panel pauseMenu;
        private InputState inputState = new InputState();
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();
        private ParticleSystem particleSystem = new ParticleSystem();
        public FormGame(Player player)
        {
            this.player = player;
            InitializeComponents();
            InitializeGame();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            this.KeyDown += FormGame_KeyDown;

            // دکمه مکث
            btnPause = new Button
            {
                Text = "II",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(50, 50),
                Location = new Point(20, 20)
            };
            btnPause.Click += BtnPause_Click;
            this.Controls.Add(btnPause);

            // دکمه فروشگاه
            btnShop = new Button
            {
                Text = "Shop",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 50),
                Location = new Point(80, 20)
            };
            btnShop.Click += BtnShop_Click;
            this.Controls.Add(btnShop);

            // دکمه اینونتوری
            btnInventory = new Button
            {
                Text = "Items",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 50),
                Location = new Point(170, 20)
            };
            btnInventory.Click += BtnInventory_Click;
            this.Controls.Add(btnInventory);

            // نمایش امتیاز
            lblScore = new Label
            {
                Text = $"Score: {player.Score}",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 150, 20)
            };
            this.Controls.Add(lblScore);

            // نمایش سلامت
            lblHealth = new Label
            {
                Text = $"Health: {player.Health}/{player.MaxHealth}",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 150, 50)
            };
            this.Controls.Add(lblHealth);

            // نمایش سطح
            lblLevel = new Label
            {
                Text = $"Level: {player.Level}",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 150, 80)
            };
            this.Controls.Add(lblLevel);

            // منوی مکث
            pauseMenu = new Panel
            {
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(300, 300),
                Location = new Point(
                    (this.ClientSize.Width - 300) / 2,
                    (this.ClientSize.Height - 300) / 2
                ),
                Visible = false
            };

            Button btnResume = new Button
            {
                Text = "Resume",
                Font = new Font("Arial", 14),
                Size = new Size(200, 40),
                Location = new Point(50, 30)
            };
            btnResume.Click += (s, e) =>
            {
                pauseMenu.Visible = false;
                gameManager.ResumeGame();
            };
            pauseMenu.Controls.Add(btnResume);

            Button btnSave = new Button
            {
                Text = "Save Game",
                Font = new Font("Arial", 14),
                Size = new Size(200, 40),
                Location = new Point(50, 90)
            };
            btnSave.Click += (s, e) => gameManager.SaveGame($"save_{DateTime.Now:yyyyMMddHHmmss}");
            pauseMenu.Controls.Add(btnSave);

            Button btnSettings = new Button
            {
                Text = "Settings",
                Font = new Font("Arial", 14),
                Size = new Size(200, 40),
                Location = new Point(50, 150)
            };
            btnSettings.Click += (s, e) =>
            {
                FormSettings settings = new FormSettings();
                settings.ShowDialog();
            };
            pauseMenu.Controls.Add(btnSettings);

            Button btnExitToMenu = new Button
            {
                Text = "Exit to Menu",
                Font = new Font("Arial", 14),
                Size = new Size(200, 40),
                Location = new Point(50, 210)
            };
            btnExitToMenu.Click += (s, e) =>
            {
                this.Close();
                FormMenu mainMenu = new FormMenu();
                mainMenu.Show();
            };
            pauseMenu.Controls.Add(btnExitToMenu);

            this.Controls.Add(pauseMenu);
        }

        private void InitializeGame()
        {
            gameManager = new GameManager(player);
            gameManager.OnGameOver += GameOver;
            gameManager.OnLevelComplete += LevelComplete;

            gameTimer = new Timer { Interval = 16 }; // ~60 FPS
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            player.UpdateMovement(inputState);
            // Update UI
            lblScore.Text = $"Score: {player.Score}";
            lblHealth.Text = $"Health: {player.Health}/{player.MaxHealth}";
            lblLevel.Text = $"Level: {player.Level}";

            // Redraw game
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw player
            player.Draw(e.Graphics);

            // Draw enemies
            foreach (var enemy in gameManager.Enemies)
            {
                enemy.Draw(e.Graphics);
            }

            // Draw bullets
            foreach (var bullet in gameManager.Bullets)
            {
                bullet.Draw(e.Graphics);
            }

            // Draw items
            foreach (var item in gameManager.Items)
            {
                item.Draw(e.Graphics);
            }
        }

        private void FormGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameManager.IsPaused) return;
            //keyStates[e.KeyCode] = true;
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    inputState.MoveUp = true;
                    break;
                case Keys.S:
                case Keys.Down:
                    inputState.MoveDown = true;
                    break;
                case Keys.A:
                case Keys.Left:
                    inputState.MoveLeft = true;
                    break;
                case Keys.D:
                case Keys.Right:
                    inputState.MoveRight = true;
                    break;
                case Keys.Space:
                    // Shoot
                    gameManager.Bullets.AddRange(player.CurrentWeapon.Shoot(player.Position));
                    break;
                case Keys.D1:
                    // Use item 1
                    if (player.Inventory.Items.Count > 0)
                        player.UseItem(player.Inventory.Items[0]);
                    break;
                case Keys.D2:
                    // Use item 2
                    if (player.Inventory.Items.Count > 1)
                        player.UseItem(player.Inventory.Items[1]);
                    break;
            }
        }
        private void FormGame_KeyUp(object sender, KeyEventArgs e)
        {
            //keyStates[e.KeyCode] = false;
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.Up:
                    inputState.MoveUp = false;
                    break;
                case Keys.S:
                case Keys.Down:
                    inputState.MoveDown = false;
                    break;
                case Keys.A:
                case Keys.Left:
                    inputState.MoveLeft = false;
                    break;
                case Keys.D:
                case Keys.Right:
                    inputState.MoveRight = false;
                    break;
            }
        }
        private void BtnPause_Click(object sender, EventArgs e)
        {
            gameManager.PauseGame();
            pauseMenu.Visible = true;
        }

        private void BtnShop_Click(object sender, EventArgs e)
        {
            gameManager.PauseGame();
            FormShop shop = new FormShop(player);
            shop.ShowDialog();
            gameManager.ResumeGame();
        }

        private void BtnInventory_Click(object sender, EventArgs e)
        {
            gameManager.PauseGame();
            FormInventory inventory = new FormInventory(player);
            inventory.ShowDialog();
            gameManager.ResumeGame();
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! Your score: " + player.Score);
            this.Close();
            FormMenu mainMenu = new FormMenu();
            mainMenu.Show();
        }

        private void LevelComplete()
        {
            player.Coins += player.Level * 10;
            MessageBox.Show($"Level {player.Level - 1} completed!\nYou earned {player.Level * 10} coins!");
        }
    }
}
