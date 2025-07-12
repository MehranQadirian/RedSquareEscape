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
        // متغیرهای حرکت
        public PointF velocity = PointF.Empty;
        public float RotationAngle { get; private set; }

        // تنظیمات حرکت
        public float MoveSpeed { get; set; } = 5f;
        public float Acceleration { get; set; } = 0.2f;
        public float MaxSpeed { get; set; } = 8f;
        public float Inertia { get; set; } = 0.95f;

        // مشخصات بازیکن
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
        public PlayerShape Shape { get; set; } = PlayerShape.Square;
        public Color Color { get; set; } = Color.Red;
        public Inventory Inventory { get; set; } = new Inventory();
        public Weapon CurrentWeapon { get; set; } = new Weapon();

        public void UpdateMovement(InputState input)
        {
            // اعمال شتاب بر اساس ورودی
            float targetVelocityX = 0;
            float targetVelocityY = 0;

            if (input.MoveUp) targetVelocityY -= MoveSpeed;
            if (input.MoveDown) targetVelocityY += MoveSpeed;
            if (input.MoveLeft) targetVelocityX -= MoveSpeed;
            if (input.MoveRight) targetVelocityX += MoveSpeed;

            // نرمالایز کردن جهت در صورت حرکت مورب
            if (targetVelocityX != 0 && targetVelocityY != 0)
            {
                float length = (float)Math.Sqrt(targetVelocityX * targetVelocityX + targetVelocityY * targetVelocityY);
                targetVelocityX = targetVelocityX / length * MoveSpeed;
                targetVelocityY = targetVelocityY / length * MoveSpeed;
            }

            // اعمال شتاب تدریجی
            velocity = new PointF(
                Lerp(velocity.X, targetVelocityX, Acceleration),
                Lerp(velocity.Y, targetVelocityY, Acceleration)
            );

            // محدودیت سرعت
            float currentSpeed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            if (currentSpeed > MaxSpeed)
            {
                velocity = new PointF(
                    velocity.X / currentSpeed * MaxSpeed,
                    velocity.Y / currentSpeed * MaxSpeed
                );
            }

            // اعمال حرکت
            Position = new PointF(
                Position.X + velocity.X,
                Position.Y + velocity.Y
            );

            // محاسبه زاویه چرخش
            if (velocity.X != 0 || velocity.Y != 0)
            {
                RotationAngle = (float)(Math.Atan2(velocity.Y, velocity.X) * 180 / Math.PI);
            }

            // اعمال اینرسی هنگام رها کردن کلیدها
            if (!input.MoveUp && !input.MoveDown && !input.MoveLeft && !input.MoveRight)
            {
                velocity = new PointF(velocity.X * Inertia, velocity.Y * Inertia);
            }

            // محدودیت مرزهای صفحه
            Position = new PointF(
                Clamp(Position.X, 30, 770),
                Clamp(Position.Y, 30, 570)
            );
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        // ... (بقیه متدها مانند TakeDamage, Die, UpgradeWeapon, etc.)
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

                    using (Brush trailBrush = new SolidBrush(Color.FromArgb((int)(trailAlpha / i), Color)))
                    {
                        g.FillEllipse(trailBrush, trailPos.X - 10, trailPos.Y - 10, 20, 20);
                    }
                }
            }
        }
    }

    [Flags]
    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8
    }
}