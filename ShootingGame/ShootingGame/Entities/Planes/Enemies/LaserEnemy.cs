using System.Windows;

namespace ShootingGame.Entities.Planes.Enemies
{
    class LaserEnemy : Enemy
    {
        public LaserEnemy(int x, int y, int level) : base(x, y, 20, 1, Images.LASER_ENEMY_IMAGE, level, 1, Bullet.RADIUS_FOR_BIG, 200)
        {
        }

        protected override int GetEXP()
        {
            return 10;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new LaserBullet(CenterXForShotBullet ,Y + Radius, LaserBullet.STRAIGHT_DEGREE,bulletRadius),
                new Bullet(CenterXForShotBullet, Y+700,bulletRadius,80,LaserBullet.STRAIGHT_DEGREE,App.window.player.Hp/2,EnemyTypes.LASER_ENEMY)
            };
            bullets[1].Img = null;
            return bullets;
        }
        protected override void Move()
        {
            Y = Y-Speed;
        }
    }

    class LaserBullet : Bullet
    {
        public static readonly int STRAIGHT_DEGREE  = 180;
        public static readonly int LEFT_SIDE_DEGREE = 270;
        public static readonly int RIGHT_SIDE_DEGREE = 90;

        public LaserBullet(int x, int y, int degree,int radius) : base(x, y, radius, 80, degree, App.window.player.Hp/2, EnemyTypes.LASER_ENEMY)
        {
            Rect tmp = Rect;

            tmp.Width  = 30;
            tmp.Height = 700;

            if (degree == LEFT_SIDE_DEGREE || degree == RIGHT_SIDE_DEGREE)
            {
                tmp.Width  = 1000;
                tmp.Height = 30;

                if (degree == LEFT_SIDE_DEGREE)
                {
                    degree = RIGHT_SIDE_DEGREE;
                    Speed *= -1;
                }
            }

            Rect = tmp;
        }

        public override string ToString()
        {
            return $"x={X},y={Y}/deg={degree}  speed={Speed}";
        }
    }
}
