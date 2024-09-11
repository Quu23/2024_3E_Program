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

        public static readonly int RADIUS_FOR_SMALL  = 4;
        public static readonly int RADIUS_FOR_MEDIUS = 8;
        public static readonly int RADIUS_FOR_BIG    = 16;

        /// <param name="degree">弾の進行方向を表す。プレイヤーの進行方向（画面の下から上）を0度として時計回りが正。一般角θとの関係は、degree = -θ + 90° </param>
        /// <param name="id">Id列挙型の要素を用いる。</param>
        public Bullet(int x, int y, int radius, int speed, int degree, int damage, Id id) : base(x, y, radius, speed, id == Id.PLAYER ? Images.PLAYER_BULLET_IMAGE : Images.ENEMY_BULLET_IMAGE)
        {
            this.degree = degree;
            this.damage = damage;
            this.id = id;

            if(id == Id.ENEMY)
            {
                if (radius == RADIUS_FOR_SMALL) Img = Images.ENEMY_BULLET_SMALL_IMAGE;
                if (radius == RADIUS_FOR_BIG)   Img = Images.ENEMY_BULLET_BIG_IMAGE;
            }
        }
        /// <summary>
        /// プレイヤーか敵かを識別する用。enumのIdを用いる。
        /// </summary>
        public Id Id { get => id;}
        public int Damage { get => damage;}

        public override void Move()
        {
            Y -= (int)(Speed * Math.Cos(degree * Math.PI / 180));
            X += (int)(Speed * Math.Sin(degree * Math.PI / 180));
            ChangeRect(X, Y);
        }
    }

}
