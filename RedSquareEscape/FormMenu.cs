using RedSquareEscape.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormMenu : Form
    {
        private Button btnNewGame;
        private Button btnLoadGame;
        private Button btnSettings;
        private Button btnExit;
        private Label lblTitle;

        public FormMenu()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // عنوان بازی
            lblTitle = new Label
            {
                Text = "RED SQUARE ESCAPE",
                Font = new Font("Arial", 60, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblTitle.Location = new Point(
                (this.ClientSize.Width) / 2 + 250 - (lblTitle.Width / 2),
                this.ClientSize.Height / 2 + 100
            );
            this.Controls.Add(lblTitle);

            // دکمه شروع بازی جدید
            btnNewGame = new Button
            {
                Text = "New Game",
                Font = new Font("Arial", 18),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 50)
            };
            btnNewGame.Location = new Point(
                (this.ClientSize.Width) / 2 + 490,
                lblTitle.Bottom + 100
            );
            btnNewGame.Click += BtnNewGame_Click;
            this.Controls.Add(btnNewGame);

            // دکمه بارگذاری بازی
            btnLoadGame = new Button
            {
                Text = "Load Game",
                Font = new Font("Arial", 18),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 50)
            };
            btnLoadGame.Location = new Point(
                (this.ClientSize.Width) / 2 + 490,
                btnNewGame.Bottom + 20
            );
            btnLoadGame.Click += BtnLoadGame_Click;
            this.Controls.Add(btnLoadGame);

            // دکمه تنظیمات
            btnSettings = new Button
            {
                Text = "Settings",
                Font = new Font("Arial", 18),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 50)
            };
            btnSettings.Location = new Point(
                (this.ClientSize.Width) / 2 + 490,
                btnLoadGame.Bottom + 20
            );
            btnSettings.Click += BtnSettings_Click;
            this.Controls.Add(btnSettings);

            // دکمه خروج
            btnExit = new Button
            {
                Text = "Exit",
                Font = new Font("Arial", 18),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(300, 50)
            };
            btnExit.Location = new Point(
                (this.ClientSize.Width) / 2 + 490,
                btnSettings.Bottom + 20
            );
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            FormNewGame newGameForm = new FormNewGame();
            newGameForm.Show();
            this.Hide();
        }

        private void BtnLoadGame_Click(object sender, EventArgs e)
        {
            FormLoadGame loadGameForm = new FormLoadGame();
            loadGameForm.Show();
            this.Hide();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            FormSettings settingsForm = new FormSettings();
            settingsForm.ShowDialog();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    public class MenuButton : Button
    {
        public MenuButton(string text, Point location, Action onClick)
        {
            this.Text = text;
            this.Location = location;
            this.Size = new Size(200, 50);
            this.BackColor = Color.FromArgb(70, 70, 70);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 14);
            this.FlatStyle = FlatStyle.Flat;
            this.Click += (s, e) => onClick();
        }
    }
}
