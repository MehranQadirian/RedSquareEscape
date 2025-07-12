using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Item
    {
        public PointF Position { get; set; }
        public ItemType Type { get; set; }
        public float Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Color Color { get; set; }

        public Item(PointF position, ItemType type, float value)
        {
            Position = position;
            Type = type;
            Value = value;

            switch (type)
            {
                case ItemType.HealthPotion:
                    Name = "Health Potion";
                    Description = "Restores 20 health";
                    Color = Color.Green;
                    Price = 30;
                    break;
                case ItemType.ShieldPotion:
                    Name = "Shield Potion";
                    Description = "Restores 15 shield";
                    Color = Color.Blue;
                    Price = 40;
                    break;
                case ItemType.Bomb:
                    Name = "Bomb";
                    Description = "Deals damage to all enemies";
                    Color = Color.Red;
                    Price = 50;
                    break;
                case ItemType.Freeze:
                    Name = "Freeze";
                    Description = "Freezes enemies for 3 seconds";
                    Color = Color.Cyan;
                    Price = 60;
                    break;
            }
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(Color);

            switch (Type)
            {
                case ItemType.HealthPotion:
                    g.FillRectangle(brush, Position.X - 10, Position.Y - 10, 20, 20);
                    break;
                case ItemType.ShieldPotion:
                    g.FillEllipse(brush, Position.X - 10, Position.Y - 10, 20, 20);
                    break;
                case ItemType.Bomb:
                    g.FillPolygon(brush, new PointF[] {
                    new PointF(Position.X, Position.Y - 10),
                    new PointF(Position.X + 10, Position.Y + 10),
                    new PointF(Position.X - 10, Position.Y + 10)
                });
                    break;
                case ItemType.Freeze:
                    g.FillRectangle(brush, Position.X - 10, Position.Y - 10, 20, 20);
                    g.FillEllipse(Brushes.White, Position.X - 5, Position.Y - 5, 10, 10);
                    break;
            }

            // Draw item name
            Font font = new Font("Arial", 8);
            g.DrawString(Name, font, Brushes.White, Position.X - 15, Position.Y - 25);
        }
    }

    public enum ItemType
    {
        HealthPotion,
        ShieldPotion,
        Bomb,
        Freeze
    }

    

    public class PowerUp
    {
        public string Name { get; protected set; }
        public float Duration { get; set; }
        public bool IsActive => Duration > 0;

        public virtual void Activate(Player player)
        {
            // فعال کردن اثر
        }

        public virtual void Deactivate(Player player)
        {
            // غیرفعال کردن اثر
        }
    }
}
