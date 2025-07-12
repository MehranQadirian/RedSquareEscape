using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class UIManager
    {
        private FormGame game;
        private List<UIElement> elements = new List<UIElement>();
        private object player;

        public UIManager(FormGame game)
        {
            this.game = game;
        }

        public void AddButton(UIButton button)
        {
            elements.Add(button);
        }

        public void Update(float gameTime, Player player)
        {
            foreach (var element in elements)
            {
                element.Update(gameTime);
            }
        }

        public void Draw(Graphics g)
        {
            // رسم HUD پایه
            DrawHUD(g);

            // رسم عناصر UI
            foreach (var element in elements)
            {
                element.Draw(g);
            }
        }

        private void DrawHUD(Graphics g)
        {
            using (var font = new Font("Arial", 14))
            {
                // سلامت
                float healthPercent = player.Health / player.MaxHealth;
                g.FillRectangle(Brushes.DarkRed, 20, 20, 200, 20);
                g.FillRectangle(Brushes.Red, 20, 20, 200 * healthPercent, 20);
                g.DrawString($"سلامت: {player.Health}/{player.MaxHealth}", font, Brushes.White, 25, 20);

                // محافظ
                if (player.Shield > 0)
                {
                    float shieldPercent = player.Shield / player.MaxShield;
                    g.FillRectangle(Brushes.DarkBlue, 20, 50, 200, 20);
                    g.FillRectangle(Brushes.Cyan, 20, 50, 200 * shieldPercent, 20);
                    g.DrawString($"محافظ: {player.Shield}/{player.MaxShield}", font, Brushes.White, 25, 50);
                }

                // امتیاز و سکه
                g.DrawString($"امتیاز: {player.Score}", font, Brushes.White, 20, 80);
                g.DrawString($"سکه: {player.Coins}", font, Brushes.Gold, 20, 110);

                // سطح
                g.DrawString($"سطح: {player.Level}", font, Brushes.White, 20, 140);
            }
        }
    }

    public abstract class UIElement
    {
        public PointF Position { get; set; }

        public abstract void Update(float gameTime);
        public abstract void Draw(Graphics g);
    }

    public class UIButton : UIElement
    {
        private string text;
        private Action onClick;

        public UIButton(string text, PointF position, Action onClick)
        {
            this.text = text;
            this.Position = position;
            this.onClick = onClick;
        }

        public override void Update(float gameTime)
        {
            // منطق به‌روزرسانی
        }

        public override void Draw(Graphics g)
        {
            using (var brush = new SolidBrush(Color.FromArgb(70, 70, 70)))
            {
                g.FillRectangle(brush, Position.X - 20, Position.Y - 20, 40, 40);
            }

            using (var font = new Font("Arial", 12))
            {
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.White,
                    Position.X - size.Width / 2,
                    Position.Y - size.Height / 2);
            }
        }

        public void Click()
        {
            onClick?.Invoke();
        }
    }
}
