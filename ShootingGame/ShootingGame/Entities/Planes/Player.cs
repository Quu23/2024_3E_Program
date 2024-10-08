﻿using ShootingGame.Entities.Items;
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

        public Player(string name) : base(/*x=*/150, /*y=*/500, /*r=*/8, /*speed=*/5, Images.PLAYER_IMAGE, /*LV=*/1, /*hp=*/5, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 30)
        {
            //最初は5にする？
            MAX_HP = 5;

            this.name = name;

            defaultSpeed = Speed;

            increaseRateOfScore = 100;

            status = new Dictionary<StatusEffects, int>() { 
                // 効果　　　　　　         効果時間
                { SPEED_UP                ,0},
                { SPEED_DOWN              ,0},
                { SHOT_RATE_UP            ,0},
                { SHOT_RATE_DOWN          ,0},
                { INCREACE_RATE_OF_SCORE  ,0},
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
                if (status[INVINCIBLE] <= 0) base.Hp = value; 
            } 
        }

        public int Exp { get => exp; set => exp = value; }
        public double GetMaxHp { get => MAX_HP; }

        //TODO:レベルアップした時の処理を考える
        public void LevelUp()
        {
            Level++;
            MAX_HP += 2;
            Hp += 2;
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

        protected override void Move()
        {
            if (X > 0 && MainWindow.isKeyPresseds[1])
            {
                X -= Speed;
            }
            if (X < 0) X = 0;

            // xはエンティティの左上の座標だから、右に行くときは「x+幅」、つまりエンティティの右端が画面の端かどうかで判断。
            if (X + Img.Width < SystemParameters.PrimaryScreenWidth && MainWindow.isKeyPresseds[3])
            {
                X += Speed;
            }
            if (X + Img.Width > SystemParameters.PrimaryScreenWidth) X = (int)(SystemParameters.PrimaryScreenWidth - Img.Width);

            if (Y > 0 && MainWindow.isKeyPresseds[0])
            {
                Y -= Speed;
            }
            if (Y < 0) Y = 0;

            if (Y + Img.Height < SystemParameters.PrimaryScreenHeight && MainWindow.isKeyPresseds[2])
            {
                Y += Speed;
            }
            if (Y + Img.Height > SystemParameters.PrimaryScreenHeight) Y = (int)(SystemParameters.PrimaryScreenHeight - Img.Height);
        }


        // TODO:levelに応じた弾の出し方・攻撃力の設定
        // TODO:弾の画像設定
        protected override List<Bullet> ShotBullet()
        {
            //弾の追加を行うかもしれないからListはこの書き方のままでいい。
            List<Bullet> bullets = new List<Bullet>();
            if (status[DESTROY_MODE] > 0)
            {
                Bullet destroyBullet = new Bullet(X + Radius, Y - 50, 100, 4, 0, 9999, Id.PLAYER); 
                destroyBullet.Img = Images.DESTROY_ITEM_IMAGE;  
                bullets.Add(destroyBullet);
            }
            else
            {
                bullets.Add(new Bullet(X + Radius, Y, 8, Speed + 5, 0, Level, Id.PLAYER));
            }   
            return bullets;
        }
    }
}
