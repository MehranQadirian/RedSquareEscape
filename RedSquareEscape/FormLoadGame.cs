using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using RedSquareEscape.Classes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RedSquareEscape
{
    public partial class FormLoadGame : Form
    {
        private ListBox lstSaveGames;
        private Button btnLoad;
        private Button btnDelete;
        private Button btnBack;
        private List<GameSave> saveGames = new List<GameSave>();

        public FormLoadGame()
        {
            InitializeComponents();
            LoadSaveGames();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // عنوان فرم
            Label lblTitle = new Label
            {
                Text = "Load Game",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(
                    (this.ClientSize.Width - 200) / 2,
                    this.ClientSize.Height / 6
                )
            };
            this.Controls.Add(lblTitle);

            // لیست بازی‌های ذخیره شده
            lstSaveGames = new ListBox
            {
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Size = new Size(600, 300),
                Location = new Point(
                    (this.ClientSize.Width - 600) / 2,
                    lblTitle.Bottom + 30
                )
            };
            this.Controls.Add(lstSaveGames);

            // دکمه بارگذاری
            btnLoad = new Button
            {
                Text = "Load Selected",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Location = new Point(
                    (this.ClientSize.Width - 450) / 2,
                    lstSaveGames.Bottom + 30
                )
            };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            // دکمه حذف
            btnDelete = new Button
            {
                Text = "Delete Selected",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Location = new Point(
                    btnLoad.Right + 50,
                    lstSaveGames.Bottom + 30
                )
            };
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // دکمه بازگشت
            btnBack = new Button
            {
                Text = "Back",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(20, 20)
            };
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }

        private void LoadSaveGames()
        {
            // این بخش باید با سیستم ذخیره واقعی جایگزین شود
            saveGames.Add(new GameSave
            {
                SaveName = "Save 1",
                SaveDate = DateTime.Now.AddDays(-1),
                Progress = 45,
                StageName = "Level 3",
                PlayerName = "Player1"
            });

            saveGames.Add(new GameSave
            {
                SaveName = "Save 2",
                SaveDate = DateTime.Now.AddHours(-3),
                Progress = 78,
                StageName = "Level 5",
                PlayerName = "Player2"
            });

            foreach (var save in saveGames)
            {
                lstSaveGames.Items.Add($"{save.SaveName} - {save.PlayerName} - {save.StageName} - {save.Progress}% - {save.SaveDate}");
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (lstSaveGames.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a save game");
                return;
            }

            // اینجا باید بازی واقعی بارگذاری شود
            GameSave selectedSave = saveGames[lstSaveGames.SelectedIndex];

            // برای نمونه، یک بازیکن جدید ایجاد می‌کنیم
            Player player = new Player
            {
                Name = selectedSave.PlayerName,
                Position = new PointF(400, 300)
            };

            FormGame gameForm = new FormGame(player);
            gameForm.Show();
            this.Hide();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstSaveGames.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a save game to delete");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this save game?",
                "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                saveGames.RemoveAt(lstSaveGames.SelectedIndex);
                lstSaveGames.Items.RemoveAt(lstSaveGames.SelectedIndex);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FormMenu mainMenu = new FormMenu();
            mainMenu.Show();
        }
    }

    public class GameSave
    {
        public string SaveName { get; set; }
        public DateTime SaveDate { get; set; }
        public int Progress { get; set; }
        public string StageName { get; set; }
        public string PlayerName { get; set; }
    }
}
