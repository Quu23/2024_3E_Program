using ShootingGame.Entities.Items;
using System.Windows;
using static ShootingGame.Entities.Items.StatusEffects;

namespace ShootingGame.Entities.Planes
{
    public class Player : Plane
    {
        public int MAX_HP;
        private int exp;

        public Dictionary<StatusEffects, int> status;

        public int defaultSpeed;

        public Player() : base(/*x=*/150, /*y=*/500, /*r=*/8, /*speed=*/5, Images.PLAYER_IMAGE, /*LV=*/1, /*hp=*/5, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 10)
        {
            //最初は5にする？
            MAX_HP = 5;
            defaultSpeed = Speed;
            status = new Dictionary<StatusEffects, int>() { 
                // 効果　　　　　　効果時間
                { SPEED_UP       ,0}, 
                { SPEED_DOWN     ,0},
                { SHOT_RATE_UP   ,0},
                { SHOT_RATE_DOWN ,0},
                { INVINCIBLE     ,0},
            };

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
            if (status[INVINCIBLE] <= 0 && base.IsHit(target)) return true;
            return false;
        }

        public override void Action()
        {
            if (BulletCoolTime > 0) BulletCoolTime -= DecreaceBulletCoolTime;

            foreach (var kvp in status)
            {
                if (kvp.Value > 0) status[kvp.Key] --;
            }

            Move();
            ChangeRect(X, Y);
            if (BulletCoolTime <= 0 && MainWindow.isKeyPresseds[4])
            {
                App.window.bullets.AddRange(ShotBullet());
                BulletCoolTime = MaxBulletCoolTime;
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
            bullets.Add(new Bullet(X + Radius, Y, 8, Speed + 5, 0, Level, Id.PLAYER));
            return bullets;
        }
    }
}
