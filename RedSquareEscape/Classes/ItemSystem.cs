using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public abstract class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Price { get; protected set; }
        public int MaxStack { get; protected set; } = 99;
        public Image Icon { get; protected set; }

        public abstract void Use(Player player);
    }

    public class HealthPack : Item
    {
        public HealthPack()
        {
            Name = "کیف بهداشتی";
            Description = "سلامت شما را 25 واحد افزایش می‌دهد";
            Price = 30;
        }

        public override void Use(Player player)
        {
            player.Health = Math.Min(player.MaxHealth, player.Health + 25);
        }
    }

    public class ShieldBooster : Item
    {
        public ShieldBooster()
        {
            Name = "تقویت‌کننده محافظ";
            Description = "50 واحد محافظ موقت به شما می‌دهد";
            Price = 50;
        }

        public override void Use(Player player)
        {
            player.Shield = Math.Min(player.MaxShield, player.Shield + 50);
        }
    }

    public class PowerUp
    {
        public string Name { get; protected set; }
        public float Duration { get; set; }
        public bool IsActive => Duration > 0;

        public virtual void Activate(Player player)
        {
            // فعال کردن اثر
        }

        public virtual void Deactivate(Player player)
        {
            // غیرفعال کردن اثر
        }
    }

    public class DoubleDamage : PowerUp
    {
        public DoubleDamage()
        {
            Name = "خسارت دوبرابر";
            Duration = 15f;
        }

        public override void Activate(Player player)
        {
            foreach (var weapon in player.Weapons)
            {
                weapon.Damage *= 2;
            }
        }

        public override void Deactivate(Player player)
        {
            foreach (var weapon in player.Weapons)
            {
                weapon.Damage /= 2;
            }
        }
    }
}
