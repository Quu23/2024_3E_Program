using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ShootingGame.Entities;
using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame
{
    /// <summary>
    /// 画面を管理し、描画処理も行う。ゲームロジックもここに記入。
    /// </summary>
    public partial class MainWindow : Window
    {
        public static WindowMode windowMode;

        const int FPS = 60;

        DispatcherTimer updateTimer;
        private VisualCollection visuals;

        private int selectForStartWindow = 0;
        
        
        /// <summary>
        ///                                     W      A      S      D    Space    Enter  Tab  
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false, false, false };

        public Player player;

        public List<Enemy> enemies = new List<Enemy>();

        public List<Bullet> bullets = new List<Bullet>();

        private MediaPlayer musicPlayer;

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private readonly BitmapImage backgroundImage;
        private Rect backgroundRect;

        //タイトルなどのロゴ関係
        private Rect titleRect;
        private Rect modeSelectionTextRect;

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

            windowMode = WindowMode.START;

            player = new Player();

            //タイマーの設定 
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
            updateTimer.Tick += Timer_Tick;

            KeyUp   += DepressedKey;
            KeyDown += PressedKey;

            // BGM再生の設定
            musicPlayer = new MediaPlayer();
            musicPlayer.Open(UtilityUris.BGM_URI);
            //musicPlayer.Position = new TimeSpan(0,1,40);
            musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
            {
                musicPlayer.Position = TimeSpan.Zero;
            };
            musicPlayer.Play();

            //背景アニメーション設定
            // https://qiita.com/tera1707/items/15fd23cab641c75945b9
            backgroundImage = Images.BACKGROUND_IMAGE;
            backgroundRect  = new Rect(0, 0 , SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            //タイトルロゴ等の設定
            titleRect = new Rect(SystemParameters.PrimaryScreenWidth / 4,250, 700,100);//割る4ぐらいがちょうどいい（適当）
            modeSelectionTextRect = new Rect(SystemParameters.PrimaryScreenWidth / 4, 500, 700, 300);

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

            // プレイヤーの移動速度とともに早くなる
            backgroundAnimationCounter += 5;
            if (backgroundAnimationCounter > SystemParameters.PrimaryScreenHeight) backgroundAnimationCounter = 0;

            switch (windowMode)
            {
                case WindowMode.START:
                    StartLoop();
                    break;
                case WindowMode.STAGE1:
                case WindowMode.STAGE1_BOSS:
                    GameLoop();
                    break;
                case WindowMode.STAGE2:
                case WindowMode.STAGE2_BOSS:
                    break;
                case WindowMode.STAGE3:
                case WindowMode.STAGE3_BOSS:
                    break;
                case WindowMode.GAMEOVER:
                    GameoverLoop();
                    break;
                case WindowMode.GAMECLEAR:
                    GameclearLoop();
                    break;
                case WindowMode.LANKING:
                    break;
            }
            
            // 画面の再描画
            visuals.Clear();
            visuals.Add(CreateDrawingVisual());

            //Debug.WriteLine(1.0 / spf.TotalSeconds);

        }

        private void StartLoop()
        {
            if (isKeyPresseds[4]) windowMode = WindowMode.STAGE1;
            if (isKeyPresseds[0] && selectForStartWindow > 0) selectForStartWindow--;
            if (isKeyPresseds[2] && selectForStartWindow < 2) selectForStartWindow++;

            if ((isKeyPresseds[0] || isKeyPresseds[2]) && isKeyPresseds[1]) selectForStartWindow = 1;
        }

        private void GameoverLoop()
        {

        }

        private void GameclearLoop()
        {

        }

        // TODO:ゲームループの実装
        private void GameLoop()
        {
            //とりあえず必要経験経験値=現在のレベル^2 + 5　にしている。
            if (player.Exp >= player.Level * player.Level * player.Level + 5 ||/*デバック用*/ isKeyPresseds[5] && isKeyPresseds[6]) player.LevelUp();
            
            //playerが死んだら即終了。ゲームオーバー画面作るならここを修正。
            if(player.Hp <= 0) Environment.Exit(0);

            if (player.BulletCoolTime > 0) player.BulletCoolTime-=player.DecreaceBulletCoolTime;

            player.Move();

            //todo:敵の配置とか種類をいじるならここを修正。
            if (enemies.Count <= 0)
            {
                int dw = (int)((Width-200) / 5.0);
                enemies.Add(new SnakeEnemy(dw, 10, 1));
                enemies.Add(new HexagonEnemy(2*dw, 10, 1));
                enemies.Add(new StraightEnemy(3*dw, 10, 1));
                enemies.Add(new TurnBackEnemy(4*dw, 10, 1));
                enemies.Add(new ShotgunEnemy(5*dw, 10, 1));
            }


            for (int bi = bullets.Count; bi > 0; bi--)
            {
                Bullet tmp_bullet = bullets[bi - 1];
                tmp_bullet.Move();

                if (tmp_bullet.X < 0 || tmp_bullet.X > SystemParameters.PrimaryScreenWidth || tmp_bullet.Y < 0 || tmp_bullet.Y > SystemParameters.PrimaryScreenHeight)
                {
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                if (tmp_bullet.Id == Id.ENEMY && tmp_bullet.IsHit(player))
                {
                    player.Hp -= tmp_bullet.Damage;
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                for (int ei = enemies.Count; ei > 0; ei--)
                {
                    Enemy tmp_enemy = enemies[ei - 1];
                    if (tmp_bullet.Id == Id.PLAYER && tmp_bullet.IsHit(tmp_enemy))
                    {
                        tmp_enemy.Hp -= tmp_bullet.Damage;
                        bullets.Remove(tmp_bullet);

                        if(tmp_enemy.Hp <= 0)
                        {
                            player.Exp += tmp_enemy.GetEXP();
                            enemies.Remove(tmp_enemy);
                        }
                    }
                }
            }

            for (int ei = enemies.Count; ei > 0; ei--)
            {
                Enemy tmp_enemy = enemies[ei - 1];
                if (tmp_enemy.BulletCoolTime > 0) tmp_enemy.BulletCoolTime-=tmp_enemy.DecreaceBulletCoolTime;
                tmp_enemy.Move();

                if (player.IsHit(tmp_enemy))
                {
                    player.Hp = 0;
                }
                if (tmp_enemy.Y > SystemParameters.PrimaryScreenHeight || tmp_enemy.Y < -tmp_enemy.Radius)
                {
                    enemies.Remove(tmp_enemy);
                    continue;
                }
                if (tmp_enemy.BulletCoolTime <= 0)
                {
                    bullets.AddRange(tmp_enemy.ShotBullet());
                    tmp_enemy.BulletCoolTime = tmp_enemy.MaxBulletCoolTime;
                }
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

                switch (windowMode)
                {
                    case WindowMode.START:
                        DrawStartWindow(drawingContext);
                        break;
                    case WindowMode.STAGE1:
                    case WindowMode.STAGE1_BOSS:
                        DrawGameWindow(drawingContext);
                        break;
                    case WindowMode.STAGE2:
                    case WindowMode.STAGE2_BOSS:
                        break;
                    case WindowMode.STAGE3:
                    case WindowMode.STAGE3_BOSS:
                        break;
                    case WindowMode.GAMEOVER:
                        DrawGameoverWindow(drawingContext);
                        break;
                    case WindowMode.GAMECLEAR:
                        DrawGameclearWindow(drawingContext);
                        break;
                    case WindowMode.LANKING:
                        break;
                }
            }
            return dv;
        }

        private void DrawStartWindow(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(Images.TITLE_IMAGE, titleRect);
            drawingContext.DrawImage(Images.MODE_SELECT_TEXT_IMAGE, modeSelectionTextRect);
            drawingContext.DrawImage(Images.STRAIGHT_ENEMY_IMAGE, new Rect(500,selectForStartWindow*70+560 , 40 ,40));
        }

        private void DrawGameWindow(DrawingContext drawingContext)
        {

            drawingContext.DrawImage(player.Img, player.Rect);
            if (isKeyPresseds[6]) DrawHitRange(drawingContext, player);

            foreach (var bullet in bullets)
            {
                drawingContext.DrawImage(bullet.Img, bullet.Rect);
                if (isKeyPresseds[6]) DrawHitRange(drawingContext, bullet);
            }

            foreach (var enemy in enemies)
            {
                drawingContext.DrawImage(enemy.Img, enemy.Rect);
                if (isKeyPresseds[6]) DrawHitRange(drawingContext, enemy);
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
            drawingContext.DrawRectangle(Brushes.Red, hpBarPen, hpBarRect);


            if (isKeyPresseds[6])
            {
                //hack:毎回newするのは効率悪いからDrawText()以外のいい方法が欲しい。
                drawingContext.DrawText(new FormattedText(
                                    $"Width = {Width} / Height = {Height}\n" +
                                    $"backgroundAnimationCounter={backgroundAnimationCounter}\n" +
                                    $"bullets.Count  = {bullets.Count}\n" +
                                    $"enemies.Count  = {enemies.Count}\n" +
                                    $"program uptime = {(DateTime.Now - GAME_START_TIME).TotalSeconds}\n" +
                                    $"fps = {1.0 / spf.TotalSeconds}\n" +
                                    $"Speed = {player.Speed}\n" +
                                    $"BGM Seconds = {musicPlayer.Position.Minutes}:{musicPlayer.Position.Seconds} / {musicPlayer.NaturalDuration.TimeSpan.Minutes}:{musicPlayer.NaturalDuration.TimeSpan.Seconds}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 12
                                    , Brushes.White
                                    , 12.5), new Point(10, 10));
            }
        }

        private void DrawGameoverWindow(DrawingContext drawingVisual)
        {

        }

        private void DrawGameclearWindow(DrawingContext drawingContext)
        {

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
            dc.DrawEllipse(colorBrush, new Pen(Brushes.DarkGreen , 1) ,new Point(target.CenterX,target.CenterY),target.Radius,target.Radius);
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
                case Key.RightShift:
                    if (player.Speed != player.defaultSpeed * 3)
                    {
                        player.Speed *= 3;
                    }
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
                case Key.RightShift:
                    player.Speed /= 3;
                    break;
                default:
                    break;
            }
        }
    }
}
