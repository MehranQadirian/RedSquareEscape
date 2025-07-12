using RedSquareEscape.Classes;
using System;
using System.Windows.Forms;

namespace RedSquareEscape
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // نمایش صفحه لودینگ
            var loadingForm = new FormLoading();
            loadingForm.Show();

            // بارگذاری منابع
            LoadResources();

            // پس از اتمام بارگذاری
            loadingForm.Close();

            // اجرای حلقه اصلی بازی
            while (true)
            {
                using (var mainMenu = new FormMenu())
                {
                    if (mainMenu.ShowDialog() == DialogResult.OK)
                    {
                        // شروع بازی جدید
                        GameManager.StartNewGame();
                        RunGame();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static void LoadResources()
        {
            // بارگذاری تصاویر، صداها و سایر منابع
            ResourceManager.LoadTextures();
            ResourceManager.LoadSounds();
        }

        private static void RunGame()
        {
            while (GameManager.State != GameState.GameOver)
            {
                using (var gameForm = new FormGame(new Player("بازیکن")))
                {
                    gameForm.ShowDialog();

                    if (GameManager.State == GameState.GameOver)
                    {
                        using (var gameOverForm = new FormGameOver(
                            GameManager.TotalScore,
                            GameManager.TotalCoins,
                            GameManager.CurrentLevel,
                            GameManager.EnemiesKilled))
                        {
                            var result = gameOverForm.ShowDialog();

                            if (result == DialogResult.Yes)
                            {
                                GameManager.StartNewGame();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
