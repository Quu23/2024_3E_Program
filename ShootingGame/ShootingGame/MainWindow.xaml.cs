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
using System.IO;
using System.Text;
using System.Diagnostics;

using SharpDX.DirectInput;
using ShootingGame.Entities.Planes.Enemies.Boss;


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

        //private SortedList<int, string> ranking;
        private List<(int, string)> ranking;
        /// <summary>
        /// pos,enemy or item,type,x
        /// </summary>
        private List<(int, int, int, int)> stageData;

        public static int stagePosition;
        public static int stageLastPosition = 0;

        public static int score = 0;

        const int FPS = 60;

        DispatcherTimer updateTimer;
        private VisualCollection visuals;


        /// <summary>
        ///                                     W      A      S      D    Space    Enter  Tab   R_SHIFT
        /// </summary>
        public static bool[] isKeyPresseds = { false, false, false, false, false, false, false, false };

        private Joystick _joy;

        public Player player;

        public List<Enemy> enemies = new List<Enemy>();

        public List<Bullet> bullets = new List<Bullet>();

        public List<Item> items = new List<Item>();

        private MediaPlayer musicPlayer;

        //背景のアニメーション関係
        int backgroundAnimationCounter = 0;
        private BitmapImage backgroundImage;
        private Rect backgroundRect;

        //タイトルなどのロゴ関係
        private Rect titleRect;
        private Rect modeSelectionTextRect;
        //GAMEOVERもしくはGAMECLEARになったときにインスタンスを代入する。
        private FormattedText scoreText = null;

        private Rect hpBarRect;
        private Rect bossHpBarRect;

        private readonly Point statusPoint;
        private readonly Point scorePoint;

        private Rect statusIconRect;

        private Rect weaponIconRect;

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
                bossHpBarRect = new Rect(moveableLeftSidePosition, 0, moveableRightSidePosition - moveableLeftSidePosition, 15);
            };
            unmoveableAreaPen = new Pen(Brushes.Yellow, 1);


            windowMode = WindowMode.START;

            player = new Player("unknown");
            Debug.WriteLine(player.X);

            //タイマーの設定 
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromMilliseconds(1000 / FPS);
            updateTimer.Tick += Timer_Tick;

            //キーコンフィグ
            KeyUp += DepressedKey;
            KeyDown += PressedKey;
            KeyDown += MessageInputKey;

            // 入力周りの初期化
            DirectInput dinput = new DirectInput();

            // 使用するゲームパッドのID
            var joystickGuid = Guid.Empty;

            // ジョイスティックからゲームパッドを取得する
            if (joystickGuid == Guid.Empty)
            {
                foreach (DeviceInstance device in dinput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = device.InstanceGuid;
                    break;
                }
            }
            // 見つかった場合
            if (joystickGuid != Guid.Empty)
            {
                // パッド入力周りの初期化
                _joy = new Joystick(dinput, joystickGuid);
                if (_joy != null)
                {
                    // バッファサイズを指定
                    _joy.Properties.BufferSize = 128;

                    // 相対軸・絶対軸の最小値と最大値を
                    // 指定した値の範囲に設定する
                    foreach (DeviceObjectInstance deviceObject in _joy.GetObjects())
                    {
                        switch (deviceObject.ObjectId.Flags)
                        {
                            case DeviceObjectTypeFlags.Axis:
                            // 絶対軸or相対軸
                            case DeviceObjectTypeFlags.AbsoluteAxis:
                            // 絶対軸
                            case DeviceObjectTypeFlags.RelativeAxis:
                                // 相対軸
                                var ir = _joy.GetObjectPropertiesById(deviceObject.ObjectId);
                                if (ir != null)
                                {
                                    try
                                    {
                                        ir.Range = new InputRange(-1000, 1000);
                                    }
                                    catch (Exception) { }
                                }
                                break;
                        }
                    }
                }
            }

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
            backgroundImage = Images.START_BACKGROUND_IMAGE;
            backgroundRect = new Rect(0, 0, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

            //タイトルロゴ等の設定
            titleRect = new Rect((SystemParameters.PrimaryScreenWidth - Images.TITLE_IMAGE.Width) / 2, (SystemParameters.PrimaryScreenHeight - Images.MODE_SELECT_TEXT_IMAGE.Height) / 8, 700, 100);//割る4ぐらいがちょうどいい（適当）
            modeSelectionTextRect = new Rect((SystemParameters.PrimaryScreenWidth - Images.MODE_SELECT_TEXT_IMAGE.Width) / 2, (SystemParameters.PrimaryScreenHeight - Images.MODE_SELECT_TEXT_IMAGE.Height) * 6 / 8, 700, 300);

            hpBarRect = new Rect(1600 / 1934.0 * SystemParameters.PrimaryScreenWidth, 1030 / 1094.0 * SystemParameters.PrimaryScreenHeight, player.GetMaxHp * 5, 10);

            scorePoint  = new Point(30, 0);
            statusPoint = new Point(hpBarRect.X - 80, hpBarRect.Y - 10);

            statusIconRect = new Rect(SystemParameters.PrimaryScreenWidth - 64  , /*起点Y=*/SystemParameters.PrimaryScreenHeight - 64 * (player.status.Count + 1), 64, 64);

            weaponIconRect = new Rect(0,0,32,32);

            FONT_TYPEFACE = new Typeface((FontFamily)Application.Current.Resources["dotfont"], FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            //データ読み込み

            //SortedDictonaryは小さい順に並べるので、SCOREが大きいほど前（順序でいえば小さい）になる。
            ranking = new List<(int, string)>();
            LoadRankingData();
            ranking.Sort(Comparer<(int, string)>.Create(((int, string) x, (int, string) y) =>
            {
                if (x.Item1 > y.Item1) return -1;
                if (x.Item1 < y.Item1) return 1;
                return 0;
            }));

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

                ranking.Add((temp2,temp[1]));
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
           // path = @"../../../data/stages/stageForDebug.txt";

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
                    if (i == 3)
                    {
                        temp2 = (int)((SystemParameters.FullPrimaryScreenWidth /1600) * temp2);
                    }
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
            using(StreamWriter file = new StreamWriter(path, true, Encoding.GetEncoding("UTF-8")))
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

            UpdateForPad();

            if (windowMode != WindowMode.START)
            {
                backgroundAnimationCounter += 5;
                if (backgroundAnimationCounter > SystemParameters.PrimaryScreenHeight) backgroundAnimationCounter = 0;
            }

            switch (windowMode)
            {
                case WindowMode.START:
                    StartLoop();
                    break;
                case WindowMode.STAGE1:
                case WindowMode.STAGE1_BOSS:
                case WindowMode.STAGE2:
                case WindowMode.STAGE2_BOSS:
                case WindowMode.STAGE3:
                case WindowMode.STAGE3_BOSS:
                    GameLoop();
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
        }

        private void StartLoop()
        {
            if (isKeyPresseds[4]) {
                if (message.Equals("DEBUG"))
                {
                    windowMode = WindowMode.DEBUG;
                    player = new Player("DebugMode");
                    player.LevelUp(20);
                    
                    return;
                }

                windowMode = WindowMode.STAGE1;
                if (message.Length > 0)
                {
                    player = new Player(message);
                    message = "";
                }

                stageLastPosition = stageData[0].Item1;

                backgroundImage = Images.BACKGROUND1_IMAGE;
            }
        }

        private void DebugModeLoop()
        {
            if (enemies.Count <= 0)
            {
                //enemies.Add(new Boss3());
                //Boss b = (Boss)enemies[0];
                //enemies.AddRange(b.GetFollowers());

                enemies.Add(new StraightEnemy(player.X, 0,1));

            }

            if (items.Count <= 0)
            {
                items.Add(new ScoreBoosterItem(player.X, 0));
            }

            BasicGameLogic();
        }

        private void GameoverLoop()
        {
            if (isKeyPresseds[5])
            {
                windowMode = WindowMode.START;
                backgroundImage = Images.START_BACKGROUND_IMAGE;

                ranking = new List<(int, string)>();
                LoadRankingData();
                ranking.Sort(Comparer<(int, string)>.Create(((int, string) x, (int, string) y) =>
                {
                    if (x.Item1 > y.Item1) return -1;
                    if (x.Item1 < y.Item1) return 1;
                    return 0;
                }));
                stageData.Clear();
                LoadStageData(WindowMode.STAGE1);

                player = new Player("unknown");

                score = 0;
                stagePosition = 0;

                enemies.Clear();
                items.Clear();
                bullets.Clear();

                musicPlayer.Open(UtilityUris.BGM2_URI);

                musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
                {
                    musicPlayer.Position = TimeSpan.Zero;
                };
                musicPlayer.IsMuted = false;
                musicPlayer.Play();

                backgroundAnimationCounter = 0;
            }
        }

        private void GameclearLoop()
        {
            if (isKeyPresseds[5])
            {
                windowMode = WindowMode.START;
                backgroundImage = Images.START_BACKGROUND_IMAGE;

                ranking = new List<(int, string)>();
                LoadRankingData();
                ranking.Sort(Comparer<(int, string)>.Create(((int, string) x, (int, string) y) =>
                {
                    if (x.Item1 > y.Item1) return -1;
                    if (x.Item1 < y.Item1) return 1;
                    return 0;
                }));

                stageData.Clear();
                LoadStageData(WindowMode.STAGE1);

                player = new Player("unknown");

                score = 0;
                stagePosition = 0;

                enemies.Clear();
                items.Clear();
                bullets.Clear();

                musicPlayer.Open(UtilityUris.BGM2_URI);

                musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
                {
                    musicPlayer.Position = TimeSpan.Zero;
                };
                musicPlayer.IsMuted = false;
                musicPlayer.Play();

                backgroundAnimationCounter = 0;
            }
        }

        // TODO:ゲームループの実装
        private void GameLoop()
        {

            if (windowMode > 0)
            {
                for (int i = stageData.Count - 1; i > 0; i--)
                {
                    var pair = stageData[i];

                    if (stagePosition < pair.Item1) break;

                    if (pair.Item1 == -1) continue;

                    if (pair.Item2 == 0)
                    {
                        enemies.Add(UtilityGenerater.GenerateEnemy((EnemyTypes)pair.Item3, pair.Item4, 0, Math.Abs((int)windowMode)));
                    }
                    else
                    {
                        items.Add(UtilityGenerater.GenerateItem((ItemTypes)pair.Item3, pair.Item4, 0));
                    }
                    stageData.Remove(pair);
                }
            }


            BasicGameLogic();    
        }

        private void BasicGameLogic()
        {
            if (windowMode > 0) stagePosition++;

            if (stagePosition > stageLastPosition + 200 && enemies.Count == 0)
            {
                windowMode = (WindowMode)(-1 * (int)windowMode);

                switch (windowMode)
                {
                    case WindowMode.STAGE1_BOSS:
                        enemies.Add(new Boss1());
                        musicPlayer.Open(UtilityUris.BOSS1_BGM_URI);
                        break;
                    case WindowMode.STAGE2_BOSS:
                        enemies.Add(new Boss2());
                        musicPlayer.Open(UtilityUris.BOSS2_BGM_URI);
                        break;
                    case WindowMode.STAGE3_BOSS:
                        enemies.Add(new Boss3());
                        musicPlayer.Open(UtilityUris.BOSS3_BGM_URI);
                        break;
                }

                musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
                {
                    musicPlayer.Position = TimeSpan.Zero;
                };
                musicPlayer.IsMuted = false;
                musicPlayer.Play();

                Boss b = (Boss)enemies[0];
                enemies.AddRange(b.GetFollowers());

                stagePosition = 0;
            }

            // BOSSのとき
            if (windowMode < 0)
            {
                if (enemies.Count == 0)
                {
                    windowMode = (WindowMode)(-1 * (int)windowMode + 1);
                    if (windowMode <= WindowMode.STAGE3)
                    {
                        stageData.Clear();
                        LoadStageData(windowMode);
                        stageLastPosition = stageData[0].Item1;

                        musicPlayer.Open(UtilityUris.BGM1_URI);
                        musicPlayer.MediaEnded += (object? sender, EventArgs e) =>
                        {
                            musicPlayer.Position = TimeSpan.Zero;
                        };
                        musicPlayer.IsMuted = false;
                        musicPlayer.Play();

                        switch (windowMode) 
                        {
                            case WindowMode.STAGE2:
                                backgroundImage = Images.BACKGROUND2_IMAGE;
                                break;
                            case WindowMode.STAGE3:
                                backgroundImage = Images.BACKGROUND3_IMAGE;
                                break;
                        }

                    }
                    else if(windowMode == WindowMode.GAMECLEAR)
                    {
                        scoreText = new FormattedText($"SCORE:{score}"
                                                    , CultureInfo.GetCultureInfo("en")
                                                    , FlowDirection.LeftToRight
                                                    , FONT_TYPEFACE
                                                    , 100
                                                    , Brushes.Yellow
                                                    , 12.5);

                        WriteScore();
                    }

                    return;
                }
            }

            //とりあえず必要経験経験値=現在のレベル^3 + 5　にしている。
            if (player.Exp >= player.Level * player.Level * player.Level + 5 ||/*デバック用*/ isKeyPresseds[5] && isKeyPresseds[6]) player.LevelUp();

            //playerが死んだら即終了。ゲームオーバー画面作るならここを修正。
            if (player.Hp <= 0)
            {
                windowMode = WindowMode.GAMEOVER;
                scoreText = new FormattedText($"SCORE:{score}"
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
                    UtilitySE.PlayPlayerHitSE();

                    player.Hp -= tmp_bullet.Damage;
                    bullets.Remove(tmp_bullet);
                    continue;
                }

                for (int ei = enemies.Count; ei > 0; ei--)
                {
                    Enemy tmp_enemy = enemies[ei - 1];
                    if (tmp_bullet.Type == EnemyTypes.PLAYER && tmp_enemy.IsHit(tmp_bullet))
                    { 
                        UtilitySE.PlayEnemyHitSE();
                        tmp_enemy.Hp -= tmp_bullet.Damage;

                        bullets.Remove(tmp_bullet);

                        if (tmp_enemy.Hp <= 0)
                        {
                            if (tmp_enemy is Boss)
                            {
                                UtilitySE.PlayBossDeadSE();
                            }
                            else
                            {
                                UtilitySE.PlayEnemyDeadSE();
                            }

                            if (tmp_enemy is Follower)
                            {
                                Follower follower = (Follower) tmp_enemy;
                                enemies[0].Hp -= follower.MAX_HP;
                                Boss b = (Boss)enemies[0];
                                b.followers.Remove(follower);
                            }
                            tmp_enemy.DeadAction(player, enemies, items);
                        }
                    }
                }
            }

            for (int ei = enemies.Count; ei > 0; ei--)
            {
                Enemy tmp_enemy = enemies[ei - 1];

                tmp_enemy.Action();

                if (tmp_enemy.IsHit(player))
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
                    if (tmp_item is ExpOrb) 
                    {
                        UtilitySE.PlayGetOrbSE();
                    }
                    else
                    {
                        UtilitySE.PlayGetItemSE();
                    }

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
                    case WindowMode.STAGE2:
                    case WindowMode.STAGE2_BOSS:
                    case WindowMode.STAGE3:
                    case WindowMode.STAGE3_BOSS:
                        DrawGameWindow(drawingContext);
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
            if (isKeyPresseds[7])
            {
                string topranking = "";
                string scores;
                string playername;
                int i = 1;
                foreach (var rankingData in ranking)
                {
                    scores = rankingData.Item1.ToString();
                    playername = rankingData.Item2;
                    if (i > 9)
                    {
                        topranking += $"{i}.{playername.PadRight(10)} , {scores.PadLeft(8)}\n";
                    }
                    else
                    {
                        topranking += $"{0}{i}.{playername.PadRight(10)} , {scores.PadLeft(8)}\n";
                    }
                    //topranking += $"{i + 1}.{playername} , {scores}\n";

                    
                    i += 1;
                    if (i > 10) break;
                }
                //背景の描画
                drawingContext.DrawRectangle(Brushes.Black, new Pen(Brushes.Yellow, 5), new Rect(new Point(SystemParameters.FullPrimaryScreenWidth / 4 - 90 , SystemParameters.FullPrimaryScreenHeight / 4 -45), new Size(SystemParameters.FullPrimaryScreenWidth / 2 + 150, SystemParameters.FullPrimaryScreenHeight / 2 + 100)));

                //ランキングを描画する
                drawingContext.DrawText(new FormattedText(
                                         topranking
                                        , CultureInfo.GetCultureInfo("en")
                                        , FlowDirection.LeftToRight
                                        , FONT_TYPEFACE
                                        , 30
                                        , Brushes.Yellow
                                        , 12.5), new Point(SystemParameters.FullPrimaryScreenWidth / 2 - topranking.Length * 1.5, modeSelectionTextRect.Y - 150));
            }
            else if (isKeyPresseds[2])
            {
                drawingContext.DrawImage(Images.STUFF_CREDIT_IMAGE, new Rect(0,0,Width , Height));
            }
            else
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

            drawingContext.DrawText(new FormattedText($"EXP:{player.Exp}\nLV_:{player.Level}\n{player.name}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , FONT_TYPEFACE
                                    , 10
                                    , Brushes.White
                                    , 12.5), statusPoint);
            drawingContext.DrawImage(Images.EXP_ORB_IMAGE, new Rect(10,Height - 80,16,16));
            drawingContext.DrawText(new FormattedText($"  ×{player.orbCount}\n\n{windowMode}"
                                    , CultureInfo.GetCultureInfo("en")
                                    , FlowDirection.LeftToRight
                                    , FONT_TYPEFACE
                                    , 16
                                    , Brushes.White
                                    , 12.5), new Point(0,Height - 80));

            hpBarRect.Width = player.GetMaxHp * 10;
            drawingContext.DrawRectangle(Brushes.White, null, hpBarRect);
            hpBarRect.Width = player.Hp >= 0 ? player.Hp * 10 : 0;
            drawingContext.DrawRectangle(Brushes.Red, null, hpBarRect);

            if (enemies.Count > 0 && enemies[0] is Boss)
            {
                Boss b = (Boss) enemies[0];
                bossHpBarRect.X = moveableLeftSidePosition;
                bossHpBarRect.Width = (moveableRightSidePosition-moveableLeftSidePosition);
                drawingContext.DrawRectangle(Brushes.White, null, bossHpBarRect);
                bossHpBarRect.Width = b.Hp >= 0 ? b.Hp * (moveableRightSidePosition - moveableLeftSidePosition) / b.MAX_HP : 0;
                drawingContext.DrawRectangle(Brushes.Red, null, bossHpBarRect);

                //drawingContext.DrawEllipse(Brushes.White, null, new Point(b.CenterX, b.CenterY), 10, 10);
            }

            foreach (var kvp in player.status)
            {
                if (kvp.Value > 0)
                {
                    drawingContext.DrawImage(GetStatusIconImage(kvp.Key), statusIconRect);
                }
                statusIconRect.Y += statusIconRect.Height;
            }
            statusIconRect.Y -= statusIconRect.Height * player.status.Count ;


            BitmapImage[] weaponIconsImage = {Images.DEFAULT_ICON_IMAGE,Images.BOUND_ICON_IMAGE, Images.SHOTGUN_ICON_IMAGE, Images.HOMING_ICON_IMAGE };
            BitmapSource[] weaponIconSource = new BitmapSource[weaponIconsImage.Length];

            for (int i = 0; i < weaponIconsImage.Length; i++)
            {
                weaponIconSource[i] = i == player.Weapon ? AdjustImageOpacity(weaponIconsImage[i], 1.00) : AdjustImageOpacity(weaponIconsImage[i], 0.50);
            }

            weaponIconRect.X = moveableRightSidePosition + moveableLeftSidePosition / 4;
            weaponIconRect.Y = hpBarRect.Y - 60;

            drawingContext.DrawImage(weaponIconSource[0], weaponIconRect);

            weaponIconRect.X += 40;

            drawingContext.DrawImage(weaponIconSource[1], weaponIconRect);
            if(Math.Abs((int)windowMode) < (int)WindowMode.STAGE2)drawingContext.DrawImage(Images.PROHIBITED_ICON_IMAGE, weaponIconRect);

            weaponIconRect.Y -= 40;

            drawingContext.DrawImage(weaponIconSource[2], weaponIconRect);
            if (Math.Abs((int)windowMode) < (int)WindowMode.STAGE3) drawingContext.DrawImage(Images.PROHIBITED_ICON_IMAGE, weaponIconRect);

            weaponIconRect.X -= 40;

            drawingContext.DrawImage(weaponIconSource[3], weaponIconRect);
            if (player.orbCount < 5)drawingContext.DrawImage(Images.PROHIBITED_ICON_IMAGE, weaponIconRect);




            if (isKeyPresseds[6])
            {
                //hack:毎回newするのは効率悪いからDrawText()以外のいい方法が欲しい。

                string status = $"Width = {Width} / Height = {Height}\n" +
                                $"backgroundAnimationCounter={backgroundAnimationCounter}\n" +
                                $"Pos = {stagePosition} / lastPos = {stageLastPosition}\n" +
                                $"bullets.Count  = {bullets.Count}\n" +
                                $"enemies.Count  = {enemies.Count}\n" +
                                $"items.Count    = {items.Count}\n" +
                                $"program uptime = {(DateTime.Now - GAME_START_TIME).TotalSeconds}\n" +
                                $"fps = {1.0 / spf.TotalSeconds}\n" +
                                $"Speed = {player.Speed}\n" +
                                $"cooltime = {player.BulletCoolTime} / decreace = {player.DecreaceBulletCoolTime}\n" +
                                $"BGM Muted = {musicPlayer.IsMuted}\n";
                               

                if (musicPlayer.NaturalDuration.HasTimeSpan)
                {
                    status += $"BGM Seconds = {musicPlayer.Position.Minutes}:{musicPlayer.Position.Seconds} / {musicPlayer.NaturalDuration.TimeSpan.Minutes}:{musicPlayer.NaturalDuration.TimeSpan.Seconds:00}\n";
                }
                else
                {
                    status += "NaN\n";
                }


                drawingContext.DrawText(new FormattedText(status
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
            double leftWidth = (Width - scoreText.Text.Length * 100) / 2;
            drawingContext.DrawText(scoreText, new Point(leftWidth, modeSelectionTextRect.Y));
        }

        private void DrawGameclearWindow(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(Images.GAMECLEAR_IMAGE, titleRect);
            double leftWidth = (Width - scoreText.Text.Length * 100) / 2;
            drawingContext.DrawText(scoreText, new Point(leftWidth, modeSelectionTextRect.Y));
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
            if (target is Boss)
            {
                Boss targetBoss = (Boss)target;
                dc.DrawEllipse(colorBrush, new Pen(Brushes.DarkGreen, 1), new Point(targetBoss.CenterX, targetBoss.CenterY), targetBoss.Width/2, targetBoss.Height/2);
            }
            else
            {
                dc.DrawEllipse(colorBrush, new Pen(Brushes.DarkGreen, 1), new Point(target.CenterX, target.CenterY), target.Radius, target.Radius);
            }      
        }

        // BitmapImageの透明度を変更する。ピクセルデータを直接いじる。
        public BitmapSource AdjustImageOpacity(BitmapImage image, double opacity)
        {
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            int stride = width * ((image.Format.BitsPerPixel + 7) / 8);
            byte[] pixelData = new byte[height * stride];
            image.CopyPixels(pixelData, stride, 0);

            for (int i = 0; i < pixelData.Length; i += 4)
            {
                pixelData[i + 3] = (byte)(pixelData[i + 3] * opacity); // アルファチャネルを調整
            }

            WriteableBitmap wb = new WriteableBitmap(width, height, image.DpiX, image.DpiY, image.Format, null);
            wb.WritePixels(new Int32Rect(0, 0, width, height), pixelData, stride, 0);
            return wb;
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
                    return Images.DESTOROY_ICON_IMAGE;
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
                case System.Windows.Input.Key.W:
                    isKeyPresseds[0] = true;
                    break;
                case System.Windows.Input.Key.A:
                    isKeyPresseds[1] = true;
                    break;
                case System.Windows.Input.Key.S:
                    isKeyPresseds[2] = true;
                    break;
                case System.Windows.Input.Key.D:
                    isKeyPresseds[3] = true;
                    break;
                case System.Windows.Input.Key.Space:
                    isKeyPresseds[4] = true;
                    break;
                case System.Windows.Input.Key.Escape:
                    Environment.Exit(0);
                    break;
                case System.Windows.Input.Key.Enter:
                    isKeyPresseds[5] = true;
                    updateTimer.Interval = TimeSpan.FromMilliseconds(100);
                    break;

                //デバック用
                case System.Windows.Input.Key.L:
                    player.Speed++;
                    break;
                case System.Windows.Input.Key.Tab:
                    isKeyPresseds[6] = true;
                    break;
                default:
                    break;
                case System.Windows.Input.Key.RightShift:
                    isKeyPresseds[7] = true;
                    break;
            }
        }
        private void DepressedKey(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.W:
                    isKeyPresseds[0] = false;
                    break;
                case System.Windows.Input.Key.A:
                    isKeyPresseds[1] = false;
                    break;
                case System.Windows.Input.Key.S:
                    isKeyPresseds[2] = false;
                    break;
                case System.Windows.Input.Key.D:
                    isKeyPresseds[3] = false;
                    break;
                case System.Windows.Input.Key.Space:
                    isKeyPresseds[4] = false;
                    break;
                case System.Windows.Input.Key.Enter:
                    isKeyPresseds[5] = false;
                    updateTimer.Interval = TimeSpan.FromMilliseconds(1000/FPS);
                    break;

                case System.Windows.Input.Key.Tab:
                    isKeyPresseds[6] = false;
                    break;
                case System.Windows.Input.Key.RightShift:
                    isKeyPresseds[7] = false;
                    break;
                default:
                    break;
            }
        }

        private void MessageInputKey(object? sender, KeyEventArgs e)
        {
            if (windowMode != WindowMode.START) return;

            if (e.Key == System.Windows.Input.Key.Back && message.Length > 0)message = message.Remove(message.Length - 1, 1);
            
            // 名前の文字数制限は8文字
            if (message.Length > 8) return;

            if (e.Key >= System.Windows.Input.Key.D0 && e.Key <= System.Windows.Input.Key.D9) message　+= $"{e.Key - System.Windows.Input.Key.D0}";

            if (e.Key >= System.Windows.Input.Key.A  && e.Key <= System.Windows.Input.Key.Z) message += e.Key.ToString();



        }

        public void UpdateForPad()
        {
            // 初期化が出来ていない場合、処理終了
            if (_joy == null) { return; }

            // キャプチャするデバイスを取得
            _joy.Acquire();
            _joy.Poll();

            // ゲームパッドのデータ取得
            var jState = _joy.GetCurrentState();
            // 取得できない場合、処理終了
            if (jState == null) { return; }

            // 以下の処理は挙動確認用

            // アナログスティックの左右軸
            bool inputX = true;
            if (jState.X > 300)
            {
                isKeyPresseds[3] = true;
            }
            else if (jState.X < -300)
            {
                isKeyPresseds[1] = true;
            }
            else
            {
                inputX = false;
            }

            // アナログスティックの上下軸
            bool inputY = true;
            if (jState.Y > 300)
            {
                isKeyPresseds[2] = true;
            }
            else if (jState.Y < -300)
            {
                isKeyPresseds[0] = true;
            }
            else
            {
                inputY = false;
            }
            // 未入力だった場合
            if (!inputX)
            {
                isKeyPresseds[1] = false;
                isKeyPresseds[3] = false;
            }
            
            if (!inputY)
            {
                isKeyPresseds[0] = false;
                isKeyPresseds[2] = false;
            }

            isKeyPresseds[4] = jState.Buttons[1]; //Space
            isKeyPresseds[7] = jState.Buttons[2]; //R_Shift

            if (jState.Buttons[3]) player.Weapon = 0;
            if (jState.Buttons[4] && Math.Abs((int) windowMode) >= (int) WindowMode.STAGE2) player.Weapon = 1; 
            if (jState.Buttons[5] && Math.Abs((int) windowMode) >= (int) WindowMode.STAGE3) player.Weapon = 2;
            if (jState.Buttons[6] && player.orbCount >= 5) player.Weapon = 3;

        }

    }
}