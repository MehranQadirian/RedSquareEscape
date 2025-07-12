using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using RedSquareEscape.Classes;

namespace RedSquareEscape
{
    public partial class FormInventory : Form
    {
        private Player player;
        private FlowLayoutPanel flpItems;
        private Button btnClose;

        public FormInventory(Player player)
        {
            this.player = player;
            InitializeComponents();
            LoadInventoryItems();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(600, 500);
            this.Text = "Inventory";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // عنوان موجودی
            Label lblTitle = new Label
            {
                Text = "Inventory",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(lblTitle);

            // پنل آیتم‌ها
            flpItems = new FlowLayoutPanel
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(560, 380),
                Location = new Point(20, 70)
            };
            this.Controls.Add(flpItems);

            // دکمه بستن
            btnClose = new Button
            {
                Text = "Close",
                Font = new Font("Arial", 14),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(
                    (this.ClientSize.Width - 100) / 2,
                    flpItems.Bottom + 20
                )
            };
            btnClose.Click += BtnClose_Click;
            this.Controls.Add(btnClose);
        }

        private void LoadInventoryItems()
        {
            foreach (var item in player.Inventory.Items)
            {
                AddItemToPanel(item);
            }
        }

        private void AddItemToPanel(Item item)
        {
            Panel itemPanel = new Panel
            {
                BackColor = Color.FromArgb(70, 70, 70),
                Size = new Size(240, 120),
                Margin = new Padding(10)
            };

            // نام آیتم
            Label lblName = new Label
            {
                Text = item.Name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            itemPanel.Controls.Add(lblName);

            // توضیحات آیتم
            Label lblDesc = new Label
            {
                Text = item.Description,
                Font = new Font("Arial", 10),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(10, 40),
                MaximumSize = new Size(220, 0)
            };
            itemPanel.Controls.Add(lblDesc);

            // دکمه استفاده
            Button btnUse = new Button
            {
                Text = "Use",
                Font = new Font("Arial", 10),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 30),
                Location = new Point(150, 80),
                Tag = item
            };
            btnUse.Click += BtnUse_Click;
            itemPanel.Controls.Add(btnUse);

            flpItems.Controls.Add(itemPanel);
        }

        private void BtnUse_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Item item = (Item)btn.Tag;

            player.UseItem(item);
            flpItems.Controls.Clear();
            LoadInventoryItems();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
