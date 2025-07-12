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
        public string Name { get; }
        public int Damage { get; set; }
        public float FireRate { get; }
        public float BulletSpeed { get; }
        public WeaponType Type { get; }
        public float LastShotTime { get; set; }
        public int UpgradeLevel { get; private set; } = 1;

        public Weapon(string name, int damage, float fireRate, float bulletSpeed, WeaponType type)
        {
            Name = name;
            Damage = damage;
            FireRate = fireRate;
            BulletSpeed = bulletSpeed;
            Type = type;
        }

        public List<Bullet> Shoot(PointF position, List<Enemy> enemies)
        {
            var bullets = new List<Bullet>();
            Enemy nearestEnemy = FindNearestEnemy(position, enemies);

            switch (Type)
            {
                case WeaponType.Straight:
                    bullets.Add(CreateBullet(position, nearestEnemy));
                    break;

                case WeaponType.Double:
                    bullets.Add(CreateBullet(position, nearestEnemy, -0.1f));
                    bullets.Add(CreateBullet(position, nearestEnemy, 0.1f));
                    break;

                case WeaponType.Spread:
                    for (float angle = -0.3f; angle <= 0.3f; angle += 0.15f)
                    {
                        bullets.Add(CreateBullet(position, nearestEnemy, angle));
                    }
                    break;

                case WeaponType.Homing:
                    var homingBullet = CreateBullet(position, nearestEnemy);
                    homingBullet.IsHoming = true;
                    homingBullet.HomingStrength = 0.1f;
                    bullets.Add(homingBullet);
                    break;
            }

            LastShotTime = GameTime.CurrentTime;
            return bullets;
        }

        private Bullet CreateBullet(PointF position, Enemy target, float angleOffset = 0)
        {
            PointF direction = target != null ?
                new PointF(
                    target.Position.X - position.X,
                    target.Position.Y - position.Y) :
                new PointF(0, -1);

            // اعمال انحراف اگر وجود دارد
            if (angleOffset != 0)
            {
                direction = RotateVector(direction, angleOffset);
            }

            // نرمالایز کردن جهت
            float length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            if (length > 0)
            {
                direction.X /= length;
                direction.Y /= length;
            }

            return new Bullet(position, direction, BulletSpeed, Damage)
            {
                Owner = BulletOwner.Player,
                WeaponType = this.Type
            };
        }

        public void Upgrade()
        {
            UpgradeLevel++;
            Damage = (int)(Damage * 1.3f);
            FireRate *= 0.9f;
        }
    }

    public enum WeaponType
    {
        Straight,
        Double,
        Spread,
        Homing,
        Laser
    }
}
