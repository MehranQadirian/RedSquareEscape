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
        public float Speed { get; set; } = 10f;
        public float Damage { get; set; } = 10f;
        public Color Color { get; set; } = Color.White;
        public int Size { get; set; } = 5;

        public Bullet(PointF position, PointF direction, float damage)
        {
            Position = position;
            Direction = direction;
            Damage = damage;
        }

        public void Update()
        {
            Position = new PointF(
                Position.X + Direction.X * Speed,
                Position.Y + Direction.Y * Speed
            );
        }

        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(Color);
            g.FillEllipse(brush, Position.X - Size, Position.Y - Size, Size * 2, Size * 2);
        }
    }
}
