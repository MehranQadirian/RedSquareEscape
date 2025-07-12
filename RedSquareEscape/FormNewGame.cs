using RedSquareEscape.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormNewGame : Form
    {
        private TextBox txtPlayerName;
        private ComboBox cmbAge;
        private Button btnStart;
        private Button btnBack;

        public FormNewGame()
        {
            InitializeComponents();
            this.Text = "New Game";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void InitializeComponents()
        {
            // Player Name
            var lblName = new Label
            {
                Text = "Player Name:",
                Location = new Point(50, 50),
                AutoSize = true
            };
            this.Controls.Add(lblName);

            txtPlayerName = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(200, 30)
            };
            this.Controls.Add(txtPlayerName);

            // Player Age
            var lblAge = new Label
            {
                Text = "Age:",
                Location = new Point(50, 100),
                AutoSize = true
            };
            this.Controls.Add(lblAge);

            cmbAge = new ComboBox
            {
                Location = new Point(150, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 8; i <= 70; i++) cmbAge.Items.Add(i);
            cmbAge.SelectedIndex = 12; // Default to 20 years
            this.Controls.Add(cmbAge);

            // Buttons
            btnStart = new Button
            {
                Text = "Start",
                Location = new Point(100, 180),
                Size = new Size(80, 30)
            };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(220, 180),
                Size = new Size(80, 30)
            };
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Please enter your name!");
                return;
            }

            // Show tutorial for first-time players
            ShowTutorial();

            // Get settings
            var settings = new FormSettings();
            if (settings.ShowDialog() == DialogResult.OK)
            {
                var player = new Player(
                    txtPlayerName.Text,
                    (int)cmbAge.SelectedItem,
                    new PointF(400, 300),
                    settings.GetSelectedShape(),
                    settings.GetSelectedColor()
                );

                this.Hide();
                new FormGame(player, 1, "Level 1").Show();
            }
        }

        private void ShowTutorial()
        {
            string tutorial = "Welcome to RedSquareEscape!\n\n" +
                             "Controls:\n" +
                             "- Move: WASD or Arrow Keys\n" +
                             "- Shoot: Space\n" +
                             "- Items: 1-9 keys\n" +
                             "- Pause: P or ESC\n\n" +
                             "Goal: Defeat all enemies in each level!";

            MessageBox.Show(tutorial, "Tutorial", MessageBoxButtons.OK);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
