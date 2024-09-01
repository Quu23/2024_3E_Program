using System.Diagnostics;
using System.Globalization;
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
    /// 画面を管理し、描画処理も行う。ゲームロジックもここに記入。
    /// </summary>
    public partial class MainWindow : Window
    {
        const int FPS = 60;

        DispatcherTimer _updateTimer;
        private VisualCollection visuals;
        
        
        /// <summary>
        ///                        W      A      S      D    Space
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false };

        public Player player;

        public List<Bullet> bullets = new List<Bullet>();
        private List<Bullet> bulletsForDelete = new List<Bullet>();

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private readonly BitmapImage backgroundImage;
        private Rect backgroundRect;

        public MainWindow()
        {
            visuals = new VisualCollection(this);

            InitializeComponent();

            player = new Player();

            //タイマーの設定 
            _updateTimer = new DispatcherTimer();
            _updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
            _updateTimer.Tick += Timer_Tick;

            KeyUp   += DepressedKey;
            KeyDown += PressedKey;

            backgroundImage = new BitmapImage(ImageUris.BACKGROUND);
            backgroundRect  = new Rect(0, 0 , backgroundImage.Width, backgroundImage.Height);

            _updateTimer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // 5がちょうどいい。
            backgroundAnimationCounter += player.Speed - 1;
            if (backgroundAnimationCounter > 1080) backgroundAnimationCounter = 0;

            GameLoop();

            // 画面の再描画
            visuals.Clear();
            visuals.Add(CreateDrawingVisual());
        }

        private DrawingVisual CreateDrawingVisual()
        {
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext drawingContext = dv.RenderOpen())
            {
                
                backgroundRect.Y = backgroundAnimationCounter;
                drawingContext.DrawImage(backgroundImage, backgroundRect);
                backgroundRect.Y = backgroundAnimationCounter - 1080;
                drawingContext.DrawImage(backgroundImage, backgroundRect);

                drawingContext.DrawImage(App.window.player.img, App.window.player.Rect);

                foreach (var bl in App.window.bullets)
                {
                    drawingContext.DrawImage(bl.img, bl.Rect);
                }

                //hack:FormattedTextを使うのは非推奨？調べたほうがいい（とりあえず動く）
                drawingContext.DrawText(new FormattedText($"bullets.Count = {bullets.Count}\nt={backgroundAnimationCounter}"
                                        , CultureInfo.GetCultureInfo("en")
                                        , FlowDirection.LeftToRight
                                        , new Typeface("Verdana")
                                        , 36
                                        , Brushes.White
                                        , 12.5)
                                        , new Point(10, 10));
            }
            return dv;
        }

        protected override int VisualChildrenCount => visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            return visuals[index];
        }

        // TODO:ゲームループの実装
        private void GameLoop()
        { 

            if (player.BulletCoolTime > 0) player.BulletCoolTime--;

            player.Move();


            foreach (Bullet bullet in bullets)
            {
                bullet.Move();
                if (bullet.Y < 0 || bullet.Y > ActualHeight)
                {
                    bulletsForDelete.Add(bullet);
                }
            }
            foreach (Bullet bullet in bulletsForDelete)
            {
                bullets.Remove(bullet);
            }

            if (isKeyPresseds[4] && player.BulletCoolTime <= 0)
            {
                bullets.AddRange(player.ShotBullet());
                player.BulletCoolTime = player.MaxBulletCoolTime;
            }
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

                //デバック用
                case Key.L:
                    player.Speed++;
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