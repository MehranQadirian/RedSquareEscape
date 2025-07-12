using RedSquareEscape.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RedSquareEscape
{
    public partial class FormSettings : Form
    {
        private ComboBox cmbTheme;
        private ComboBox cmbLanguage;
        private ComboBox cmbPlayerShape;
        private ColorDialog colorDialog;
        private Button btnColor;
        private Button btnSave;
        private Button btnCancel;

        private Color currentPlayerColor = Color.Red;
        private Theme currentTheme = Theme.DarkGreen;

        public FormSettings()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 400);
            this.Text = "Game Settings";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // عنوان فرم
            Label lblTitle = new Label
            {
                Text = "Settings",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(lblTitle);

            // تم بازی
            Label lblTheme = new Label
            {
                Text = "Theme:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 70)
            };
            this.Controls.Add(lblTheme);

            cmbTheme = new ComboBox
            {
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Dark Green", "Dark Blue", "Dark Red", "Pink White" },
                SelectedIndex = 0,
                Size = new Size(200, 30),
                Location = new Point(150, 70)
            };
            this.Controls.Add(cmbTheme);

            // زبان بازی
            Label lblLanguage = new Label
            {
                Text = "Language:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 120)
            };
            this.Controls.Add(lblLanguage);

            cmbLanguage = new ComboBox
            {
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "English", "Persian" },
                SelectedIndex = 0,
                Size = new Size(200, 30),
                Location = new Point(150, 120)
            };
            this.Controls.Add(cmbLanguage);

            // شکل بازیکن
            Label lblShape = new Label
            {
                Text = "Player Shape:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 170)
            };
            this.Controls.Add(lblShape);

            cmbPlayerShape = new ComboBox
            {
                Font = new Font("Arial", 12),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Square", "Triangle", "Circle" },
                SelectedIndex = 0,
                Size = new Size(200, 30),
                Location = new Point(150, 170)
            };
            this.Controls.Add(cmbPlayerShape);

            // رنگ بازیکن
            Label lblColor = new Label
            {
                Text = "Player Color:",
                Font = new Font("Arial", 12),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 220)
            };
            this.Controls.Add(lblColor);

            btnColor = new Button
            {
                Text = "Choose Color",
                Font = new Font("Arial", 12),
                BackColor = currentPlayerColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 30),
                Location = new Point(150, 220)
            };
            btnColor.Click += BtnColor_Click;
            this.Controls.Add(btnColor);

            colorDialog = new ColorDialog
            {
                AnyColor = true,
                Color = currentPlayerColor
            };

            // دکمه ذخیره
            btnSave = new Button
            {
                Text = "Save",
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(150, 280)
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // دکمه انصراف
            btnCancel = new Button
            {
                Text = "Cancel",
                Font = new Font("Arial", 12),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(260, 280)
            };
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void BtnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                currentPlayerColor = colorDialog.Color;
                btnColor.BackColor = currentPlayerColor;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // ذخیره تنظیمات
            currentTheme = (Theme)cmbTheme.SelectedIndex;

            // اعمال تغییرات
            ApplyTheme(currentTheme);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ApplyTheme(Theme theme)
        {
            // اینجا می‌توانید تم بازی را تغییر دهید
            switch (theme)
            {
                case Theme.DarkGreen:
                    // اعمال تم سبز و مشکی
                    break;
                case Theme.DarkBlue:
                    // اعمال تم زرد و آبی نفتی
                    break;
                case Theme.DarkRed:
                    // اعمال تم قرمز و مشکی
                    break;
                case Theme.PinkWhite:
                    // اعمال تم صورتی و سفید
                    break;
            }
        }
    }

    public enum Theme
    {
        DarkGreen,
        DarkBlue,
        DarkRed,
        PinkWhite
    }
}
