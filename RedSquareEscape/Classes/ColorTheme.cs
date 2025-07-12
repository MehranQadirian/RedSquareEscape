using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public class ColorTheme
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TextColor { get; set; }
        public Color PlayerColor { get; set; }
        public Color EnemyColor { get; set; }
        public Color BulletColor { get; set; }
        public Color UiColor { get; set; }

        public static ColorTheme GreenBlack => new ColorTheme
        {
            Name = "Green/Black",
            BackgroundColor = Color.FromArgb(30, 30, 30),
            TextColor = Color.FromArgb(134, 253, 233),
            PlayerColor = Color.Lime,
            EnemyColor = Color.Red,
            BulletColor = Color.Cyan,
            UiColor = Color.FromArgb(70, 70, 70)
        };

        public static ColorTheme YellowNavy => new ColorTheme
        {
            Name = "Yellow/Navy",
            BackgroundColor = Color.Navy,
            TextColor = Color.Yellow,
            PlayerColor = Color.Yellow,
            EnemyColor = Color.OrangeRed,
            BulletColor = Color.White,
            UiColor = Color.FromArgb(90, 90, 120)
        };

        public static ColorTheme RedBlack = new ColorTheme
        {
            BackgroundColor = Color.Black,
            TextColor = Color.Red,
            PlayerColor = Color.Red,
            EnemyColor = Color.White
        };

        public static ColorTheme PinkWhite = new ColorTheme
        {
            BackgroundColor = Color.White,
            TextColor = Color.FromArgb(255, 77, 249),
            PlayerColor = Color.FromArgb(255, 77, 249),
            EnemyColor = Color.Black
        };
    }
}
