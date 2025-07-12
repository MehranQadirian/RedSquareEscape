using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Bullet
    {
        public PointF Position { get; set; }
        public PointF Direction { get; set; }
        public float Speed { get; }
        public int Damage { get; }
        public BulletOwner Owner { get; set; }
        public WeaponType WeaponType { get; set; }
        public bool IsHoming { get; set; }
        public float HomingStrength { get; set; }
        public float Lifetime { get; set; }
        public float MaxLifetime { get; } = 3f;

        public Bullet(PointF position, PointF direction, float speed, int damage)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Damage = damage;
        }

        public void Update(float deltaTime, List<Enemy> enemies)
        {
            // به‌روزرسانی هدایت شونده‌ها
            if (IsHoming && Owner == BulletOwner.Player)
            {
                UpdateHoming(deltaTime, enemies);
            }

            // به‌روزرسانی موقعیت
            Position = new PointF(
                Position.X + Direction.X * Speed * deltaTime,
                Position.Y + Direction.Y * Speed * deltaTime);

            Lifetime += deltaTime;
        }

        private void UpdateHoming(float deltaTime, List<Enemy> enemies)
        {
            Enemy nearest = FindNearestEnemy(Position, enemies);
            if (nearest != null)
            {
                PointF targetDir = new PointF(
                    nearest.Position.X - Position.X,
                    nearest.Position.Y - Position.Y);

                // نرمالایز کردن جهت
                float length = (float)Math.Sqrt(targetDir.X * targetDir.X + targetDir.Y * targetDir.Y);
                if (length > 0)
                {
                    targetDir.X /= length;
                    targetDir.Y /= length;
                }

                // ترکیب جهت فعلی و جهت به سمت هدف
                Direction = new PointF(
                    Direction.X * (1f - HomingStrength) + targetDir.X * HomingStrength,
                    Direction.Y * (1f - HomingStrength) + targetDir.Y * HomingStrength);

                // نرمالایز کردن جهت نهایی
                length = (float)Math.Sqrt(Direction.X * Direction.X + Direction.Y * Direction.Y);
                if (length > 0)
                {
                    Direction.X /= length;
                    Direction.Y /= length;
                }
            }
        }

        public void Draw(Graphics g)
        {
            Color color = Owner == BulletOwner.Player ? Color.Cyan : Color.Red;

            switch (WeaponType)
            {
                case WeaponType.Laser:
                    DrawLaser(g, color);
                    break;

                default:
                    DrawStandardBullet(g, color);
                    break;
            }
        }

        private void DrawStandardBullet(Graphics g, Color color)
        {
            using (var brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, Position.X - 3, Position.Y - 3, 6, 6);
            }
        }

        private void DrawLaser(Graphics g, Color color)
        {
            using (var pen = new Pen(color, 3))
            {
                g.DrawLine(pen,
                    Position.X, Position.Y,
                    Position.X - Direction.X * 20,
                    Position.Y - Direction.Y * 20);
            }
        }
    }

    public enum BulletOwner
    {
        Player,
        Enemy
    }
}
