using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    public class Player : Plane
    {
        private int MAX_HP;
        private int exp;

        public Player() : base(/*x=*/150, /*y=*/500, /*r=*/8, /*speed=*/5, new BitmapImage(ImageUris.PLAYER), /*LV=*/1, /*hp=*/5, 20)
        {
            //最初は5にする？
            MAX_HP = 5;
        }

        public int Exp { get => exp; set => exp = value; }
        public int GetMaxHp { get => MAX_HP; }

        //TODO:レベルアップした時の処理を考える
        public void LevelUp()
        {
            Level++;
            Speed++;
            MAX_HP += 2; 
            HeelFullOfHp();
        }

        private void HeelFullOfHp()
        {
            Hp = MAX_HP;
        }

        public override void Move()
        {
            if (X > 0 && MainWindow.isKeyPresseds[1])
            {
                X -= Speed;
            }
            if(X < 0)X = 0;

            // xはエンティティの左上の座標だから、右に行くときは「x+幅」、つまりエンティティの右端が画面の端かどうかで判断。
            if (X + Img.Width < SystemParameters.PrimaryScreenWidth && MainWindow.isKeyPresseds[3])
            {
                X += Speed;
            }
            if(X + Img.Width > SystemParameters.PrimaryScreenWidth) X=(int)(SystemParameters.PrimaryScreenWidth - Img.Width);

            if (Y > 0 && MainWindow.isKeyPresseds[0]) 
            {
                Y -= Speed;
            }
            if(Y < 0)Y = 0;

            if (Y + Img.Height < SystemParameters.PrimaryScreenHeight && MainWindow.isKeyPresseds[2])
            {
                Y += Speed;
            }
            if (Y + Img.Height > SystemParameters.PrimaryScreenHeight) Y = (int)(SystemParameters.PrimaryScreenHeight - Img.Height);

            ChangeRect(X, Y);
        }


        // TODO:levelに応じた弾の出し方・攻撃力の設定
        // TODO:弾の画像設定
        public override List<Bullet> ShotBullet()
        {
            //弾の追加を行うかもしれないからListはこの書き方のままでいい。
            List<Bullet> bullets = new List<Bullet>();
            bullets.Add(new Bullet(X, Y, 8, Speed + 5, 0, Level, Id.PLAYER));
            return bullets;
        }
    }
}
