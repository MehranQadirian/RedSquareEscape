using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using RedSquareEscape.Classes;

namespace RedSquareEscape
{
    public partial class FormShop : Form
    {
        private Player player;
        private FlowLayoutPanel flpItems;
        private Label lblCoins;
        private Button btnClose;

        private List<ShopItem> shopItems = new List<ShopItem>();

        public FormShop(Player player)
        {
            this.player = player;
            InitializeComponents();
            LoadShopItems();
        }

        private void InitializeComponents()
        {
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 600);
            this.Text = "Shop";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // عنوان فروشگاه
            Label lblTitle = new Label
            {
                Text = "Shop",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(134, 253, 233),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(lblTitle);

            // نمایش سکه‌ها
            lblCoins = new Label
            {
                Text = $"Coins: {player.Coins}",
                Font = new Font("Arial", 14),
                ForeColor = Color.Gold,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 150, 20)
            };
            this.Controls.Add(lblCoins);

            // پنل آیتم‌ها
            flpItems = new FlowLayoutPanel
            {
                AutoScroll = true,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(760, 450),
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

        private void LoadShopItems()
        {
            // ایجاد آیتم‌های فروشگاه
            shopItems.Add(new ShopItem
            {
                Name = "Health Potion",
                Description = "Restores 20 health",
                Price = 30,
                Type = ItemType.HealthPotion,
                Value = 20
            });

            shopItems.Add(new ShopItem
            {
                Name = "Shield Potion",
                Description = "Restores 15 shield",
                Price = 40,
                Type = ItemType.ShieldPotion,
                Value = 15
            });

            shopItems.Add(new ShopItem
            {
                Name = "Bomb",
                Description = "Deals damage to all enemies",
                Price = 50,
                Type = ItemType.Bomb,
                Value = 0
            });

            shopItems.Add(new ShopItem
            {
                Name = "Freeze",
                Description = "Freezes enemies for 3 seconds",
                Price = 60,
                Type = ItemType.Freeze,
                Value = 0
            });

            // نمایش آیتم‌ها
            foreach (var item in shopItems)
            {
                AddShopItemToPanel(item);
            }
        }

        private void AddShopItemToPanel(ShopItem shopItem)
        {
            Panel itemPanel = new Panel
            {
                BackColor = Color.FromArgb(70, 70, 70),
                Size = new Size(240, 150),
                Margin = new Padding(10)
            };

            // نام آیتم
            Label lblName = new Label
            {
                Text = shopItem.Name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            itemPanel.Controls.Add(lblName);

            // توضیحات آیتم
            Label lblDesc = new Label
            {
                Text = shopItem.Description,
                Font = new Font("Arial", 10),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(10, 40),
                MaximumSize = new Size(220, 0)
            };
            itemPanel.Controls.Add(lblDesc);

            // قیمت آیتم
            Label lblPrice = new Label
            {
                Text = $"Price: {shopItem.Price} coins",
                Font = new Font("Arial", 11),
                ForeColor = Color.Gold,
                AutoSize = true,
                Location = new Point(10, 90)
            };
            itemPanel.Controls.Add(lblPrice);

            // دکمه خرید
            Button btnBuy = new Button
            {
                Text = "Buy",
                Font = new Font("Arial", 10),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(80, 30),
                Location = new Point(150, 110),
                Tag = shopItem
            };
            btnBuy.Click += BtnBuy_Click;
            itemPanel.Controls.Add(btnBuy);

            flpItems.Controls.Add(itemPanel);
        }

        private void BtnBuy_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ShopItem item = (ShopItem)btn.Tag;

            if (player.Coins >= item.Price)
            {
                player.Coins -= item.Price;
                lblCoins.Text = $"Coins: {player.Coins}";

                // اضافه کردن آیتم به اینونتوری بازیکن
                player.Inventory.AddItem(new Item(new PointF(0, 0), item.Type, item.Value));

                MessageBox.Show($"{item.Name} purchased successfully!");
            }
            else
            {
                MessageBox.Show("Not enough coins!");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class ShopItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ItemType Type { get; set; }
        public float Value { get; set; }
    }
}
