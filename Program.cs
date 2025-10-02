using System.Globalization;
using System.Threading;

namespace RunDog
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Définir la culture pour le thread actuel en fonction de la culture de l'OS
            Thread.CurrentThread.CurrentCulture = CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;

            // CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InstalledUICulture; // Cette ligne peut être commentée ou supprimée si les lignes ci-dessus sont utilisées

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);;

            // Au lieu de lancer une Form, nous lançons un ApplicationContext.
            // C'est la méthode standard pour créer une application qui s'exécute 
            // uniquement dans la barre des tâches (systray) sans fenêtre principale.
            Application.Run(new RunDogApplicationContext());
        }
    }
}
