
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    internal class Boss3 : Boss
    {
        public Boss3() : base(App.window.moveableLeftSidePosition + 50, 2, 500, 100, 5, Images.STRAIGHT_ENEMY_IMAGE, 2000, Bullet.RADIUS_FOR_BIG, 50, GenerateFollowers())
        {
        }

        private static List<Follower> GenerateFollowers()
        {
            return new List<Follower> {
                new Follower(new SplashEnemy(App.window.moveableLeftSidePosition + 100,100,1), 300),
                new Follower(new CycloneEnemy(App.window.moveableLeftSidePosition  + 150,100,1), 150),
                new Follower(new CycloneEnemy(App.window.moveableLeftSidePosition  + 250,100,1), 150),
                new Follower(new CycloneEnemy(App.window.moveableLeftSidePosition  + 300,100,1), 150),
                new Follower(new CycloneEnemy(App.window.moveableLeftSidePosition  + 400,100,1), 150),
                new Follower(new SplashEnemy(App.window.moveableLeftSidePosition + 450,100,1), 300),
            };
        }

        protected override int GetEXP()
        {
            return 1000 ;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            { 
                new Bullet(CenterXForShotBullet,Y,bulletRadius,45,180,5,EnemyTypes.STRAIGHT_ENEMY),
                new Bullet(CenterXForShotBullet,Y+2 * bulletRadius,bulletRadius,50,180,5,EnemyTypes.STRAIGHT_ENEMY),
                new Bullet(CenterXForShotBullet,Y+4 * bulletRadius,bulletRadius,45,180,5,EnemyTypes.STRAIGHT_ENEMY),
            };


            if ((double)Hp / MAX_HP < 0.5) 
            {
                bullets.AddRange(new List<Bullet>
                {
                    new Bullet(CenterXForShotBullet-50,Y,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                    new Bullet(CenterXForShotBullet-50,Y+2 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                    new Bullet(CenterXForShotBullet-50,Y+4 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                    new Bullet(CenterXForShotBullet+50,Y,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                    new Bullet(CenterXForShotBullet+50,Y+2 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                    new Bullet(CenterXForShotBullet+50,Y+4 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                });
            }

            return bullets;
        }

        protected override void Move()
        {
            if ((X <= App.window.moveableLeftSidePosition && Speed < 0) || (X >= App.window.moveableRightSidePosition - Width && Speed > 0)) Speed *= -1;

            X += Speed;

            foreach (Follower follower in followers)
            {
                follower.X += Speed;
                Rect tmp = follower.Rect;
                tmp.X = follower.X;
                follower.Rect = tmp;

                follower.wrappedEnemy.X += Speed;
                follower.wrappedEnemy.Rect = tmp;

            }
        }
    }
}
