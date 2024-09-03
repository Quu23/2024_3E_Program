using System.Windows.Media.Imaging;

namespace ShootingGame
{
    public enum Id { PLAYER , ENEMY }
    public class Bullet :Entity
    {
        private int degree;
        private int damage;
        /// <summary>
        /// idはプレイヤーか敵かを識別するためのもの
        /// </summary>
        private Id id;
        

        public Bullet(int x, int y, int radius, int speed, int degree, int damage, Id id) : base(x, y, radius, speed, new BitmapImage(id == Id.PLAYER ? ImageUris.P_BULLET : ImageUris.E_BULLET))
        {
            this.degree = degree;
            this.damage = damage;
            this.id = id;
        }

        public Id Id { get => id;}
        public int Damage { get => damage;}

        public override void Move()
        {
            Y -= (int)(Speed * Math.Cos(degree * Math.PI / 180));//degreeは進行方向に対して時計回りに大きくなる。
            X += (int)(Speed * Math.Sin(degree * Math.PI / 180));
            ChangeRect(X, Y);
        }
    }

}
