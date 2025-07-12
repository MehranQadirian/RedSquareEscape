using RedSquareEscape.Classes;
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
    public partial class FormCharacterCreation : Form
    {
        private TextBox txtName;
        private ComboBox cmbAppearance;
        private Button btnCreate;

        public Player CreatedPlayer { get; private set; }

        public FormCharacterCreation()
        {
            InitializeComponents();
            this.Text = "ساخت شخصیت";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponents()
        {
            // نام بازیکن
            var lblName = new Label()
            {
                Text = "نام بازیکن:",
                Location = new Point(50, 50),
                AutoSize = true
            };

            txtName = new TextBox()
            {
                Location = new Point(150, 50),
                Size = new Size(200, 30)
            };

            // ظاهر بازیکن
            var lblAppearance = new Label()
            {
                Text = "ظاهر:",
                Location = new Point(50, 100),
                AutoSize = true
            };

            cmbAppearance = new ComboBox()
            {
                Location = new Point(150, 100),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbAppearance.Items.AddRange(new object[] { "پیش‌فرض", "جنگجو", "نینجا", "سایبورگ" });
            cmbAppearance.SelectedIndex = 0;

            // دکمه ایجاد
            btnCreate = new Button()
            {
                Text = "ایجاد شخصیت",
                Location = new Point(150, 180),
                Size = new Size(200, 40)
            };
            btnCreate.Click += (s, e) => CreateCharacter();

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblAppearance);
            this.Controls.Add(cmbAppearance);
            this.Controls.Add(btnCreate);
        }

        private void CreateCharacter()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("لطفاً نامی برای بازیکن انتخاب کنید");
                return;
            }

            CreatedPlayer = new Player(txtName.Text)
            {
                Appearance = (PlayerAppearance)cmbAppearance.SelectedIndex
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
