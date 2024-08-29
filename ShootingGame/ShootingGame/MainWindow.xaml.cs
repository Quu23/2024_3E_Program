using System.Text;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ShootingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int FPS = 60;

        DispatcherTimer _updateTimer;
        public DrawCanvas drawCanvas;
        
        
        /// <summary>
        ///                        W      A      S      D    Space
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false };

        static Player player;

        public List<Bullet> bullets = new List<Bullet>();

        public MainWindow()
        {
            InitializeComponent();

            drawCanvas = new DrawCanvas();

            Content = drawCanvas;

            //player = new Player(new Image());
            
            //タイマーの設定 
            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
            _updateTimer.Tick += GameLoop;

            KeyUp   += DepressedKey;
            KeyDown += PressedKey;

            _updateTimer.Start();
        }

        // TODO:ゲームループの実装
        private void GameLoop(object? sender, EventArgs e)
        {
            
        }



        private void PressedKey(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    isKeyPresseds[0] = true; 
                    break;
                case Key.A:
                    isKeyPresseds[1] = true;
                    break;
                case Key.S:
                    isKeyPresseds[2] = true;
                    break;
                case Key.D:
                    isKeyPresseds[3] = true;
                    break;
                case Key.Space:
                    isKeyPresseds[4] = true;
                    break;
                case Key.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        private void DepressedKey(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    isKeyPresseds[0] = false;
                    break;
                case Key.A:
                    isKeyPresseds[1] = false;
                    break;
                case Key.S:
                    isKeyPresseds[2] = false;
                    break;
                case Key.D:
                    isKeyPresseds[3] = false;
                    break;
                case Key.Space:
                    isKeyPresseds[4] = false;
                    break;
                default:
                    break;
            }
        }
    }
}