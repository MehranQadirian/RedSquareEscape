using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RedSquareEscape.Classes
{
    public static class SaveManager
    {
        public static void SaveGame(Player player, string filePath)
        {
            var saveData = new SaveData()
            {
                PlayerName = player.Name,
                Health = player.Health,
                MaxHealth = player.MaxHealth,
                Score = player.Score,
                Coins = player.Coins,
                Level = player.Level,
                Experience = player.Experience,
                Inventory = player.Inventory
            };

            using (var stream = File.Create(filePath))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, saveData);
            }
        }

        public static Player LoadGame(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var formatter = new BinaryFormatter();
                var saveData = (SaveData)formatter.Deserialize(stream);

                var player = new Player(saveData.PlayerName)
                {
                    Health = saveData.Health,
                    MaxHealth = saveData.MaxHealth,
                    Score = saveData.Score,
                    Coins = saveData.Coins,
                    Level = saveData.Level,
                    Experience = saveData.Experience
                };

                foreach (var item in saveData.Inventory)
                {
                    player.Inventory[item.Key] = item.Value;
                }

                return player;
            }
        }
    }

    [Serializable]
    public class SaveData
    {
        public string PlayerName { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public Dictionary<string, int> Inventory { get; set; }
    }
}
