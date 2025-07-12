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
        public float Acceleration { get; set; } = 0.15f;
        public float Deceleration { get; set; } = 0.92f;
        public float SpecialAttackCooldown { get; set; }
        private GameManager gameManager;
        // وضعیت حرکت
        public PointF Velocity { get; set; } = PointF.Empty;
        public Player(GameManager gm)
        {
            this.gameManager = gm;
            // سایر مقداردهی‌ها...
        }
        public void UpdateMovement(InputState input, float deltaTime = 1f)
        {
            // محاسبه جهت هدف
            PointF targetDirection = PointF.Empty;

            if (input.MoveUp) targetDirection.Y -= 1;
            if (input.MoveDown) targetDirection.Y += 1;
            if (input.MoveLeft) targetDirection.X -= 1;
            if (input.MoveRight) targetDirection.X += 1;

            // نرمالایز کردن جهت در صورت حرکت مورب
            if (targetDirection.X != 0 && targetDirection.Y != 0)
            {
                float length = (float)Math.Sqrt(targetDirection.X * targetDirection.X + targetDirection.Y * targetDirection.Y);
                targetDirection.X /= length;
                targetDirection.Y /= length;
            }

            // محاسبه سرعت هدف
            PointF targetVelocity = new PointF(
                targetDirection.X * MoveSpeed,
                targetDirection.Y * MoveSpeed
            );

            // اعمال شتاب/ترمز
            Velocity = new PointF(
                Lerp(Velocity.X, targetVelocity.X, input.IsMoving ? Acceleration : Deceleration),
                Lerp(Velocity.Y, targetVelocity.Y, input.IsMoving ? Acceleration : Deceleration)
            );

            // اعمال حرکت با در نظر گرفتن زمان فریم
            Position = new PointF(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime
            );

            // محاسبه زاویه چرخش
            if (Velocity.X != 0 || Velocity.Y != 0)
            {
                RotationAngle = (float)(Math.Atan2(Velocity.Y, Velocity.X) * 180 / Math.PI);
            }

            // محدودیت مرزهای صفحه
            Position = new PointF(
                Math.Max(30, Math.Min(770, Position.X)),
                Math.Max(30, Math.Min(570, Position.Y))
            );
        }

        private float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
        public void UseSpecialAttack()
        {
            if (SpecialAttackCooldown <= 0 && gameManager != null)
            {
                gameManager.DefeatEnemies(30);
                SpecialAttackCooldown = 5f;
            }
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

        public void UseItem(ItemType itemType)
        {
            var item = Inventory.Items.FirstOrDefault(i => i.Type == itemType);
            if (item != null)
            {
                switch (item.Type)
                {
                    case ItemType.HealthPotion:
                        Health = Math.Min(Health + item.Value, MaxHealth);
                        break;
                        // سایر موارد...
                }
                Inventory.RemoveItem(item.Type);
            }
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