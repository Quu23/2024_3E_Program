using System.Windows;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    class Boss1 : Boss
    {
        public Boss1() : base(App.window.moveableLeftSidePosition + 50, 2, 500, 100, 1, Images.BOSS1_IMAGE, 400, Bullet.RADIUS_FOR_BIG, 200, GenerateFollowers())
        {
        }

        protected override int GetEXP()
        {
            return 50;
        }

        public override void Action()
        {
            base.Action();

            //BOSSだけの状態だったら
            if (App.window.enemies.Count == 1)
            {
                App.window.enemies.Add(new SnakeEnemy(X +  50, 100, 1));
                App.window.enemies.Add(new SnakeEnemy(X + 400, 100, 1));
            }
        }

        protected override List<Bullet> ShotBullet()
        {
            return new List<Bullet>
            {
                new Bullet(CenterXForShotBullet,Y,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
            };
        }

        private static List<Follower> GenerateFollowers() 
        {
            return new List<Follower> {
                new Follower(new StraightEnemy(App.window.moveableLeftSidePosition + 100,100,1),100),
                new Follower(new ShotgunEnemy(App.window.moveableLeftSidePosition  + 150,100,1),100),
                new Follower(new ShotgunEnemy(App.window.moveableLeftSidePosition  + 400,100,1),100),
                new Follower(new StraightEnemy(App.window.moveableLeftSidePosition + 450,100,1),100),
            };
        }

        protected override void Move()
        {
            if ((X <= App.window.moveableLeftSidePosition && Speed < 0) || (X >= App.window.moveableRightSidePosition - Width && Speed > 0)) Speed *= -1;

            X += Speed;

            foreach (Follower follower in followers) { 
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
