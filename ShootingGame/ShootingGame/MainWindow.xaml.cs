﻿using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ShootingGame.Entities;
using ShootingGame.Entities.Items;
using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace ShootingGame
{
    /// <summary>
    /// 画面を管理し、描画処理も行う。ゲームロジックもここに記入。
    /// </summary>
    public partial class MainWindow : Window
    {
        public static WindowMode windowMode;

        //　画面横の黒画面用
        public int moveableLeftSidePosition;
        public int moveableRightSidePosition;
        public Rect unmoveableAreaRect;
        public Pen  unmoveableAreaPen;


        private static string message = "";

        private SortedDictionary<int, string> ranking;
        /// <summary>
        /// pos,enemy or item,type,x
        /// </summary>
        private List<(int, int, int, int)> stageData;

        public static int stagePosition;

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

        private readonly Typeface FONT_TYPEFACE;

        readonly DateTime GAME_START_TIME;
        TimeSpan spf;


        protected override int VisualChildrenCount => visuals.Count;

        public MainWindow()
        {
            GAME_START_TIME = DateTime.Now;

            visuals = new VisualCollection(this);

            InitializeComponent();

            Loaded += (sender, e) =>
            {
                moveableLeftSidePosition = (int)(Width / 4);
                moveableRightSidePosition = (int)(Width - moveableLeftSidePosition);
                unmoveableAreaRect = new Rect(0, 0, moveableLeftSidePosition, Height - 15);
            };
            unmoveableAreaPen = new Pen(Brushes.Yellow, 1);


            windowMode = WindowMode.START;

            player = new Player("ななし");
            Debug.WriteLine(player.X);

            //タイマーの設定 
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000 / FPS);
            updateTimer.Tick += Timer_Tick;

            KeyUp += DepressedKey;
            KeyDown += PressedKey;
            KeyDown += MessageInputKey;

            // BGM再生の設定
            musicPlayer = new MediaPlayer();
            musicPlayer.Open(UtilityUris.BGM2_URI);
            //musicPlayer.Position = new TimeSpan(0,1,40);
            musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
            {
                musicPlayer.Position = TimeSpan.Zero;
            };
            musicPlayer.IsMuted = false;
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

            scorePoint  = new Point(30, 0);
            statusPoint = new Point(hpBarRect.X - 80, hpBarRect.Y - 10);

            statusIconRect = new Rect(SystemParameters.PrimaryScreenWidth - 32  , /*起点Y=*/SystemParameters.PrimaryScreenHeight - 32 * (player.status.Count + 1), 32, 32);

            FONT_TYPEFACE = new Typeface((FontFamily)Application.Current.Resources["dotfont"], FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            //データ読み込み

            //SortedDictonaryは小さい順に並べるので、SCOREが大きいほど前（順序でいえば小さい）になる。
            ranking = new SortedDictionary<int, string>(Comparer<int>.Create((int x,int y) =>
            {
                if (x > y) return -1;
                if (x < y) return  1;
                return 0;
            }));
            LoadRankingData();

            stageData = new List<(int, int, int, int)>();
            LoadStageData(WindowMode.STAGE1);

            stagePosition = 0;

            updateTimer.Start();

        }

        private void LoadRankingData()
        {
            string path = @"../../../data/score_board.txt";

            // ファイル読み込み＆文字化け防止
            var lines = File.ReadAllLines(path, Encoding.GetEncoding("UTF-8"));

            // 1行ずつ読み込み
            foreach (var line in lines)
            {
                string[] temp = line.Split(',');

                int temp2 = Convert.ToInt32(temp[0]);

                ranking.Add(temp2,temp[1]);
            }
        }

        private void LoadStageData(WindowMode nextMode)
        {
            string path;
            switch (nextMode)
            {
                case WindowMode.STAGE1:
                    path = @"../../../data/stages/stage1.txt";
                    break;
                case WindowMode.STAGE2:
                    path = @"../../../data/stages/stage2.txt";
                    break;
                case WindowMode.STAGE3:
                    path = @"../../../data/stages/stage3.txt";
                    break;
                default:
                    throw new ArgumentException($"nextModeにはSTAGE1,STAGE2,STAGE3のみが指定されるべきです。 nextMode={nextMode.ToString()}");
            }
            // ファイル読み込み＆文字化け防止
            var lines = File.ReadAllLines(path, Encoding.GetEncoding("UTF-8"));

            // 1行ずつ読み込み
            foreach (var line in lines)//ステージデータは一つにつき要素を4つ持っている。[stage_position(int) , 0 / 1 , enemy_type(EnemyType) / item_type(ItemType) , x (int),]
            {
                string[] temp = line.Split(',');
                int[] data = new int[temp.Length];

                for(int i = 0; i < 4; i++)
                {
                    int temp2 = Convert.ToInt32(temp[i]);
                    data[i] = temp2;
                }
                stageData.Add((data[0], data[1], data[2], data[3]));
            }
        }

        private void WriteScore()
        {
            // 書き込むファイルの絶対パス
            string path = @"../../../data/score_board.txt";

            // ファイルを開く＆文字化け防止
            // 第二引数が「true」 → 追加書き込みOK
            // 　　　　　「false」→ 追加書き込みせず、上書きして書き込む
            using(StreamWriter file = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
            {
                // score_board.txt に書き込み
                file.WriteLine($"{score},{player.name}");
            }
            

            // 書き込んだファイルを読み込む
            Console.WriteLine(File.ReadAllText(path, Encoding.GetEncoding("UTF-8")));
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
                case WindowMode.DEBUG:
                    DebugModeLoop();
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
                if (message.Equals("DEBUG"))
                {
                    windowMode = WindowMode.DEBUG;
                    player = new Player("DebugMode");
                    player.LevelUp(30);
                    return;
                }

                windowMode = WindowMode.STAGE1;
                if (message.Length > 0)
                {
                    player = new Player(message);
                }
            }
        }

        private void DebugModeLoop()
        {
            if (enemies.Count <= 0)
            {
                int dw = (int)((Width - 2 * moveableLeftSidePosition) / 5.0);

                int basicX = moveableLeftSidePosition - 50;

                enemies.Add(new SplitEnemy(dw + basicX, 10, 1));
                enemies.Add(new CycloneEnemy(2 * dw + basicX, 10, 1));
                enemies.Add(new SplashEnemy(3 * dw + basicX, 10, 1));
                enemies.Add(new LaserEnemy(4 * dw + basicX, 10, 1));
                enemies.Add(new BigEnemy(5 * dw + basicX, 10, 1));

            }

            //if (items.Count <= 0)
            //{
            //    int dw = (int)((Width - 200) / 7.0);
            //    items.Add(new ClearEnemiesItem(dw, 30));
            //    items.Add(new ExpOrb(2 * dw, 30));
            //    items.Add(new HealingItem(3 * dw, 30));
            //    items.Add(new InvincibleItem(4 * dw, 30));
            //    items.Add(new ShotRateDownItem(5 * dw, 30));
            //    items.Add(new SpeedDownItem(6 * dw, 30));
            //    items.Add(new DestroyItem(7 * dw, 30));
            //}

            BasicGameLogic();
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

            for (int i = 0; i < stageData.Count; i++)
            {
                var pair = stageData[i];

                if (stagePosition < pair.Item1) break;

                if (pair.Item2 == 0)
                {
                    enemies.Add(UtilityGenerater.GenerateEnemy((EnemyTypes)pair.Item3, pair.Item4, 0, 1));
                }
                else
                {
                    items.Add(UtilityGenerater.GenerateItem((ItemTypes)pair.Item3, pair.Item4, 0));
                }
                stageData.Remove(pair);
            }

            BasicGameLogic();    
        }

        private void BasicGameLogic()
        {
            stagePosition++;

            //とりあえず必要経験経験値=現在のレベル^3 + 5　にしている。
            if (player.Exp >= player.Level * player.Level * player.Level + 5 ||/*デバック用*/ isKeyPresseds[5] && isKeyPresseds[6]) player.LevelUp();

            //playerが死んだら即終了。ゲームオーバー画面作るならここを修正。
            if (player.Hp <= 0)
            {
                windowMode = WindowMode.GAMEOVER;
                scoreText = new FormattedText($"SCORE = {score}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , FONT_TYPEFACE
                                    , 100
                                    , Brushes.White
                                    , 12.5);

                WriteScore();
            }

            player.Action();

            for (int bi = bullets.Count; bi > 0; bi--)
            {

                Bullet tmp_bullet = bullets[bi - 1]; Debug.WriteLine($"{bi}:{tmp_bullet.ToString()}");
                tmp_bullet.Action();

                if (tmp_bullet.IsOutOfWindow())
                {
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                if (tmp_bullet.Type != EnemyTypes.PLAYER && player.IsHit(tmp_bullet))
                {
                    player.Hp -= tmp_bullet.Damage;
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                for (int ei = enemies.Count; ei > 0; ei--)
                {
                    Enemy tmp_enemy = enemies[ei - 1];
                    if (tmp_bullet.Type == EnemyTypes.PLAYER && tmp_enemy.IsHit(tmp_bullet))
                    {
                        tmp_enemy.Hp -= tmp_bullet.Damage;
                        bullets.Remove(tmp_bullet);

                        if (tmp_enemy.Hp <= 0)
                        {
                            tmp_enemy.DeadAction(player, enemies, items);
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
                    case WindowMode.DEBUG:
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
                                    , FONT_TYPEFACE
                                    , 30
                                    , Brushes.Yellow
                                    , 12.5), new Point(SystemParameters.FullPrimaryScreenWidth / 2 - 30 * 7, modeSelectionTextRect.Y - 40));
        }

        private void DrawGameWindow(DrawingContext drawingContext)
        {

            drawingContext.DrawRectangle(Brushes.Black, unmoveableAreaPen, unmoveableAreaRect);
            unmoveableAreaRect.X = moveableRightSidePosition;
            drawingContext.DrawRectangle(Brushes.Black, unmoveableAreaPen, unmoveableAreaRect);
            unmoveableAreaRect.X = 0;

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
                                    , FONT_TYPEFACE
                                    , 20
                                    , Brushes.White
                                    , 12.5), scorePoint);

            drawingContext.DrawText(new FormattedText($"EXP:{player.Exp}\nLV_:{player.Level}\nWEAPON:{player.Weapon}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , FONT_TYPEFACE
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
                                    $"stagePosition = {stagePosition}\n" +
                                    $"bullets.Count  = {bullets.Count}\n" +
                                    $"enemies.Count  = {enemies.Count}\n" +
                                    $"items.Count    = {items.Count}\n" +
                                    $"program uptime = {(DateTime.Now - GAME_START_TIME).TotalSeconds}\n" +
                                    $"fps = {1.0 / spf.TotalSeconds}\n" +
                                    $"Speed = {player.Speed}\n" +
                                    $"BGM Muted = {musicPlayer.IsMuted}\n" +
                                    $"BGM Seconds = {musicPlayer.Position.Minutes}:{musicPlayer.Position.Seconds} / {musicPlayer.NaturalDuration.TimeSpan.Minutes}:{musicPlayer.NaturalDuration.TimeSpan.Seconds}\n" +
                                    $"cooltime = {player.BulletCoolTime},decreace = {player.DecreaceBulletCoolTime}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , FONT_TYPEFACE
                                    , 12
                                    , Brushes.White
                                    , 12.5), new Point(10, 30));
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
                case StatusEffects.DESTROY_MODE:
                    return Images.DESTROY_ITEM_IMAGE;
                case StatusEffects.SCORE_BOOST:
                    return Images.SCORE_BOOST_ICON_IMAGE;
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
                    updateTimer.Interval = TimeSpan.FromMilliseconds(100);
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
                    updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
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