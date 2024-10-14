using ShootingGame.Entities.Planes.Enemies;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities
{
    public class Bullet : Entity
    {
        protected int degree;
        private   int damage;
        /// <summary>
        /// typeはプレイヤーか敵かを識別するためのもの
        /// </summary>
        private EnemyTypes type;

        public static readonly int RADIUS_FOR_SMALL = 4;
        public static readonly int RADIUS_FOR_MEDIUM = 8;
        public static readonly int RADIUS_FOR_BIG = 16;

        /// <param name="degree">弾の進行方向を表す。プレイヤーの進行方向（画面の下から上）を0度として時計回りが正。一般角θとの関係は、degree = -θ + 90° </param>
        /// <param name="id">Id列挙型の要素を用いる。</param>
        public Bullet(int x, int y, int radius, int speed, int degree, int damage, EnemyTypes type) : base(x, y, radius, speed, GetBulletImage(radius,type))
        {
            this.degree = degree;
            this.damage = damage;
            this.type = type;
        }
        /// <summary>
        /// 弾の種類を識別する用。enumのIdを用いる。
        /// </summary>
        public EnemyTypes Type { get => type; }
        public int Damage { get => damage; }

        public override void Action()
        {
            base.Action();
        }

        protected override void Move()
        {
            Y -= (int)(Speed * Math.Cos(degree * Math.PI / 180));
            X += (int)(Speed * Math.Sin(degree * Math.PI / 180));
            ChangeRect(X, Y);
        }

        private static BitmapImage GetBulletImage(int radius,EnemyTypes type)
        {
            if (type == EnemyTypes.PLAYER)
            {
                if (radius == RADIUS_FOR_SMALL)
                {
                    return Images.PLAYER_BULLET_SMALL_IMAGE;
                }
                if (radius == RADIUS_FOR_BIG)
                {
                    return Images.PLAYER_BULLET_BIG_IMAGE;
                }
                return Images.PLAYER_BULLET_IMAGE;
            }
            else
            {
                if (radius == RADIUS_FOR_SMALL)
                {
                    return Images.ENEMY_BULLET_SMALL_IMAGE;
                }
                if (radius == RADIUS_FOR_BIG)
                {
                    return Images.ENEMY_BULLET_BIG_IMAGE;
                }
                return Images.ENEMY_BULLET_IMAGE;
            }
        }
    }

}
