using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public static class ResourceManager
    {
        private static Dictionary<string, Image> textures = new Dictionary<string, Image>();
        private static Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();

        public static void LoadTextures()
        {
            // بارگذاری تصاویر
            textures.Add("player_default", LoadImage("player1.png"));
            textures.Add("player_warrior", LoadImage("player2.png"));
            textures.Add("enemy_grunt", LoadImage("enemy1.png"));
            // ...
        }

        public static void LoadSounds()
        {
            // بارگذاری صداها
            sounds.Add("shoot", LoadSound("shoot.wav"));
            sounds.Add("explosion", LoadSound("explosion.wav"));
            // ...
        }

        public static Image GetTexture(string name)
        {
            return textures.ContainsKey(name) ? textures[name] : null;
        }

        public static void PlaySound(string name)
        {
            if (sounds.ContainsKey(name))
            {
                sounds[name].Play();
            }
        }

        private static Image LoadImage(string path)
        {
            // کد بارگذاری تصویر
            return Image.FromFile("Resources/" + path);
        }

        private static Sound LoadSound(string path)
        {
            // کد بارگذاری صدا
            return new Sound("Resources/" + path);
        }
    }

    public class Sound
    {
        public Sound(string path)
        {
            // پیاده‌سازی بارگذاری صدا
        }

        public void Play()
        {
            // پخش صدا
        }
    }
}
