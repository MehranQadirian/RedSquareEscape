using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormTutorial : Form
    {
        private Button btnOK;

        public FormTutorial()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 500);
            this.Text = "Game Tutorial";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // عنوان آموزش
            Label lblTitle = new Label
            {
                Text = "How to Play",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(lblTitle);

            // محتوای آموزش
            string tutorialText = @"Welcome to Red Square Escape!

Controls:
- WASD or Arrow Keys: Move your player
- Space: Shoot at enemies
- 1-2: Use items from your inventory
- P: Pause the game

Gameplay:
- Avoid enemies and shoot them to earn points
- Collect coins to buy items in the shop
- Every 3 levels you'll face a boss
- Use items strategically to survive longer

Items:
- Health Potion: Restores health
- Shield Potion: Restores shield
- Bomb: Damages all enemies
- Freeze: Temporarily stops enemies

Good luck!";

            Label lblTutorial = new Label
            {
                Text = tutorialText,
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(560, 350),
                Location = new Point(20, 70)
            };
            this.Controls.Add(lblTutorial);

            // دکمه تأیید
            btnOK = new Button
            {
                Text = "I Understand",
                Font = new Font("Arial", 14),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 40),
                Location = new Point(
                    (this.ClientSize.Width - 150) / 2,
                    lblTutorial.Bottom + 20
                )
            };
            btnOK.Click += BtnOK_Click;
            this.Controls.Add(btnOK);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
