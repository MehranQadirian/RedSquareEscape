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
        private ListBox lstItems;
        private Label lblDescription;

        public FormInventory(Player player)
        {
            this.player = player;
            InitializeComponents();
            this.Text = "Inventory";
            this.Size = new Size(400, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponents()
        {
            // Items List
            lstItems = new ListBox
            {
                Size = new Size(150, 250),
                Location = new Point(20, 50)
            };
            lstItems.SelectedIndexChanged += LstItems_SelectedIndexChanged;
            this.Controls.Add(lstItems);

            // Description
            lblDescription = new Label
            {
                Size = new Size(200, 150),
                Location = new Point(180, 50),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblDescription);

            // Refresh inventory
            RefreshInventory();
        }

        public void RefreshInventory()
        {
            lstItems.Items.Clear();
            foreach (var item in player.Inventory)
            {
                lstItems.Items.Add($"{item.Key} x{item.Value}");
            }
        }

        private void LstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex == -1)
            {
                lblDescription.Text = "";
                return;
            }

            string itemName = lstItems.SelectedItem.ToString().Split(' ')[0];

            switch (itemName)
            {
                case "Bomb":
                    lblDescription.Text = "Bomb\n\nDestroys all enemies on screen.";
                    break;
                case "Freeze":
                    lblDescription.Text = "Freeze\n\nFreezes all enemies for 5 seconds.";
                    break;
                case "HealthPotion":
                    lblDescription.Text = "Health Potion\n\nRestores 1 health point.";
                    break;
                default:
                    lblDescription.Text = itemName + "\n\nNo description available.";
                    break;
            }
        }
    }
}
