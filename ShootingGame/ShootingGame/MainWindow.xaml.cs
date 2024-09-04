using System.Diagnostics;
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

        DispatcherTimer updateTimer;
        private VisualCollection visuals;
        
        
        /// <summary>
        ///                                     W      A      S      D    Space    Enter  Tab
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false, false, false};

        public Player player;

        public List<Enemy> enemies = new List<Enemy>();
        private List<Enemy> enemiesForDelete = new List<Enemy>();

        public List<Bullet> bullets = new List<Bullet>();
        private List<Bullet> bulletsForDelete = new List<Bullet>();

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private readonly BitmapImage backgroundImage;
        private Rect backgroundRect;

        private readonly Pen hpBarPen;
        private Rect hpBarRect;

        private readonly Point statusPoint;

        readonly DateTime GAME_START_TIME;
        TimeSpan spf;


        protected override int VisualChildrenCount => visuals.Count;

        public MainWindow()
        {
            GAME_START_TIME = DateTime.Now;

            visuals = new VisualCollection(this);

            InitializeComponent();

            player = new Player();

            //タイマーの設定 
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
            updateTimer.Tick += Timer_Tick;

            KeyUp   += DepressedKey;
            KeyDown += PressedKey;

            // https://qiita.com/tera1707/items/15fd23cab641c75945b9
            backgroundImage = new BitmapImage(ImageUris.BACKGROUND);
            backgroundRect  = new Rect(0, 0 , SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            hpBarPen  = new Pen(Brushes.Black, 1);
            hpBarRect = new Rect(1600/1934.0 * SystemParameters.PrimaryScreenWidth, 1030/1094.0 * SystemParameters.PrimaryScreenHeight, player.GetMaxHp * 5, 10);

            statusPoint = new Point(hpBarRect.X - 50, hpBarRect.Y - 10);

            updateTimer.Start();
        }


        private DateTime start;
        private DateTime end;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            end = DateTime.Now;
            spf = end - start;
            start = DateTime.Now;

            // 5がちょうどいい。
            backgroundAnimationCounter += player.Speed - 1;
            if (backgroundAnimationCounter > SystemParameters.PrimaryScreenHeight) backgroundAnimationCounter = 0;

            GameLoop();
            
            // 画面の再描画
            visuals.Clear();
            visuals.Add(CreateDrawingVisual());

            //Debug.WriteLine(1.0 / spf.TotalSeconds);
            
        }

        // TODO:ゲームループの実装
        private void GameLoop()
        {
            //とりあえず必要経験経験値=現在のレベル^2 + 5　にしている。
            if (player.Exp >= player.Level * player.Level + 5 ||/*デバック用*/ isKeyPresseds[5] && isKeyPresseds[6]) player.LevelUp();
            
            //playerが死んだら即終了。ゲームオーバー画面作るならここを修正。
            if(player.Hp <= 0) Environment.Exit(0);

            if (player.BulletCoolTime > 0) player.BulletCoolTime--;

            player.Move();

            //todo:敵の配置とか種類をいじるならここを修正。
            if (enemies.Count <= 0)
            {
                int dw = (int)((Width-200) / 5.0);
                enemies.Add(new SnakeEnemy(dw, 10, 1));
                enemies.Add(new StraightEnemy(2*dw, 10, 1));
                enemies.Add(new StraightEnemy(3*dw, 10, 1));
                enemies.Add(new StraightEnemy(4*dw, 10, 1));
                enemies.Add(new ShotgunEnemy(5*dw, 10, 1));
            }


            foreach (Bullet bullet in bullets)
            {
                bullet.Move();

                if (bullet.Y < 0 || bullet.Y > Height)
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
                    if (bullet.Id == Id.PLAYER && bullet.IsHit(enemy))
                    {
                        enemy.Hp -= bullet.Damage;
                        bulletsForDelete.Add(bullet);

                        if(enemy.Hp <= 0)
                        {
                            player.Exp += enemy.GetEXP();
                            enemy.isDead = true;
                            enemiesForDelete.Add(enemy);
                        }
                    }
                }
            }


            foreach (Bullet bullet in bulletsForDelete)
            {
                bullets.Remove(bullet);
            }

            foreach (Enemy enemy in enemies)
            {
                if(enemy.isDead)continue;

                if (enemy.BulletCoolTime > 0) enemy.BulletCoolTime--;
                enemy.Move();

                if (player.IsHit(enemy))
                {
                    player.Hp = 0;
                }
                if (enemy.Y > SystemParameters.PrimaryScreenHeight)
                {
                    enemiesForDelete.Add(enemy);
                    continue;
                }
                if (enemy.BulletCoolTime <= 0)
                {
                    bullets.AddRange(enemy.ShotBullet());
                    enemy.BulletCoolTime = enemy.MaxBulletCoolTime;
                }
            }
            foreach (Enemy enemy in enemiesForDelete)
            {
                enemies.Remove(enemy);
            }

            if (isKeyPresseds[4] && player.BulletCoolTime <= 0)
            {
                bullets.AddRange(player.ShotBullet());
                player.BulletCoolTime = player.MaxBulletCoolTime;
            }
        }

        private DrawingVisual CreateDrawingVisual()
        {
            DrawingVisual dv = new DrawingVisual();

            //描画処理。JavaでいうGraphics
            using (DrawingContext drawingContext = dv.RenderOpen())
            {
                
                backgroundRect.Y = backgroundAnimationCounter;
                drawingContext.DrawImage(backgroundImage, backgroundRect);
                backgroundRect.Y = backgroundAnimationCounter - SystemParameters.PrimaryScreenHeight;
                drawingContext.DrawImage(backgroundImage, backgroundRect);

                drawingContext.DrawImage(player.img, player.Rect);
                if (isKeyPresseds[6]) DrawHitRange(drawingContext, player);

                foreach (var bl in bullets)
                {
                    drawingContext.DrawImage(bl.img, bl.Rect);
                    if (isKeyPresseds[6])DrawHitRange(drawingContext, bl);
                }

                foreach (var en in enemies)
                {
                    drawingContext.DrawImage(en.img, en.Rect);
                    if (isKeyPresseds[6])DrawHitRange(drawingContext, en);
                }


                drawingContext.DrawText(new FormattedText($"EXP:{player.Exp}\nLV_:{player.Level}"
                                        , CultureInfo.GetCultureInfo("en")
                                        , FlowDirection.LeftToRight
                                        , new Typeface("Verdana")
                                        , 10
                                        , Brushes.White
                                        , 12.5), statusPoint);
                hpBarRect.Width = player.GetMaxHp * 10;
                drawingContext.DrawRectangle(Brushes.White, hpBarPen, hpBarRect);
                hpBarRect.Width = player.Hp * 10;
                drawingContext.DrawRectangle(Brushes.Red  , hpBarPen, hpBarRect);


                if (isKeyPresseds[6])
                {
                    //hack:毎回newするのは効率悪いからDrawText()以外のいい方法が欲しい。
                    drawingContext.DrawText(new FormattedText(
                                        $"Width = {Width} / Height = {Height}\n" +
                                        $"backgroundAnimationCounter={backgroundAnimationCounter}\n" +
                                        $"bullets.Count  = {bullets.Count}\n" +
                                        $"enemies.Count  = {enemies.Count}\n" +
                                        $"program uptime = {(DateTime.Now - GAME_START_TIME).TotalSeconds}\n" +
                                        $"fps = {1.0/spf.TotalSeconds}\n"
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
                case Key.Enter:
                    isKeyPresseds[5] = true;
                    break;

                //デバック用
                case Key.L:
                    player.Speed++;
                    break;
                case Key.Tab:
                    isKeyPresseds[6] = true;
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
                case Key.Enter:
                    isKeyPresseds[5] = false;
                    break;

                case Key.Tab:
                    isKeyPresseds[6] = false;
                    break;
                default:
                    break;
            }
        }
    }
}