using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class Player : Plane
    {
        private int MAX_HP;
        private int exp;

        public Player() : base(150, 500, 3, 2, new BitmapImage(ImageUris.PLAYER), 1, 5, 5)
        {
            //最初は5にする？
            MAX_HP = 5;
        }

        public int Exp { get => exp; set => exp = value; }

        public void HeelFullOfHp()
        {
            Hp = MAX_HP;
        }

        public override void Move()
        {
            Canvas canvas = App.window.drawCanvas;

            if (X > 0 && MainWindow.isKeyPresseds[1])
            {
                X -= Speed;
            }
            if(X < 0)X = 0;

            // xはエンティティの左上の座標だから、右に行くときは「x+幅」、つまりエンティティの右端が画面の端かどうかで判断。
            // Canvas上で描画しているので、端の限界はMainWindowではなくMainCanvasのWidthにする。
            if (X + Img.Width < canvas.ActualWidth && MainWindow.isKeyPresseds[3])
            {
                X += Speed;
            }
            if(X + Img.Width > canvas.ActualWidth)X=(int)(canvas.ActualWidth - Img.Width);

            if (Y > 0 && MainWindow.isKeyPresseds[0]) 
            {
                Y -= Speed;
            }
            if(Y < 0)Y = 0;

            if (Y + Img.Height < canvas.ActualHeight && MainWindow.isKeyPresseds[3])
            {
                Y += Speed;
            }
            if (Y + Img.Height > canvas.ActualHeight) Y = (int)(canvas.ActualHeight - Img.Height);
        }


        // TODO:levelに応じた弾の出し方・攻撃力の設定
        // TODO:弾の画像設定
        public override List<Bullet> ShotBullet()
        {
            //弾の追加を行うかもしれないからListはこの書き方のままでいい。
            var bullets = new List<Bullet>();
            bullets.Add(new Bullet(X, Y, 2, Speed - 1, 0, new Image(), Level, Id.PLAYER));
            return bullets;
        }
    }
}
