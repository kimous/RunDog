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
