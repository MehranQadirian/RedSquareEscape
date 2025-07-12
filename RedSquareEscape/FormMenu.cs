using RedSquareEscape.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponents();
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeComponents()
        {
            // عنوان بازی
            var lblTitle = new Label()
            {
                Text = "RedSquareEscape",
                Font = new Font("Arial", 48, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 200, 200),
                AutoSize = true,
                Location = new Point(this.ClientSize.Width / 2 - 200, 100)
            };

            // دکمه‌های منو
            var btnNewGame = new MenuButton("شروع جدید", new Point(this.ClientSize.Width / 2 - 100, 250),
                () => StartNewGame());

            var btnLoadGame = new MenuButton("بارگذاری بازی", new Point(this.ClientSize.Width / 2 - 100, 320),
                () => LoadGame());

            var btnSettings = new MenuButton("تنظیمات", new Point(this.ClientSize.Width / 2 - 100, 390),
                () => ShowSettings());

            var btnExit = new MenuButton("خروج", new Point(this.ClientSize.Width / 2 - 100, 460),
                () => Application.Exit());

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnNewGame);
            this.Controls.Add(btnLoadGame);
            this.Controls.Add(btnSettings);
            this.Controls.Add(btnExit);
        }

        private void StartNewGame()
        {
            var form = new FormCharacterCreation();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.Hide();
                new FormGame(form.CreatedPlayer).Show();
            }
        }

        private void LoadGame()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "Save Files (*.sav)|*.sav"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var player = SaveManager.LoadGame(dialog.FileName);
                this.Hide();
                new FormGame(player).Show();
            }
        }

        private void ShowSettings()
        {
            new FormSettings().ShowDialog();
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
