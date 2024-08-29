using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    public enum Id { PLAYER , ENEMY }
    public class Bullet :Entity
    {
        private int degree;
        private int damage;
        private Id id;
        

        public Bullet(int x, int y, int radius, int speed, int degree, Image img, int damage, Id id) : base(x, y, radius, speed, new BitmapImage(ImageUris.P_BULLET))
        {
            this.degree = degree;
            this.damage = damage;
            this.id = id;
        }
        
        public override void Move()
        {
            Y -= (int)(Speed * Math.Cos(degree * Math.PI / 180));//degreeは進行方向に対して時計回りに大きくなる。
            X += (int)(Speed * Math.Sin(degree * Math.PI / 180));
            ChangeRect(X, Y);
        }
    }

}
