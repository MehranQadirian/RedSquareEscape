using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    [Serializable]
    public class Player
    {
        // مشخصات پایه
        public string Name { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; }
        public int MaxExperience => Level * 100;

        // وضعیت سلامتی
        public float Health { get; set; } = 100;
        public float MaxHealth { get; set; } = 100;
        public float Shield { get; set; } = 0;
        public float MaxShield { get; set; } = 50;

        // امتیازات
        public int Score { get; set; }
        public int Coins { get; set; }

        // مشخصات فیزیکی
        public PointF Position { get; set; }
        public SizeF Size { get; } = new SizeF(30, 30);
        public float Speed { get; set; } = 250f;
        public Color Color { get; set; } = Color.Lime;

        // سیستم تیراندازی
        public List<Weapon> Weapons { get; } = new List<Weapon>();
        public int CurrentWeaponIndex { get; set; }
        public Weapon CurrentWeapon => Weapons[CurrentWeaponIndex];

        // قدرت‌ها و توانایی‌ها
        public List<PowerUp> ActivePowerUps { get; } = new List<PowerUp>();
        public Dictionary<string, int> Inventory { get; } = new Dictionary<string, int>();

        // ظاهر
        public PlayerAppearance Appearance { get; set; } = PlayerAppearance.Default;

        public Player(string name)
        {
            Name = name;
            InitializeWeapons();
            InitializeInventory();
        }

        private void InitializeWeapons()
        {
            Weapons.Add(new Weapon("پایه", 10, 0.3f, 600f, WeaponType.Straight));
            Weapons.Add(new Weapon("دوگانه", 8, 0.4f, 500f, WeaponType.Double));
            Weapons.Add(new Weapon("پخش‌شونده", 5, 0.5f, 400f, WeaponType.Spread));
        }

        private void InitializeInventory()
        {
            Inventory.Add("بمب", 3);
            Inventory.Add("پوشش محافظ", 2);
            Inventory.Add("الکتروشوک", 1);
            Inventory.Add("تیر تقویتی", 5);
        }

        public void Draw(Graphics g)
        {
            // رسم بازیکن با ظاهر انتخابی
            Appearance.Draw(g, Position, Size, Color);

            // نمایش وضعیت
            DrawStatusBars(g);
        }

        private void DrawStatusBars(Graphics g)
        {
            float barWidth = 60;
            float barHeight = 5;
            float yOffset = -20;

            // نوار سلامت
            float healthPercent = Health / MaxHealth;
            g.FillRectangle(Brushes.DarkRed,
                Position.X - barWidth / 2,
                Position.Y + yOffset,
                barWidth,
                barHeight);
            g.FillRectangle(Brushes.Red,
                Position.X - barWidth / 2,
                Position.Y + yOffset,
                barWidth * healthPercent,
                barHeight);

            // نوار محافظ (اگر وجود دارد)
            if (Shield > 0)
            {
                float shieldPercent = Shield / MaxShield;
                yOffset -= 8;
                g.FillRectangle(Brushes.DarkBlue,
                    Position.X - barWidth / 2,
                    Position.Y + yOffset,
                    barWidth,
                    barHeight);
                g.FillRectangle(Brushes.Cyan,
                    Position.X - barWidth / 2,
                    Position.Y + yOffset,
                    barWidth * shieldPercent,
                    barHeight);
            }
        }

        public void Update(float deltaTime)
        {
            // به‌روزرسانی قدرت‌ها
            UpdatePowerUps(deltaTime);
        }

        private void UpdatePowerUps(float deltaTime)
        {
            for (int i = ActivePowerUps.Count - 1; i >= 0; i--)
            {
                ActivePowerUps[i].Duration -= deltaTime;
                if (ActivePowerUps[i].Duration <= 0)
                {
                    ActivePowerUps[i].Deactivate(this);
                    ActivePowerUps.RemoveAt(i);
                }
            }
        }
    }

    public enum PlayerAppearance
    {
        Default,
        Warrior,
        Ninja,
        Cyborg
    }

    public static class PlayerAppearanceExtensions
    {
        public static void Draw(this PlayerAppearance appearance, Graphics g, PointF position, SizeF size, Color baseColor)
        {
            switch (appearance)
            {
                case PlayerAppearance.Default:
                    using (var brush = new SolidBrush(baseColor))
                    {
                        g.FillRectangle(brush,
                            position.X - size.Width / 2,
                            position.Y - size.Height / 2,
                            size.Width,
                            size.Height);
                    }
                    break;

                case PlayerAppearance.Warrior:
                    // طراحی ظاهر جنگجو
                    break;

                    // ظاهرهای دیگر...
            }
        }
    }
}
