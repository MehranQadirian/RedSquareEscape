using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace RedSquareEscape
{
    public partial class FormLoading : Form
    {
        private Label lblGameName;
        private Label lblDeveloper;
        private Label lblLoading;
        private System.Windows.Forms.Timer loadingTimer;
        private int dotCount = 0;

        public FormLoading()
        {
            InitializeComponents();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
        }

        private void InitializeComponents()
        {
            // Game Name Label
            lblGameName = new Label();
            lblGameName.Text = "RedSquareEscape";
            lblGameName.Font = new Font("Arial", 48, FontStyle.Bold);
            lblGameName.ForeColor = Color.White;
            lblGameName.AutoSize = true;
            lblGameName.Location = new Point(
                this.ClientSize.Width / 2 - lblGameName.Width / 2,
                this.ClientSize.Height / 2 - lblGameName.Height / 2
            );
            this.Controls.Add(lblGameName);

            // Developer Info
            lblDeveloper = new Label();
            lblDeveloper.Text = "Developer : \n" +
                                "Name : Mehran Ghadirian\n" +
                                "Email: Mehranghadirian01@gmail.com\n" +
                                "Phone: +989023181330\n" +
                                "Telegram: @bi_buk\n" +
                                "Developed with C# Windows Form";
            lblDeveloper.Font = new Font("Arial", 12);
            lblDeveloper.ForeColor = Color.White;
            lblDeveloper.AutoSize = true;
            lblDeveloper.Location = new Point(this.ClientSize.Width + 120 - lblGameName.Width / 2, this.ClientSize.Height);
            this.Controls.Add(lblDeveloper);

            // Loading Label
            lblLoading = new Label();
            lblLoading.Text = "Loading";
            lblLoading.Font = new Font("Arial", 24);
            lblLoading.ForeColor = Color.White;
            lblLoading.AutoSize = true;
            lblLoading.Location = new Point(this.ClientSize.Width + 1000, this.ClientSize.Height + 550);
            this.Controls.Add(lblLoading);

            // Loading Timer
            loadingTimer = new Timer();
            loadingTimer.Interval = 500;
            loadingTimer.Tick += LoadingTimer_Tick;
            loadingTimer.Start();
        }

        private void LoadingTimer_Tick(object sender, EventArgs e)
        {
            dotCount = (dotCount + 1) % 4;
            lblLoading.Text = "Loading" + new string('.', dotCount);

            if (dotCount == 3)
            {
                loadingTimer.Stop();
                this.Hide();
                FormMenu menu = new FormMenu();
                menu.Show();
            }
        }
    }
}
