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
        private PointF velocity = PointF.Empty;
        private float randomMovementTimer = 0;
        private PointF randomDirection = PointF.Empty;
        public PointF Position { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public float Damage { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public bool IsBoss { get; set; }
        public EnemyType Type { get; set; }

        public Enemy(PointF position, float health, float damage, float speed, Color color)
        {
            Position = position;
            Health = MaxHealth = health;
            Damage = damage;
            Speed = speed;
            Color = color;
            Name = $"E{new Random().Next(1000)}";
            Type = EnemyType.Normal;
        }
        public void ApplyEffect(EffectType effect)
        {
            switch (effect)
            {
                case EffectType.Freeze:
                    Speed *= 0.3f; // Reduce speed
                    break;
                case EffectType.Poison:
                    // Apply poison over time
                    break;
            }
        }
        public void Update(PointF playerPosition)
        {
            // Simple AI: Move toward player
            randomMovementTimer -= 0.1f;

            if (randomMovementTimer <= 0)
            {
                randomDirection = new PointF(
                    (float)(new Random().NextDouble() * 2 - 1),
                    (float)(new Random().NextDouble() * 2 - 1)
                );
                randomMovementTimer = 2f;
            }

            PointF directionToPlayer = new PointF(
                playerPosition.X - Position.X,
                playerPosition.Y - Position.Y
            );

            float distance = (float)Math.Sqrt(directionToPlayer.X * directionToPlayer.X +
                                            directionToPlayer.Y * directionToPlayer.Y);

            if (distance > 0)
            {
                // ترکیب حرکت به سمت بازیکن و حرکت تصادفی
                velocity.X += (directionToPlayer.X / distance * 0.7f + randomDirection.X * 0.3f) * 0.2f;
                velocity.Y += (directionToPlayer.Y / distance * 0.7f + randomDirection.Y * 0.3f) * 0.2f;
            }

            // محدودیت سرعت
            float speed = (float)Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
            if (speed > Speed)
            {
                velocity.X = velocity.X / speed * Speed;
                velocity.Y = velocity.Y / speed * Speed;
            }

            // اعمال حرکت
            Position = new PointF(
                Position.X + velocity.X,
                Position.Y + velocity.Y
            );
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(Color);

            // Draw enemy shape
            g.FillRectangle(brush, Position.X - 15, Position.Y - 15, 30, 30);

            // Draw health bar
            float healthPercent = Health / MaxHealth;
            g.FillRectangle(Brushes.Red, Position.X - 15, Position.Y - 25, 30, 5);
            g.FillRectangle(Brushes.Green, Position.X - 15, Position.Y - 25, 30 * healthPercent, 5);

            // Draw enemy name
            Font font = new Font("Arial", 8);
            g.DrawString(Name, font, Brushes.Red, Position.X - 15, Position.Y - 40);

            // Draw boss indicator
            if (IsBoss)
            {
                g.DrawString("BOSS", new Font("Arial", 10, FontStyle.Bold), Brushes.Purple,
                    Position.X - 15, Position.Y - 55);
            }
        }
    }

    public enum EnemyType
    {
        Normal,
        Fast,
        Tank,
        Boss
    }
    public enum EffectType
    {
        Freeze,
        Poison
    }
}
