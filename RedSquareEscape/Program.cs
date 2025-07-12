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

            // Show loading form first
            FormLoading loadingForm = new FormLoading();
            Application.Run(loadingForm);

            // After loading, show main menu
            FormMenu mainMenu = new FormMenu();
            Application.Run(mainMenu);
        }
    }
}
