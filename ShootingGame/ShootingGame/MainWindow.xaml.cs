using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        ///                                     W      A      S      D    Space    Tab
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false, false,};

        public Player player;

        public List<Enemy> enemies = new List<Enemy>();
        private List<Enemy> enemiesForDelete = new List<Enemy>();

        public List<Bullet> bullets = new List<Bullet>();
        private List<Bullet> bulletsForDelete = new List<Bullet>();

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private readonly BitmapImage backgroundImage;
        private Rect backgroundRect;


        protected override int VisualChildrenCount => visuals.Count;

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
            backgroundRect  = new Rect(0, 0 , 1920, 1080);

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

            //描画処理。JavaでいうGraphics
            using (DrawingContext drawingContext = dv.RenderOpen())
            {
                
                backgroundRect.Y = backgroundAnimationCounter;
                drawingContext.DrawImage(backgroundImage, backgroundRect);
                backgroundRect.Y = backgroundAnimationCounter - 1080;
                drawingContext.DrawImage(backgroundImage, backgroundRect);

                drawingContext.DrawImage(player.img, player.Rect);
                if (isKeyPresseds[5]) DrawHitRange(drawingContext, player);

                foreach (var bl in bullets)
                {
                    drawingContext.DrawImage(bl.img, bl.Rect);
                    if (isKeyPresseds[5])DrawHitRange(drawingContext, bl);
                }

                foreach (var en in enemies)
                {
                    drawingContext.DrawImage(en.img, en.Rect);
                    if (isKeyPresseds[5])DrawHitRange(drawingContext, en);
                }

                //hack:FormattedTextを使うのは非推奨？調べたほうがいい（とりあえず動く）<-誰かこれ何とかして～。
                if (isKeyPresseds[5])
                {
                    drawingContext.DrawText(new FormattedText($"t={backgroundAnimationCounter}\nbullets.Count = {bullets.Count}\nenemies.Count = {enemies.Count}"
                                        , CultureInfo.GetCultureInfo("en")
                                        , FlowDirection.LeftToRight
                                        , new Typeface("Verdana")
                                        , 12
                                        , Brushes.White
                                        , 12.5), new Point(10, 10));
                }

            }
            return dv;
        }

        //VisualCollection用にoverrideした。多分使うのかな？
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

            if (enemies.Count <= 0)
            {
                enemies.Add(new SnakeEnemy(300 , 10, 1));
                enemies.Add(new StraightEnemy(600 , 10, 1));
                enemies.Add(new StraightEnemy(900 , 10, 1));
                enemies.Add(new StraightEnemy(1200, 10, 1));
                enemies.Add(new StraightEnemy(1500, 10, 1));
            }


            foreach (Bullet bullet in bullets)
            {
                bullet.Move();

                if (bullet.Y < 0 || bullet.Y > ActualHeight)
                {
                    bulletsForDelete.Add(bullet);
                    continue;
                }

                if (bullet.Id == Id.ENEMY && bullet.IsHit(player))
                {
                    player.Hp -= bullet.Damage;
                    bulletsForDelete.Add(bullet);
                    continue;
                }

                foreach (Enemy enemy in enemies) 
                {
                    if (bullet.IsHit(enemy))
                    {
                        enemy.Hp -= bullet.Damage;
                        bulletsForDelete.Add(bullet);
                    }
                }


            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Move();
                if (enemy.Y > 1100 || enemy.Hp <= 0)
                {
                    enemiesForDelete.Add(enemy);
                }
            }
            foreach (Enemy enemy in enemiesForDelete)
            {
                enemies.Remove(enemy);
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

        private void DrawHitRange(DrawingContext dc, Entity target)
        {
            SolidColorBrush colorBrush = Brushes.ForestGreen.Clone();
            colorBrush.Opacity = 0.25;
            dc.DrawEllipse(colorBrush, new Pen(Brushes.DarkGreen , 1) ,new Point(target.X+target.Radius,target.Y+target.Radius),target.Radius,target.Radius);
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
                case Key.Tab:
                    isKeyPresseds[5] = true;
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

                case Key.Tab:
                    isKeyPresseds[5] = false;
                    break;
                default:
                    break;
            }
        }
    }
}