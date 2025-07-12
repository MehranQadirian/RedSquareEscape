using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RedSquareEscape.Classes
{
    public static class SaveManager
    {
        private static string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RedSquareEscapeSaves");

        public static void SaveGame(GameState gameState, string saveName)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string filePath = Path.Combine(savePath, $"{saveName}.save");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, gameState);
            }
        }

        public static GameState LoadGame(string saveName)
        {
            string filePath = Path.Combine(savePath, $"{saveName}.save");

            if (!File.Exists(filePath))
                return null;

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (GameState)formatter.Deserialize(fs);
            }
        }

        public static string[] GetSaveFiles()
        {
            if (!Directory.Exists(savePath))
                return new string[0];

            return Directory.GetFiles(savePath, "*.save");
        }

        public static void DeleteSave(string saveName)
        {
            string filePath = Path.Combine(savePath, $"{saveName}.save");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Serializable]
    public class GameState
    {
        public Player Player { get; set; }
        public int CurrentLevel { get; set; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public DateTime SaveTime { get; set; }
        public string StageName { get; set; }
    }
}
