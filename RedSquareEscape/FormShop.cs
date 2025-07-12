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
        private FlowLayoutPanel flowPanel;
        private Label lblCoins;

        private List<ShopItem> availableItems = new List<ShopItem>()
    {
        new ShopItem("تقویت سلامت", "افزایش سلامت حداکثر 20 واحد", 50, p => p.MaxHealth += 20),
        new ShopItem("تقویت محافظ", "افزایش محافظ حداکثر 30 واحد", 75, p => p.MaxShield += 30),
        new ShopItem("سلاح دوگانه", "توانایی شلیک دوگانه", 150, p => p.Weapons.Add(new Weapon("دوگانه", 8, 0.4f, 500f, WeaponType.Double))),
        new ShopItem("سلاح پخش‌شونده", "شلیک سه تیر با پخش شدگی", 200, p => p.Weapons.Add(new Weapon("پخش‌شونده", 5, 0.5f, 400f, WeaponType.Spread))),
        new ShopItem("بمب", "نابودی تمام دشمنان در صفحه", 80, p => p.Inventory["بمب"] = p.Inventory.ContainsKey("بمب") ? p.Inventory["بمب"] + 1 : 1),
        new ShopItem("کیف بهداشتی", "بازیابی 25 واحد سلامت", 30, p => p.Inventory["کیف بهداشتی"] = p.Inventory.ContainsKey("کیف بهداشتی") ? p.Inventory["کیف بهداشتی"] + 1 : 1)
    };

        public FormShop(Player player)
        {
            this.player = player;
            InitializeComponents();
            this.Text = "فروشگاه";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponents()
        {
            // نمایش سکه‌ها
            lblCoins = new Label()
            {
                Text = $"سکه‌های شما: {player.Coins}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Gold,
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblCoins);

            // پنل آیتم‌ها
            flowPanel = new FlowLayoutPanel()
            {
                Location = new Point(20, 60),
                Size = new Size(740, 460),
                AutoScroll = true,
                WrapContents = true
            };

            // ایجاد کارت‌های آیتم
            foreach (var item in availableItems)
            {
                flowPanel.Controls.Add(CreateItemCard(item));
            }

            this.Controls.Add(flowPanel);

            // دکمه خروج
            var btnExit = new Button()
            {
                Text = "بازگشت به بازی",
                Size = new Size(150, 40),
                Location = new Point(this.ClientSize.Width - 170, this.ClientSize.Height - 60)
            };
            btnExit.Click += (s, e) => this.Close();
            this.Controls.Add(btnExit);
        }

        private Panel CreateItemCard(ShopItem item)
        {
            var panel = new Panel()
            {
                Size = new Size(230, 150),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            // عنوان آیتم
            var lblTitle = new Label()
            {
                Text = item.Name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            // توضیحات
            var lblDesc = new Label()
            {
                Text = item.Description,
                Location = new Point(10, 40),
                Size = new Size(210, 40)
            };

            // قیمت
            var lblPrice = new Label()
            {
                Text = $"{item.Price} سکه",
                ForeColor = Color.Gold,
                Location = new Point(10, 90),
                AutoSize = true
            };

            // دکمه خرید
            var btnBuy = new Button()
            {
                Text = "خرید",
                Tag = item,
                Size = new Size(80, 30),
                Location = new Point(140, 90),
                Enabled = player.Coins >= item.Price
            };
            btnBuy.Click += (s, e) => BuyItem((ShopItem)((Button)s).Tag);

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDesc);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(btnBuy);

            return panel;
        }

        private void BuyItem(ShopItem item)
        {
            if (player.Coins >= item.Price)
            {
                player.Coins -= item.Price;
                item.ApplyEffect(player);
                lblCoins.Text = $"سکه‌های شما: {player.Coins}";

                // غیرفعال کردن دکمه‌های خرید برای آیتم‌های غیرقابل خرید
                foreach (Control control in flowPanel.Controls)
                {
                    if (control is Panel panel)
                    {
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is Button btn && btn.Tag is ShopItem btnItem)
                            {
                                btn.Enabled = player.Coins >= btnItem.Price;
                            }
                        }
                    }
                }

                MessageBox.Show($"{item.Name} خریداری شد!", "خرید موفق");
            }
        }
    }

    public class ShopItem
    {
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
        public Action<Player> ApplyEffect { get; }

        public ShopItem(string name, string description, int price, Action<Player> effect)
        {
            Name = name;
            Description = description;
            Price = price;
            ApplyEffect = effect;
        }
    }
}
