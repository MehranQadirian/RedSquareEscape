using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class ParticleSystem
    {
        private List<Particle> particles = new List<Particle>();
        private Random random = new Random();

        public void CreateParticles(PointF position, Color color, int count)
        {
            for (int i = 0; i < count; i++)
            {
                particles.Add(new Particle
                {
                    Position = position,
                    Velocity = new PointF(
                        (float)(random.NextDouble() * 6 - 3),
                        (float)(random.NextDouble() * 6 - 3)
                    ),
                    Color = color,
                    Life = 1f,
                    Size = random.Next(3, 8)
                });
            }
        }

        public void Update()
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Position = new PointF(
                    particles[i].Position.X + particles[i].Velocity.X,
                    particles[i].Position.Y + particles[i].Velocity.Y
                );

                particles[i].Life -= 0.02f;

                if (particles[i].Life <= 0)
                    particles.RemoveAt(i);
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var particle in particles)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(
                    (int)(particle.Life * 255), particle.Color)))
                {
                    g.FillEllipse(brush,
                        particle.Position.X - particle.Size / 2,
                        particle.Position.Y - particle.Size / 2,
                        particle.Size, particle.Size);
                }
            }
        }
        public void CreateExplosionEffect()
        {
            // پیاده‌سازی ایجاد جلوه‌های ذره‌ای
        }
    }

    public class Particle
    {
        public PointF Position { get; set; }
        public PointF Velocity { get; set; }
        public Color Color { get; set; }
        public float Life { get; set; }
        public int Size { get; set; }
    }
}
