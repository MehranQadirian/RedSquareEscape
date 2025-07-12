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
        private Button btnColorPicker;
        private Button btnSave;
        private Button btnCancel;
        private Color selectedColor = Color.Lime;
        private ColorTheme currentTheme;

        public FormSettings()
        {
            InitializeComponents();
            this.Text = "Game Settings";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void InitializeComponents()
        {
            // Theme Selection
            Label lblTheme = new Label();
            lblTheme.Text = "Theme:";
            lblTheme.Location = new Point(20, 20);
            this.Controls.Add(lblTheme);

            cmbTheme = new ComboBox();
            cmbTheme.Items.AddRange(new object[] { "Green/Black", "Yellow/Navy", "Red/Black", "Pink/White" });
            cmbTheme.SelectedIndex = 0;
            cmbTheme.Location = new Point(120, 20);
            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbTheme);

            // Language Selection
            Label lblLanguage = new Label();
            lblLanguage.Text = "Language:";
            lblLanguage.Location = new Point(20, 60);
            this.Controls.Add(lblLanguage);

            cmbLanguage = new ComboBox();
            cmbLanguage.Items.AddRange(new object[] { "English", "Persian" });
            cmbLanguage.SelectedIndex = 0;
            cmbLanguage.Location = new Point(120, 60);
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbLanguage);

            // Player Shape Selection
            Label lblPlayerShape = new Label();
            lblPlayerShape.Text = "Player Shape:";
            lblPlayerShape.Location = new Point(20, 100);
            this.Controls.Add(lblPlayerShape);

            cmbPlayerShape = new ComboBox();
            cmbPlayerShape.Items.AddRange(new object[] { "Square", "Triangle", "Circle" });
            cmbPlayerShape.SelectedIndex = 0;
            cmbPlayerShape.Location = new Point(120, 100);
            cmbPlayerShape.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cmbPlayerShape);

            // Player Color Picker
            Label lblPlayerColor = new Label();
            lblPlayerColor.Text = "Player Color:";
            lblPlayerColor.Location = new Point(20, 140);
            this.Controls.Add(lblPlayerColor);

            btnColorPicker = new Button();
            btnColorPicker.Text = "Choose Color";
            btnColorPicker.BackColor = selectedColor;
            btnColorPicker.Location = new Point(120, 140);
            btnColorPicker.Click += BtnColorPicker_Click;
            this.Controls.Add(btnColorPicker);

            // Save Button
            btnSave = new Button();
            btnSave.Text = "Save";
            btnSave.Location = new Point(100, 300);
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(200, 300);
            btnCancel.Click += BtnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void BtnColorPicker_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedColor = colorDialog.Color;
                btnColorPicker.BackColor = selectedColor;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Save settings to application properties
            Properties.Settings.Default.Theme = cmbTheme.SelectedIndex;
            Properties.Settings.Default.Language = cmbLanguage.SelectedIndex;
            Properties.Settings.Default.PlayerShape = cmbPlayerShape.SelectedIndex;
            Properties.Settings.Default.PlayerColor = selectedColor;
            Properties.Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public ColorTheme GetSelectedTheme()
        {
            return currentTheme;
        }

        public PlayerShape GetSelectedShape()
        {
            return (PlayerShape)cmbPlayerShape.SelectedIndex;
        }

        public Color GetSelectedColor()
        {
            return selectedColor;
        }

        public string GetSelectedLanguage()
        {
            return cmbLanguage.SelectedItem.ToString();
        }
    }
}
