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
    public partial class FormGameOver : Form
    {
        public FormGameOver(int score, int coins, int level, int enemiesKilled)
        {
            InitializeComponents(score, coins, level, enemiesKilled);
            this.Text = "پایان بازی";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void InitializeComponents(int score, int coins, int level, int enemiesKilled)
        {
            // عنوان
            var lblTitle = new Label()
            {
                Text = "پایان بازی",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width / 2 - 80, 30)
            };

            // آمار بازی
            var stats = new string[]
            {
            $"امتیاز نهایی: {score}",
            $"سکه‌های جمع‌آوری شده: {coins}",
            $"مرحله رسیده: {level}",
            $"دشمنان شکست خورده: {enemiesKilled}"
            };

            for (int i = 0; i < stats.Length; i++)
            {
                var lblStat = new Label()
                {
                    Text = stats[i],
                    Font = new Font("Arial", 14),
                    AutoSize = true,
                    Location = new Point(50, 100 + i * 40)
                };
                this.Controls.Add(lblStat);
            }

            // دکمه‌ها
            var btnRestart = new Button()
            {
                Text = "شروع مجدد",
                Size = new Size(150, 40),
                Location = new Point(50, 280)
            };
            btnRestart.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            };

            var btnMenu = new Button()
            {
                Text = "منوی اصلی",
                Size = new Size(150, 40),
                Location = new Point(220, 280)
            };
            btnMenu.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            };

            var btnExit = new Button()
            {
                Text = "خروج",
                Size = new Size(150, 40),
                Location = new Point(390, 280)
            };
            btnExit.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnRestart);
            this.Controls.Add(btnMenu);
            this.Controls.Add(btnExit);
        }
    }
}
