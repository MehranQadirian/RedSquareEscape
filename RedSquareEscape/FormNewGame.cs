using RedSquareEscape.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormNewGame : Form
    {
        private TextBox txtPlayerName;
        private ComboBox cmbPlayerAge;
        private Button btnStart;
        private Button btnBack;

        public FormNewGame()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // عنوان فرم
            Label lblTitle = new Label
            {
                Text = "New Game",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(
                    (this.ClientSize.Width - 200) / 2,
                    this.ClientSize.Height / 4
                )
            };
            this.Controls.Add(lblTitle);

            // فیلد نام بازیکن
            Label lblName = new Label
            {
                Text = "Player Name:",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(
                    (this.ClientSize.Width - 300) / 2,
                    lblTitle.Bottom + 50
                )
            };
            this.Controls.Add(lblName);

            txtPlayerName = new TextBox
            {
                Font = new Font("Arial", 14),
                Size = new Size(300, 30),
                Location = new Point(
                    (this.ClientSize.Width - 300) / 2,
                    lblName.Bottom + 10
                )
            };
            this.Controls.Add(txtPlayerName);

            // فیلد سن بازیکن
            Label lblAge = new Label
            {
                Text = "Player Age:",
                Font = new Font("Arial", 14),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(
                    (this.ClientSize.Width - 300) / 2,
                    txtPlayerName.Bottom + 20
                )
            };
            this.Controls.Add(lblAge);

            cmbPlayerAge = new ComboBox
            {
                Font = new Font("Arial", 14),
                Size = new Size(300, 30),
                Location = new Point(
                    (this.ClientSize.Width - 300) / 2,
                    lblAge.Bottom + 10
                ),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // پر کردن سن‌ها از 8 تا 70
            for (int i = 8; i <= 70; i++)
            {
                cmbPlayerAge.Items.Add(i);
            }
            cmbPlayerAge.SelectedIndex = 12; // Default to 20 years old
            this.Controls.Add(cmbPlayerAge);

            // دکمه شروع
            btnStart = new Button
            {
                Text = "Start Game",
                Font = new Font("Arial", 16),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(70, 70, 70),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 50),
                Location = new Point(
                    (this.ClientSize.Width - 200) / 2,
                    cmbPlayerAge.Bottom + 40
                )
            };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

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

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlayerName.Text))
            {
                MessageBox.Show("Please enter your name");
                return;
            }

            // ایجاد بازیکن جدید
            Player player = new Player
            {
                Name = txtPlayerName.Text,
                Age = (int)cmbPlayerAge.SelectedItem,
                Position = new PointF(400, 300)
            };

            // نمایش آموزش بازی
            FormTutorial tutorial = new FormTutorial();
            if (tutorial.ShowDialog() == DialogResult.OK)
            {
                // شروع بازی
                FormGame gameForm = new FormGame(player);
                gameForm.Show();
                this.Hide();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            FormMenu mainMenu = new FormMenu();
            mainMenu.Show();
        }
    }
}
