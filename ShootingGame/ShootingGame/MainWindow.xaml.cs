using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ShootingGame.Entities;
using ShootingGame.Entities.Items;
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


        private static string message = ""; 

        public static int score = 0;

        const int FPS = 60;

        DispatcherTimer updateTimer;
        private VisualCollection visuals;


        /// <summary>
        ///                                     W      A      S      D    Space    Enter  Tab   R_SHIFT
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false, false, false, false };

        public Player player;

        public List<Enemy> enemies = new List<Enemy>();

        public List<Bullet> bullets = new List<Bullet>();

        public List<Item> items = new List<Item>();

        private MediaPlayer musicPlayer;

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private readonly BitmapImage backgroundImage;
        private Rect backgroundRect;

        //タイトルなどのロゴ関係
        private Rect titleRect;
        private Rect modeSelectionTextRect;
        //GAMEOVERもしくはGAMECLEARになったときにインスタンスを代入する。
        private FormattedText scoreText = null;

        private readonly Pen hpBarPen;
        private Rect hpBarRect;

        private readonly Point statusPoint;
        private readonly Point scorePoint;

        private Rect statusIconRect;

        readonly DateTime GAME_START_TIME;
        TimeSpan spf;


        protected override int VisualChildrenCount => visuals.Count;

        public MainWindow()
        {
            GAME_START_TIME = DateTime.Now;

            visuals = new VisualCollection(this);

            InitializeComponent();

            windowMode = WindowMode.START;

            player = new Player("ななし");

            //タイマーの設定 
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000 / FPS);
            updateTimer.Tick += Timer_Tick;

            KeyUp += DepressedKey;
            KeyDown += PressedKey;
            KeyDown += MessageInputKey;

            // BGM再生の設定
            musicPlayer = new MediaPlayer();
            musicPlayer.Open(UtilityUris.BGM1_URI);
            //musicPlayer.Position = new TimeSpan(0,1,40);
            musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
            {
                musicPlayer.Position = TimeSpan.Zero;
            };
            musicPlayer.IsMuted = true;
            musicPlayer.Play();

            //背景アニメーション設定
            // https://qiita.com/tera1707/items/15fd23cab641c75945b9
            backgroundImage = Images.BACKGROUND_IMAGE;
            backgroundRect = new Rect(0, 0, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            //タイトルロゴ等の設定
            titleRect = new Rect((SystemParameters.PrimaryScreenWidth - Images.TITLE_IMAGE.Width) / 2, (SystemParameters.PrimaryScreenHeight - Images.MODE_SELECT_TEXT_IMAGE.Height) / 8, 700, 100);//割る4ぐらいがちょうどいい（適当）
            modeSelectionTextRect = new Rect((SystemParameters.PrimaryScreenWidth - Images.MODE_SELECT_TEXT_IMAGE.Width) / 2, (SystemParameters.PrimaryScreenHeight - Images.MODE_SELECT_TEXT_IMAGE.Height) * 6 / 8, 700, 300);

            hpBarPen = new Pen(Brushes.Black, 1);
            hpBarRect = new Rect(1600 / 1934.0 * SystemParameters.PrimaryScreenWidth, 1030 / 1094.0 * SystemParameters.PrimaryScreenHeight, player.GetMaxHp * 5, 10);

            scorePoint  = new Point(30, 30);
            statusPoint = new Point(hpBarRect.X - 50, hpBarRect.Y - 10);

            statusIconRect = new Rect(SystemParameters.PrimaryScreenWidth - 32  , /*起点Y=*/SystemParameters.PrimaryScreenHeight - 32 * (player.status.Count + 1), 32, 32);

            //データ読み込み
            //LoadData();

            updateTimer.Start();

        }

        private void LoadData()
        {
            throw new NotImplementedException();
        }

        private void WriteScore()
        {
            throw new NotImplementedException();
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
            if (isKeyPresseds[4]) { 
                windowMode = WindowMode.STAGE1;
                if(message.Length > 0)player = new Player(message);
            }
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
            //とりあえず必要経験経験値=現在のレベル^3 + 5　にしている。
            if (player.Exp >= player.Level * player.Level * player.Level + 5 ||/*デバック用*/ isKeyPresseds[5] && isKeyPresseds[6]) player.LevelUp();

            //playerが死んだら即終了。ゲームオーバー画面作るならここを修正。
            if (player.Hp <= 0)
            {
                windowMode = WindowMode.GAMEOVER;
                scoreText = new FormattedText($"SCORE = {score}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 100
                                    , Brushes.White
                                    , 12.5);
            }

            player.Action();

            //todo:敵の配置とか種類をいじるならここを修正。
            if (enemies.Count <= 0)
            {
                int dw = (int)((Width - 200)/ 5.0 );
                enemies.Add(new HexagonEnemy(dw, 10, 1));
                enemies.Add(new SnakeEnemy(2*dw, 10, 1));
                enemies.Add(new SplitEnemy(3*dw, 10, 1));
                enemies.Add(new TurnBackEnemy(4*dw, 10, 1));
                enemies.Add(new MissileEnemy(5*dw, 10, 1));

            }

            //todo:アイテムの位置とか種類をいじるならここ。
            if (items.Count <= 0)
            {
                int dw = (int)((Width - 200) / 7.0);
                items.Add(new ClearEnemiesItem(dw, 30));
                items.Add(new ExpOrb(2 * dw, 30));
                items.Add(new HealingItem(3 * dw, 30));
                items.Add(new InvincibleItem(4 * dw, 30));
                items.Add(new ShotRateUpItem(5 * dw, 30));
                items.Add(new SpeedUpItem(6 * dw, 30));
                items.Add(new DestroyItem(7 * dw, 30));
            }


            for (int bi = bullets.Count; bi > 0; bi--)
            {
                Bullet tmp_bullet = bullets[bi - 1];
                tmp_bullet.Action();

                if (tmp_bullet.IsOutOfWindow())
                {
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                if (tmp_bullet.Id == Id.ENEMY && player.IsHit(tmp_bullet))
                {
                    player.Hp -= tmp_bullet.Damage;
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                for (int ei = enemies.Count; ei > 0; ei--)
                {
                    Enemy tmp_enemy = enemies[ei - 1];
                    if (tmp_bullet.Id == Id.PLAYER && tmp_enemy.IsHit(tmp_bullet))
                    {
                        tmp_enemy.Hp -= tmp_bullet.Damage;
                        bullets.Remove(tmp_bullet);

                        if (tmp_enemy.Hp <= 0)
                        {
                            tmp_enemy.DeadAction(player,enemies);
                        }
                    }
                }
            }

            for (int ei = enemies.Count; ei > 0; ei--)
            {
                Enemy tmp_enemy = enemies[ei - 1];

                tmp_enemy.Action();

                if (player.IsHit(tmp_enemy))
                {
                    player.Hp = 0;
                }
                if (tmp_enemy.IsOutOfWindow())
                {
                    enemies.Remove(tmp_enemy);
                    continue;
                }
            }

            for (int i = items.Count; i > 0; i--)
            {
                Item tmp_item = items[i - 1];

                tmp_item.Action();
                if (player.IsHit(tmp_item))
                {
                    tmp_item.MakeEffect(player);
                    items.Remove(tmp_item);
                    continue;
                }
                if (tmp_item.IsOutOfWindow())
                {
                    items.Remove(tmp_item);
                    continue;
                }
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

            drawingContext.DrawText(new FormattedText(
                                     $"NAME = [{message}]"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 30
                                    , Brushes.Yellow
                                    , 12.5), new Point(SystemParameters.FullPrimaryScreenWidth / 2 - 30 * 7, modeSelectionTextRect.Y - 40));
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

            foreach (var item in items)
            {
                drawingContext.DrawImage(item.Img, item.Rect);
                if (isKeyPresseds[6]) DrawHitRange(drawingContext, item);
            }

            drawingContext.DrawText(new FormattedText($"SCORE:{score}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 20
                                    , Brushes.White
                                    , 12.5), scorePoint);

            drawingContext.DrawText(new FormattedText($"EXP:{player.Exp}\nLV_:{player.Level}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 10
                                    , Brushes.White
                                    , 12.5), statusPoint);
            hpBarRect.Width = player.GetMaxHp * 10;
            drawingContext.DrawRectangle(Brushes.White, hpBarPen, hpBarRect);
            hpBarRect.Width = player.Hp >= 0 ? player.Hp * 10 : 0;
            drawingContext.DrawRectangle(Brushes.Red, hpBarPen, hpBarRect);

            foreach (var kvp in player.status)
            {
                if (kvp.Value > 0)
                {
                    drawingContext.DrawImage(GetStatusIconImage(kvp.Key), statusIconRect);
                }
                statusIconRect.Y += statusIconRect.Height;
            }
            statusIconRect.Y -= statusIconRect.Height * player.status.Count ;


            if (isKeyPresseds[6])
            {
                //hack:毎回newするのは効率悪いからDrawText()以外のいい方法が欲しい。
                drawingContext.DrawText(new FormattedText(
                                    $"Width = {Width} / Height = {Height}\n" +
                                    $"backgroundAnimationCounter={backgroundAnimationCounter}\n" +
                                    $"bullets.Count  = {bullets.Count}\n" +
                                    $"enemies.Count  = {enemies.Count}\n" +
                                    $"items.Count    = {items.Count}\n" +
                                    $"program uptime = {(DateTime.Now - GAME_START_TIME).TotalSeconds}\n" +
                                    $"fps = {1.0 / spf.TotalSeconds}\n" +
                                    $"Speed = {player.Speed}\n" +
                                    $"BGM Muted = {musicPlayer.IsMuted}\n" +
                                    $"BGM Seconds = {musicPlayer.Position.Minutes}:{musicPlayer.Position.Seconds} / {musicPlayer.NaturalDuration.TimeSpan.Minutes}:{musicPlayer.NaturalDuration.TimeSpan.Seconds}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , new Typeface("Verdana")
                                    , 12
                                    , Brushes.White
                                    , 12.5), new Point(10, 10));
            }
        }

        private void DrawGameoverWindow(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(Images.GAMEOVER_IMAGE, titleRect);
            drawingContext.DrawText(scoreText, new Point(modeSelectionTextRect.X, modeSelectionTextRect.Y));
        }

        private void DrawGameclearWindow(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(Images.GAMECLEAR_IMAGE, titleRect);
            drawingContext.DrawText(scoreText, new Point(modeSelectionTextRect.X, modeSelectionTextRect.Y));
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
            dc.DrawEllipse(colorBrush, new Pen(Brushes.DarkGreen, 1), new Point(target.CenterX, target.CenterY), target.Radius, target.Radius);
        }

        private BitmapImage GetStatusIconImage(StatusEffects effect)
        {
            switch (effect) {
                case StatusEffects.SPEED_UP:
                    return Images.SPEED_UP_ICON_IMAGE;
                case StatusEffects.SPEED_DOWN:
                    return Images.SPEED_DOWN_ICON_IMAGE;
                case StatusEffects.SHOT_RATE_UP:
                    return Images.SHOT_RATE_UP_ICON_IMAGE;
                case StatusEffects.SHOT_RATE_DOWN:
                    return Images.SHOT_RATE_DOWN_ICON_IMAGE;
                case StatusEffects.INVINCIBLE:
                    return Images.INCINCIBLE_ICON_IMAGE;
                default:
                    throw new NotImplementedException();
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
                    isKeyPresseds[7] = true;
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
                    isKeyPresseds[7] = false;
                    break;
                default:
                    break;
            }
        }

        private void MessageInputKey(object? sender, KeyEventArgs e)
        {
            if (windowMode != WindowMode.START) return;

            if (e.Key == Key.Back && message.Length > 0)message = message.Remove(message.Length - 1, 1);
            
            // 名前の文字数制限は8文字
            if (message.Length > 8) return;

            if (e.Key >= Key.D0 && e.Key <= Key.D9) message　+= $"{e.Key - Key.D0}";

            if (e.Key >= Key.A  && e.Key <= Key.Z) message += e.Key.ToString();



        }

    }
}