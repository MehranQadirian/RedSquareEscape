using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormPause : Form
    {
        public FormPause()
        {
            InitializeComponents();
            this.Text = "Game Paused";
            this.Size = new Size(300, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void InitializeComponents()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "Game Paused",
                Font = new Font("Arial", 24),
                Location = new Point(50, 20),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Buttons
            var buttons = new[]
            {
            new { Text = "Resume", DialogResult = DialogResult.OK },
            new { Text = "Save Game", DialogResult = DialogResult.Retry },
            new { Text = "Settings", DialogResult = DialogResult.Ignore },
            new { Text = "Exit to Menu", DialogResult = DialogResult.Cancel },
            new { Text = "Exit Game", DialogResult = DialogResult.Abort }
        };

            for (int i = 0; i < buttons.Length; i++)
            {
                var btn = new Button
                {
                    Text = buttons[i].Text,
                    DialogResult = buttons[i].DialogResult,
                    Size = new Size(200, 40),
                    Location = new Point(50, 80 + i * 50)
                };
                btn.Click += (s, e) => this.DialogResult = btn.DialogResult;
                this.Controls.Add(btn);
            }
        }
    }
}
