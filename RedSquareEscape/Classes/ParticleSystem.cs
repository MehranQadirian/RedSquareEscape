using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class Particle
    {
        public PointF Position { get; set; }
        public PointF Velocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public float Lifetime { get; set; }
        public float MaxLifetime { get; set; }

        public void Update(float deltaTime)
        {
            Position = new PointF(
                Position.X + Velocity.X * deltaTime,
                Position.Y + Velocity.Y * deltaTime);

            Lifetime += deltaTime;
            Size *= 0.98f;
        }

        public void Draw(Graphics g)
        {
            float alpha = 255 * (1 - Lifetime / MaxLifetime);
            using (var brush = new SolidBrush(Color.FromArgb((int)alpha, Color)))
            {
                g.FillEllipse(brush, Position.X - Size / 2, Position.Y - Size / 2, Size, Size);
            }
        }
    }

    public static class ParticleSystem
    {
        private static List<Particle> particles = new List<Particle>();
        private static Random random = new Random();

        public static void Update(float deltaTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(deltaTime);
                if (particles[i].Lifetime >= particles[i].MaxLifetime)
                {
                    particles.RemoveAt(i);
                }
            }
        }

        public static void Draw(Graphics g)
        {
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }

        public static void CreateExplosion(PointF position, Color color, int count = 20)
        {
            for (int i = 0; i < count; i++)
            {
                particles.Add(new Particle()
                {
                    Position = position,
                    Velocity = new PointF(
                        (float)(random.NextDouble() * 2 - 1) * 100,
                        (float)(random.NextDouble() * 2 - 1) * 100),
                    Color = color,
                    Size = (float)(random.NextDouble() * 5 + 2),
                    Lifetime = 0,
                    MaxLifetime = (float)(random.NextDouble() * 0.5 + 0.3)
                });
            }
        }

        public static void CreateTrail(PointF position, Color color)
        {
            particles.Add(new Particle()
            {
                Position = position,
                Velocity = new PointF(
                    (float)(random.NextDouble() * 2 - 1) * 10,
                    (float)(random.NextDouble() * 2 - 1) * 10),
                Color = color,
                Size = (float)(random.NextDouble() * 3 + 1),
                Lifetime = 0,
                MaxLifetime = (float)(random.NextDouble() * 0.3 + 0.2)
            });
        }
    }
}
