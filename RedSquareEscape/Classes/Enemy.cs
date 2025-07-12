using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Enemy
    {
        public EnemyType Type { get; }
        public string Name { get; }
        public PointF Position { get; set; }
        public SizeF Size { get; }
        public float Health { get; set; }
        public float MaxHealth { get; }
        public float Damage { get; }
        public float Speed { get; set; }
        public int ScoreValue { get; }
        public int CoinValue { get; }
        public Color BaseColor { get; }
        public EnemyBehavior Behavior { get; }
        public bool IsDead => Health <= 0;

        // سیستم حمله
        public float AttackCooldown { get; set; }
        public float AttackRate { get; }
        public List<EnemyAbility> Abilities { get; }

        public Enemy(EnemyType type, PointF position)
        {
            Type = type;
            Position = position;

            // تنظیمات بر اساس نوع دشمن
            switch (type)
            {
                case EnemyType.Grunt:
                    Name = "سرباز";
                    Size = new SizeF(25, 25);
                    Health = MaxHealth = 50;
                    Damage = 10;
                    Speed = 120f;
                    ScoreValue = 20;
                    CoinValue = 5;
                    BaseColor = Color.Red;
                    Behavior = EnemyBehavior.Chase;
                    AttackRate = 1.5f;
                    Abilities = new List<EnemyAbility>();
                    break;

                case EnemyType.Tank:
                    Name = "تانک";
                    Size = new SizeF(40, 40);
                    Health = MaxHealth = 150;
                    Damage = 20;
                    Speed = 70f;
                    ScoreValue = 50;
                    CoinValue = 15;
                    BaseColor = Color.DarkRed;
                    Behavior = EnemyBehavior.Charge;
                    AttackRate = 2f;
                    Abilities = new List<EnemyAbility> { EnemyAbility.Shockwave };
                    break;

                    // انواع دیگر دشمنان...
            }
        }

        public void Update(Player player, float deltaTime)
        {
            // به‌روزرسانی رفتار
            UpdateBehavior(player, deltaTime);

            // به‌روزرسانی زمان توانایی‌ها
            AttackCooldown -= deltaTime;
        }

        private void UpdateBehavior(Player player, float deltaTime)
        {
            switch (Behavior)
            {
                case EnemyBehavior.Chase:
                    ChasePlayer(player, deltaTime);
                    break;

                case EnemyBehavior.Charge:
                    ChargePlayer(player, deltaTime);
                    break;

                    // رفتارهای دیگر...
            }
        }

        private void ChasePlayer(Player player, float deltaTime)
        {
            // محاسبه جهت به سمت بازیکن
            PointF direction = new PointF(
                player.Position.X - Position.X,
                player.Position.Y - Position.Y);

            // نرمالایز کردن جهت
            float length = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            if (length > 0)
            {
                direction.X /= length;
                direction.Y /= length;
            }

            // حرکت
            Position = new PointF(
                Position.X + direction.X * Speed * deltaTime,
                Position.Y + direction.Y * Speed * deltaTime);
        }

        public void Draw(Graphics g)
        {
            // رسم بدنه اصلی
            using (var brush = new SolidBrush(BaseColor))
            {
                g.FillRectangle(brush,
                    Position.X - Size.Width / 2,
                    Position.Y - Size.Height / 2,
                    Size.Width,
                    Size.Height);
            }

            // نمایش وضعیت
            DrawStatus(g);
        }

        private void DrawStatus(Graphics g)
        {
            // نوار سلامت
            float healthPercent = Health / MaxHealth;
            float barWidth = 40;
            float barHeight = 4;

            g.FillRectangle(Brushes.DarkRed,
                Position.X - barWidth / 2,
                Position.Y - Size.Height / 2 - 10,
                barWidth,
                barHeight);

            g.FillRectangle(Brushes.Red,
                Position.X - barWidth / 2,
                Position.Y - Size.Height / 2 - 10,
                barWidth * healthPercent,
                barHeight);

            // نام دشمن
            using (var font = new Font("Arial", 8))
            {
                var size = g.MeasureString(Name, font);
                g.DrawString(Name, font, Brushes.White,
                    Position.X - size.Width / 2,
                    Position.Y - Size.Height / 2 - 25);
            }
        }
    }

    public enum EnemyType
    {
        Grunt,
        Tank,
        Sniper,
        Bomber,
        Boss
    }

    public enum EnemyBehavior
    {
        Chase,
        Charge,
        Patrol,
        Ranged
    }

    public enum EnemyAbility
    {
        Shockwave,
        FireTrail,
        Summon,
        Teleport
    }
}
