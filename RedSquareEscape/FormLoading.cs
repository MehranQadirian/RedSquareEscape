using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace RedSquareEscape
{
    public partial class FormLoading : Form
    {
        private Label lblTitle;
        private Label lblDeveloper;
        private Label lblLoading;
        private System.Windows.Forms.Timer timer;
        private int dotCount = 0;
        private ProgressBar progressBar;
        private PictureBox loadingSpinner;
        private float rotationAngle = 0f;

        public FormLoading()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // تنظیمات اصلی فرم
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;

            // پس‌زمینه گرادیان
            this.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(30, 30, 30),
                    Color.FromArgb(70, 70, 70),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // عنوان بازی با سایه
            lblTitle = new Label
            {
                Text = "RED SQUARE ESCAPE",
                Font = new Font("Arial", 48, FontStyle.Bold),
                ForeColor = Color.FromArgb(86, 86, 86),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblTitle.Location = new Point(
                (this.ClientSize.Width) * 2 + 150,
                this.ClientSize.Height * 3 - 25
            );

            // اضافه کردن سایه به متن
            Label lblTitleShadow = new Label
            {
                Text = lblTitle.Text,
                Font = lblTitle.Font,
                ForeColor = Color.FromArgb(100, 0, 0, 0),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(lblTitle.Left + 5, lblTitle.Top + 5)
            };
            this.Controls.Add(lblTitleShadow);
            this.Controls.Add(lblTitle);
            
            // اطلاعات توسعه دهنده
            lblDeveloper = new Label
            {
                Text = "Developed by: Mehran Qadirian\n" +
                       "Email: Mehranghadirian01@gmail.com\n" +
                       "Phone: +989023181330\n" +
                       "Telegram: @bi_buk\n" +
                       "C# Windows Form Application",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.BottomLeft
            };
            lblDeveloper.Location = new Point(30, this.ClientSize.Height * 2 +100);
            this.Controls.Add(lblDeveloper);
            // متن لودینگ
            lblLoading = new Label
            {
                Text = "Loading",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                BackColor = Color.Transparent,
                Location = new Point(
                    (this.ClientSize.Width - 100) / 2,
                    lblDeveloper.Bottom + 30
                )
            };
            this.Controls.Add(lblLoading);
            // اسپینر لودینگ (دایره چرخان)
            loadingSpinner = new PictureBox
            {
                Size = new Size(60, 60),
                BackColor = Color.Transparent,
                Location = new Point(
                    (lblLoading.Left + 20) / 2,
                    lblDeveloper.Bottom + 30
                )
            };
            loadingSpinner.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Pen pen = new Pen(Color.FromArgb(134, 253, 233), 5);
                e.Graphics.DrawArc(pen, 5, 5, 15, 15, rotationAngle, 270);
                pen.Dispose();
            };
            this.Controls.Add(loadingSpinner);
            // نوار پیشرفت
            progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Continuous,
                Size = new Size(200, 5),
                Location = new Point(
                    lblLoading.Right + 20,
                    lblLoading.Top + lblLoading.Height / 2
                ),
                ForeColor = Color.FromArgb(255, 77, 249),
                BackColor = Color.FromArgb(70, 70, 70)
            };
            this.Controls.Add(progressBar);
            // تایمر برای انیمیشن‌ها
            timer = new System.Windows.Forms.Timer { Interval = 50 };
            timer.Tick += Timer_Tick;
            timer.Start();

            // شبیه‌سازی فرآیند لودینگ
            Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                {
                    Thread.Sleep(50);
                    this.Invoke(new Action(() =>
                    {
                        progressBar.Value = i;
                    }));
                }

                this.Invoke(new Action(() =>
                {
                    timer.Stop();
                    FormMenu mainMenu = new FormMenu();
                    mainMenu.Show();
                    this.Hide();
                }));
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // انیمیشن نقاط لودینگ
            dotCount = (dotCount + 1) % 4;
            lblLoading.Text = "Loading" + new string('.', dotCount);

            // انیمیشن چرخش اسپینر
            rotationAngle = (rotationAngle + 10) % 360;
            loadingSpinner.Invalidate();

            // انیمیشن پالس برای عنوان
            if (DateTime.Now.Millisecond % 1000 < 500)
            {
                lblTitle.ForeColor = Color.FromArgb(255, 77, 249);
            }
            else
            {
                lblTitle.ForeColor = Color.FromArgb(134, 253, 233);
            }
        }
    }
}