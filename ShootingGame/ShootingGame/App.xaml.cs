using System.Configuration;
using System.Data;
using System.Windows;

namespace ShootingGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow window;

        App(){
            window = new MainWindow();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            window = new MainWindow();
            window.Show();
        }
    }

}
