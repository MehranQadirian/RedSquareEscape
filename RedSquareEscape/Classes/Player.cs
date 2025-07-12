using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    [Serializable]
    public enum PlayerShape
    {
        Square,
        Triangle,
        Circle
    }

    public class Player
    {
        private float acceleration = 0.5f;
        private float deceleration = 0.9f;
        private float maxSpeed = 8f;
        public PointF velocity = PointF.Empty;
        public string Name { get; set; }
        public int Age { get; set; }
        public PointF Position { get; set; }
        public float Health { get; set; } = 100;
        public float MaxHealth { get; set; } = 100;
        public float Shield { get; set; } = 0;
        public float MaxShield { get; set; } = 50;
        public int Score { get; set; } = 0;
        public int Coins { get; set; } = 0;
        public int Level { get; set; } = 1;
        public float Speed { get; set; } = 5f;
        public PlayerShape Shape { get; set; } = PlayerShape.Square;
        public Color Color { get; set; } = Color.Red;
        public Inventory Inventory { get; set; } = new Inventory();
        public Weapon CurrentWeapon { get; set; } = new Weapon();

        public void Move(Direction direction)
        {
            // اعمال شتاب
            if (direction.HasFlag(Direction.Up))
                velocity.Y -= acceleration;

            if (direction.HasFlag(Direction.Down))
                velocity.Y += acceleration;

            if (direction.HasFlag(Direction.Left))
                velocity.X -= acceleration;

            if (direction.HasFlag(Direction.Right))
                velocity.X += acceleration;

            // اگر هیچ جهتی فعال نبود (None)، سرعت را کاهش دهیم
            if (direction == Direction.None)
            {
                velocity.X *= deceleration;
                velocity.Y *= deceleration;
            }

            // محدودیت سرعت
            float speed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            if (speed > maxSpeed)
            {
                velocity.X = velocity.X / speed * maxSpeed;
                velocity.Y = velocity.Y / speed * maxSpeed;
            }

            // اعمال حرکت
            Position = new PointF(
                Position.X + velocity.X,
                Position.Y + velocity.Y
            );

            // محدودیت مرزهای صفحه
            Position = new PointF(
            Clamp(Position.X, 30, 770),
            Clamp(Position.Y, 30, 570)
        );
        }
        private float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
        public void TakeDamage(float damage)
        {
            if (Shield > 0)
            {
                Shield -= damage;
                if (Shield < 0)
                {
                    Health += Shield; // Shield is negative here
                    Shield = 0;
                }
            }
            else
            {
                Health -= damage;
            }

            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            // Handle player death
            Health = 0;
        }
        public void UpgradeWeapon()
        {
            if (CurrentWeapon.BulletCount < 10)
            {
                CurrentWeapon.BulletCount++;
            }
            else
            {
                CurrentWeapon.Damage += 5;
            }
        }
        public void AddCoins(int amount)
        {
            Coins += amount;
        }
        public void UseItem(Item item)
        {
            switch (item.Type)
            {
                case ItemType.HealthPotion:
                    Health = Math.Min(Health + item.Value, MaxHealth);
                    break;
                case ItemType.ShieldPotion:
                    Shield = Math.Min(Shield + item.Value, MaxShield);
                    break;
                case ItemType.Bomb:
                    // Implement bomb logic
                    break;
                case ItemType.Freeze:
                    // Implement freeze logic
                    break;
            }

            Inventory.RemoveItem(item);
        }

        public void Draw(Graphics g)
        {
            // محاسبه زاویه حرکت
            float angle = 0;
            if (velocity.X != 0 || velocity.Y != 0)
            {
                angle = (float)(Math.Atan2(velocity.Y, velocity.X) * 180 / Math.PI);
            }

            // اعمال چرخش
            g.TranslateTransform(Position.X, Position.Y);
            g.RotateTransform(angle);

            Brush brush = new SolidBrush(Color);
            switch (Shape)
            {
                case PlayerShape.Square:
                    g.FillRectangle(brush, -15, -15, 30, 30);
                    break;
                case PlayerShape.Triangle:
                    PointF[] points = { new PointF(0, -15), new PointF(-10, 10), new PointF(10, 10) };
                    g.FillPolygon(brush, points);
                    break;
                case PlayerShape.Circle:
                    g.FillEllipse(brush, -15, -15, 30, 30);
                    break;
            }

            g.ResetTransform();

            // نمایش اثر حرکت
            if (velocity.X != 0 || velocity.Y != 0)
            {
                float trailAlpha = 100;
                for (int i = 1; i <= 3; i++)
                {
                    PointF trailPos = new PointF(
                        Position.X - velocity.X * i * 2,
                        Position.Y - velocity.Y * i * 2
                    );

                    // روش سازگار با نسخه‌های قدیمی:
                    Brush trailBrush = null;
                    try
                    {
                        trailBrush = new SolidBrush(Color.FromArgb((int)(trailAlpha / i), Color));
                        g.FillEllipse(trailBrush, trailPos.X - 10, trailPos.Y - 10, 20, 20);
                    }
                    finally
                    {
                        if (trailBrush != null)
                            trailBrush.Dispose();
                    }
                }
            }
        }
    }
    


    [Flags]
    public enum Direction
    {
        None = 0,       // اضافه کردن مقدار None
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }
}
