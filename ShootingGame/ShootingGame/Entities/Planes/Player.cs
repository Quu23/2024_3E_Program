using ShootingGame.Entities.Items;
using ShootingGame.Entities.Planes.Enemies;
using ShootingGame.Entities.Planes.Enemies.Boss;
using System.Windows;
using static ShootingGame.Entities.Items.StatusEffects;

namespace ShootingGame.Entities.Planes
{
    public class Player : Plane
    {
        public readonly string name;

        public int MAX_HP;
        private int exp;
        public int increaseRateOfScore;

        /// <summary>
        /// 状態異常やダッシュ状態を抜きにした（つまり平常状態の）ステータス <br/>
        /// r , defaultSpeed , maxhp , decreaceBulletCool , increaceRateOfScore
        /// </summary>
        private int[] normalStatus;

        public Dictionary<StatusEffects, int> status;

        public int defaultSpeed;

        private int weapon;

        public int orbCount;

        public Player(string name) : base(/*x=*/700, /*y=*/500, /*r=*/20, /*speed=*/5, Images.PLAYER_IMAGE, /*LV=*/1, /*hp=*/5, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 60)
        {
            //最初は5にする？
            MAX_HP = 5;

            this.name = name;

            defaultSpeed = Speed;

            increaseRateOfScore = 100;

            orbCount = 0;

            Weapon = 0;

            status = new Dictionary<StatusEffects, int>() { 
                // 効果　　　　　　         効果時間
                { SPEED_UP                ,0},
                { SPEED_DOWN              ,0},
                { SHOT_RATE_UP            ,0},
                { SHOT_RATE_DOWN          ,0},
                { SCORE_BOOST             ,0},
                { INVINCIBLE              ,0},
                { DESTROY_MODE            ,0},
            };

            normalStatus = [
                Radius, defaultSpeed, MAX_HP, DecreaceBulletCoolTime, 100,
            ];

        }

        public override int Hp 
        { 
            get => base.Hp; 
            set
            { 
                if (status == null)
                {
                    base.Hp = value;
                    return;
                }
                if (status[INVINCIBLE] <= 0)
                {
                    base.Hp = value;
                }
            } 
        }

        public int Exp { get => exp; set => exp = value; }
        public double GetMaxHp { get => MAX_HP; }

        /// <summary>
        /// 現在の武器を示す。
        /// [0] normal （弾のサイズも普通）
        /// [1] shotgun（弾のサイズは小,弾の数はLV依存）
        /// [2] bound  （弾のサイズは大,弾の数は2つ,壁を反射する）
        /// [3] homing （弾のサイズは普通,追尾する）
        /// </summary>
        public int Weapon { get => weapon; set => weapon = value % 4;}

        //TODO:レベルアップした時の処理を考える
        public void LevelUp()
        {
            Level++;
            MAX_HP += 1;
            Hp += 1;

            if (Level % 5 == 0)
            {
                normalStatus[3]++;

                if (status[SHOT_RATE_DOWN] == 0 && status[SHOT_RATE_UP] == 0)
                {
                    DecreaceBulletCoolTime = normalStatus[3];
                }
            }
        }

        public void LevelUp(int count)
        {
            if (count == 0) return;
            LevelUp();
            LevelUp(count-1);
        }

        public void HeelFullOfHp()
        {
            Hp = MAX_HP;
        }

        public override bool IsHit(Entity target)
        {
            if (base.IsHit(target)) return true;
            return false;
        }

        public override void Action()
        {
            if (BulletCoolTime > 0) BulletCoolTime -= DecreaceBulletCoolTime;

            if (MainWindow.isKeyPresseds[5]) RotateWeapon();

            foreach (var kvp in status)
            {
                if (kvp.Value > 0)
                {
                    status[kvp.Key]--;
                    if (status[kvp.Key] <= 0) ResetStatus(kvp.Key);
                }
            }

            if (MainWindow.isKeyPresseds[7])
            {
                if (Speed != defaultSpeed * 3)
                {
                    Speed *= 3;
                }
            }
            else
            {
                Speed = defaultSpeed;
            }

            Move();
            ChangeRect(X, Y);
            if (BulletCoolTime <= 0 && MainWindow.isKeyPresseds[4])
            {
                App.window.bullets.AddRange(ShotBullet());
                BulletCoolTime = MaxBulletCoolTime;
            }
        }

        /// <summary>
        /// 引数で指定した状態異常に関するステータスをリセットする。
        /// </summary>
        /// <param name="key">状態異常の種類。全部リセットするならNORMALを指定する。</param>
        private void ResetStatus(StatusEffects key)
        {
            switch (key)
            {
                case NORMALE:
                    Radius = normalStatus[0];
                    defaultSpeed = normalStatus[1];
                    MAX_HP = normalStatus[2];
                    DecreaceBulletCoolTime = normalStatus[3];
                    break;
                case SPEED_UP:
                case SPEED_DOWN:
                    defaultSpeed = normalStatus[1];
                    Speed = defaultSpeed;
                    break;
                case SHOT_RATE_UP:
                case SHOT_RATE_DOWN:
                    DecreaceBulletCoolTime = normalStatus[3];
                    break;
                case INVINCIBLE:
                    //無敵になったからステータスが上がってるとかはないからなんも書かん。
                    ;
                    break;
                default:
                    break;
            }
        }

        private void RotateWeapon()
        {
            Weapon++;


            if (Weapon > Math.Abs((int)MainWindow.windowMode) - 1)
            {
                weapon = 0;

                if(orbCount >= 10)
                {
                    weapon = 3 ;
                }
            }


            switch (weapon)
            {
                case 0:
                    bulletRadius = Bullet.RADIUS_FOR_MEDIUM; break;
                case 1:
                    bulletRadius = Bullet.RADIUS_FOR_BIG; break;
                case 2:
                    bulletRadius = Bullet.RADIUS_FOR_SMALL; break;
                case 3:
                    bulletRadius = Bullet.RADIUS_FOR_MEDIUM;break;
            }
        }

        protected override void Move()
        {
            if (X > App.window.moveableLeftSidePosition && MainWindow.isKeyPresseds[1])
            {
                X -= Speed;
            }
            if (X < App.window.moveableLeftSidePosition) X = App.window.moveableLeftSidePosition;

            // xはエンティティの左上の座標だから、右に行くときは「x+幅」、つまりエンティティの右端が画面の端かどうかで判断。
            if (X + Radius * 2 < App.window.moveableRightSidePosition && MainWindow.isKeyPresseds[3])
            {
                X += Speed;
            }
            if (X + Radius * 2 > App.window.moveableRightSidePosition) X = (int)(App.window.moveableRightSidePosition - Radius * 2);

            if (Y > 0 && MainWindow.isKeyPresseds[0])
            {
                Y -= Speed;
            }
            if (Y < 0) Y = 0;

            if (Y + Radius * 2 < SystemParameters.PrimaryScreenHeight && MainWindow.isKeyPresseds[2])
            {
                Y += Speed;
            }
            if (Y + Radius * 2 > SystemParameters.PrimaryScreenHeight) Y = (int)(SystemParameters.PrimaryScreenHeight - Radius * 2);
        }


        // TODO:levelに応じた弾の出し方・攻撃力の設定
        // TODO:弾の画像設定
        protected override List<Bullet> ShotBullet()
        {
            UtilitySE.PlayShotSE();

            //弾の追加を行うかもしれないからListはこの書き方のままでいい。
            List<Bullet> bullets = new List<Bullet>();
            if (status[DESTROY_MODE] > 0)
            {
                Bullet destroyBullet = new Bullet(CenterX - 100, Y - 50, 100, 4, 0, 9999, Enemies.EnemyTypes.PLAYER); 
                destroyBullet.Img = Images.PLAYER_BULLET_DESTROY_IMAGE;  
                bullets.Add(destroyBullet);
            }
            else
            {
                SetBullets(bullets);
            }   
            return bullets;
        }

        private void SetBullets(List<Bullet> bullets)
        {
            if (Weapon == 0)
            {
                bullets.Add(new Bullet(CenterXForShotBullet, Y, bulletRadius, Speed + 5, 0, 4, Enemies.EnemyTypes.PLAYER));
                return;
            }


            //bound
            if (Weapon == 1)
            {
                //bullets.Add(new BoundBullet(CenterXForShotBullet - Radius, Y, Speed + 10, BoundBullet.LEFT_DEGREE , Level));
                //bullets.Add(new BoundBullet(CenterXForShotBullet + Radius, Y, Speed + 10, BoundBullet.RIGHT_DEGREE, Level));
                bullets.Add(new BoundBullet(CenterXForShotBullet - Radius, Y, Speed + 10, BoundBullet.WIDE_RIGHT_DEGREE, 2));
                bullets.Add(new BoundBullet(CenterXForShotBullet + Radius, Y, Speed + 10, BoundBullet.WIDE_LEFT_DEGREE, 2));
                return;
            }

            //shotgun
            if (Weapon == 2)
            {
                bullets.Add(new Bullet(CenterXForShotBullet, Y, bulletRadius, Speed + 5, 0, 2, Enemies.EnemyTypes.PLAYER));

                for (int i = 1; i < 100; i += 5)
                {
                    if (Level >= i)
                    {
                        Bullet leftBullet  = new Bullet(CenterX - Radius, Y, bulletRadius, Speed + 5, -30 * (i / 5 + 1), 2, Enemies.EnemyTypes.PLAYER);
                        Bullet rightBullet = new Bullet(CenterX + Radius, Y, bulletRadius, Speed + 5,  30 * (i / 5 + 1), 2, Enemies.EnemyTypes.PLAYER);

                        bullets.Add(leftBullet);
                        bullets.Add(rightBullet);
                    }
                }
                return;
            }

            //homing
            if (Weapon == 3)
            {
                bullets.Add(new HomingBullet(CenterXForShotBullet, Y, Speed + 10, 0));
                return;
            }

            throw new ArgumentException($"weaponとして不正な値が指定されています。weapon = {Weapon}");
        }
    }

    internal class BoundBullet : Bullet
    {
        //public static int LEFT_DEGREE  = -20;
        //public static int RIGHT_DEGREE =  20;
        public static int WIDE_LEFT_DEGREE = -65;
        public static int WIDE_RIGHT_DEGREE = 65;

        int boundCounter;

        public BoundBullet(int x, int y, int speed, int degree, int damage) : base(x, y, RADIUS_FOR_MEDIUM, speed, degree, damage, EnemyTypes.PLAYER)
        {
            boundCounter = 1;
        }

        public override void Action()
        {
            base.Action();

            if (boundCounter > 0)
            {
                if (Y <= 0)
                {
                    degree += Math.Sign(degree) * 90;
                    base.Action();
                    boundCounter--;
                }
                if (X <= App.window.moveableLeftSidePosition || X >= App.window.moveableRightSidePosition)
                {
                    degree *= -1;
                    base.Action();
                    boundCounter--;
                }
            }
        }
    }

    internal class HomingBullet : Bullet
    {
        private int homingTimer;

        public HomingBullet(int x, int y, int speed, int degree) : base(x, y, RADIUS_FOR_MEDIUM, speed, degree, 1, EnemyTypes.PLAYER)
        {
            homingTimer = 100;
        }

        public override void Action()
        {
            if (homingTimer > 0 && App.window.enemies.Count > 0)
            {
                List<Enemy> enemies = App.window.enemies;

                int minDistanceEnemyIndex = 0;
                int minSquareDistance = (CenterX - enemies[minDistanceEnemyIndex].CenterX) * (CenterX - enemies[minDistanceEnemyIndex].CenterX) + (CenterY - enemies[minDistanceEnemyIndex].CenterY) * (CenterY - enemies[minDistanceEnemyIndex].CenterY);

                if (enemies[minDistanceEnemyIndex] is Boss)
                {
                    Boss b = (Boss) enemies[minDistanceEnemyIndex];
                    minSquareDistance = (CenterX - b.CenterX) * (CenterX - b.CenterX) + (CenterY - b.CenterY) * (CenterY - b.CenterY);
                }

                for (int i = 1; i < enemies.Count; i++)
                {
                    int squareDistance;

                    if (enemies[i] is Boss)
                    {
                        Boss b = (Boss) enemies[i];
                        squareDistance = (CenterX - b.CenterX) * (CenterX - b.CenterX) + (CenterY - b.CenterY) * (CenterY - b.CenterY);
                    }
                    else
                    {
                        squareDistance = (CenterX - enemies[i].CenterX) * (CenterX - enemies[i].CenterX) + (CenterY - enemies[i].CenterY) * (CenterY - enemies[i].CenterY);
                    }

                    if (squareDistance < minSquareDistance)
                    {
                        minDistanceEnemyIndex = i;
                        minSquareDistance = squareDistance;
                    }
                }

                Enemy target = enemies[minDistanceEnemyIndex];

                int dx;
                int dy;

                if (target is Boss)
                {
                    Boss b = (Boss)target;

                    dx = b.CenterX - CenterX;
                    dy = b.CenterY - CenterY;   
                }
                else 
                {
                    dx = target.CenterX - CenterX;
                    dy = target.CenterY - CenterY;
                }

                double radian = Math.PI - Math.Atan2(dx, dy);

                degree = (int)(radian * 180 / Math.PI);

                homingTimer--;
            }

            base.Action();
        }
    }
}
