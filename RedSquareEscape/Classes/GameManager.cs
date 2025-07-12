using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSquareEscape.Classes
{
    public static class GameManager
    {
        public static int CurrentLevel { get; private set; } = 1;
        public static int EnemiesKilled { get; private set; }
        public static int TotalScore { get; private set; }
        public static int TotalCoins { get; private set; }
        public static GameState State { get; private set; } = GameState.MainMenu;

        private static Dictionary<int, LevelSettings> levelSettings = new Dictionary<int, LevelSettings>()
    {
        {1, new LevelSettings(5, 1.0f, EnemyType.Grunt)},
        {2, new LevelSettings(8, 0.9f, EnemyType.Grunt)},
        {3, new LevelSettings(10, 0.8f, EnemyType.Grunt)},
        {4, new LevelSettings(7, 1.0f, EnemyType.Tank)},
        {5, new LevelSettings(12, 0.7f, EnemyType.Grunt)},
        {6, new LevelSettings(15, 0.6f, EnemyType.Grunt)},
        {7, new LevelSettings(10, 0.8f, EnemyType.Tank)},
        {8, new LevelSettings(20, 0.5f, EnemyType.Grunt)},
        {9, new LevelSettings(1, 1.0f, EnemyType.Boss)}
    };

        public static void StartNewGame()
        {
            CurrentLevel = 1;
            EnemiesKilled = 0;
            TotalScore = 0;
            TotalCoins = 0;
            State = GameState.Playing;
        }

        public static LevelSettings GetCurrentLevelSettings()
        {
            return levelSettings.ContainsKey(CurrentLevel) ?
                levelSettings[CurrentLevel] :
                new LevelSettings(15, 0.5f, EnemyType.Grunt);
        }

        public static void LevelCompleted()
        {
            CurrentLevel++;
            State = GameState.LevelTransition;
        }

        public static void GameOver()
        {
            State = GameState.GameOver;
        }

        public static void AddScore(int score)
        {
            TotalScore += score;
            EnemiesKilled++;
        }

        public static void AddCoins(int coins)
        {
            TotalCoins += coins;
        }
    }

    public class LevelSettings
    {
        public int EnemyCount { get; }
        public float DifficultyModifier { get; }
        public EnemyType MainEnemyType { get; }

        public LevelSettings(int enemyCount, float difficultyModifier, EnemyType mainEnemyType)
        {
            EnemyCount = enemyCount;
            DifficultyModifier = difficultyModifier;
            MainEnemyType = mainEnemyType;
        }
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        LevelTransition,
        GameOver,
        Shop
    }
}
