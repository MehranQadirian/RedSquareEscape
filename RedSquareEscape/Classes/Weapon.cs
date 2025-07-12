using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Weapon
    {
        public string Name { get; set; } = "Pistol";
        public float FireRate { get; set; } = 0.5f;
        public float Damage { get; set; } = 10f;
        public int BulletCount { get; set; } = 1;
        public float SpreadAngle { get; set; } = 0f;
        public Color BulletColor { get; set; } = Color.White;

        private float lastShotTime = 0f;

        public List<Bullet> Shoot(PointF position)
        {
            List<Bullet> bullets = new List<Bullet>();
            PointF direction = GetShootingDirection(position);

            if (BulletCount == 1)
            {
                bullets.Add(new Bullet(position, direction, Damage) { Color = BulletColor });
            }
            else
            {
                float startAngle = -SpreadAngle / 2;
                float angleStep = SpreadAngle / (BulletCount - 1);

                for (int i = 0; i < BulletCount; i++)
                {
                    float angle = startAngle + i * angleStep;
                    PointF bulletDirection = RotateVector(direction, angle);
                    bullets.Add(new Bullet(position, bulletDirection, Damage) { Color = BulletColor });
                }
            }

            lastShotTime = GetCurrentTime();
            return bullets;
        }

        private PointF GetShootingDirection(PointF position)
        {
            // Default direction (right)
            return new PointF(1, 0);
        }

        private PointF RotateVector(PointF vector, float angle)
        {
            float rad = angle * (float)(Math.PI / 180);
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            return new PointF(
                vector.X * cos - vector.Y * sin,
                vector.X * sin + vector.Y * cos
            );
        }

        private float GetCurrentTime()
        {
            return (float)Environment.TickCount / 1000f;
        }

        public bool CanShoot()
        {
            return (GetCurrentTime() - lastShotTime) >= FireRate;
        }
    }
}
