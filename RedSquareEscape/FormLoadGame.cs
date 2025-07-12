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
        private ListBox lstSaves;
        private Button btnLoad;
        private Button btnDelete;

        private List<GameState> savedGames = new List<GameState>();

        public FormLoadGame()
        {
            InitializeComponents();
            this.Text = "Load Game";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            LoadSavedGames();
        }

        private void InitializeComponents()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "Load Saved Game",
                Font = new Font("Arial", 24),
                Location = new Point(50, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Saves list
            lstSaves = new ListBox
            {
                Size = new Size(400, 200),
                Location = new Point(50, 80)
            };
            lstSaves.SelectedIndexChanged += LstSaves_SelectedIndexChanged;
            this.Controls.Add(lstSaves);

            // Load button
            btnLoad = new Button
            {
                Text = "Load",
                Enabled = false,
                Size = new Size(100, 40),
                Location = new Point(100, 300)
            };
            btnLoad.Click += BtnLoad_Click;
            this.Controls.Add(btnLoad);

            // Delete button
            btnDelete = new Button
            {
                Text = "Delete",
                Enabled = false,
                Size = new Size(100, 40),
                Location = new Point(250, 300)
            };
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // Back button
            var btnBack = new Button
            {
                Text = "Back",
                Size = new Size(100, 40),
                Location = new Point(400, 300)
            };
            btnBack.Click += (s, e) => this.Close();
            this.Controls.Add(btnBack);
        }

        private void LoadSavedGames()
        {
            // In a real game, this would load from disk
            savedGames = new List<GameState>
        {
            new GameState(
                new Player("Player1", 25, new PointF(400, 300), PlayerShape.Square, Color.Lime),
                3, "Level 3") { Score = 450, Coins = 120 },
            new GameState(
                new Player("Player2", 30, new PointF(400, 300), PlayerShape.Circle, Color.Blue),
                5, "Boss Fight") { Score = 780, Coins = 200 }
        };

            lstSaves.Items.Clear();
            foreach (var save in savedGames)
            {
                lstSaves.Items.Add($"{save.StageName} - {save.SaveTime} - {save.ProgressPercent}%");
            }
        }

        private void LstSaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lstSaves.SelectedIndex >= 0;
            btnLoad.Enabled = hasSelection;
            btnDelete.Enabled = hasSelection;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (lstSaves.SelectedIndex >= 0)
            {
                var selectedGame = savedGames[lstSaves.SelectedIndex];

                this.Hide();
                new FormGame(
                    selectedGame.Player,
                    selectedGame.CurrentLevel,
                    selectedGame.StageName
                ).Show();

                this.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (lstSaves.SelectedIndex >= 0 &&
                MessageBox.Show("Delete this saved game?", "Confirm",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                savedGames.RemoveAt(lstSaves.SelectedIndex);
                lstSaves.Items.RemoveAt(lstSaves.SelectedIndex);
            }
        }
    }

    [Serializable]
    public class SavedGame
    {
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public int Health { get; set; }
        public DateTime SaveTime { get; set; }
        public Dictionary<string, int> Inventory { get; set; }

        public void SaveToFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static SavedGame LoadFromFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (SavedGame)formatter.Deserialize(fs);
            }
        }
    }
}
