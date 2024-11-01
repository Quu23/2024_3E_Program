
using ShootingGame.Entities.Items;
using System.Windows;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    class Boss2 : Boss
    {
        int actionCount = 0;

        int followerAddCounter = 0;

        int patern;

        private const int PATERN_A = 0;
        private const int PATERN_B = 1;
        private const int PATERN_C = 2;

        public Boss2() : base(App.window.moveableLeftSidePosition + 50, 2, 500, 100, 3, Images.BOSS2_IMAGE, 700, Bullet.RADIUS_FOR_BIG, 200, GenerateFollowers())
        {
            patern = PATERN_A;
        }

        public override void Action()
        {
            base.Action();

            switch (patern)
            {
                case PATERN_A:
                    if (actionCount == 3 && App.window.player.status[StatusEffects.INVINCIBLE] == 0 && App.window.items.Count <=2)
                    {
                        int randX = new Random().Next(App.window.moveableLeftSidePosition + 30, App.window.moveableRightSidePosition - 30);
                        App.window.items.Add(new InvincibleItem(randX,300));
                    }
                    break;
                case PATERN_B:
                    if (App.window.enemies.Count <= 5)
                    {
                        App.window.enemies.Add(new MissileEnemy(X + 50 , 100, 2));
                        App.window.enemies.Add(new MissileEnemy(X + 400, 100, 2));
                    }
                    break;
                case PATERN_C:
                    if (App.window.enemies.Count == 1)
                    {
                        App.window.enemies.Add(new SnakeEnemy(X + 50 , 100, 2));
                        App.window.enemies.Add(new SnakeEnemy(X + 400, 100, 2));
                    }
                    break;
            }
            if (followers.Count == 0)
            {
                if (followerAddCounter <= 0)
                {

                    followers.AddRange(new List<Follower>()
                    {
                        new Follower(new SplitEnemy(X + 50 ,Y + 100,1), 50),
                        new Follower(new StraightEnemy(CenterX-30,Y + 100,1), 50),
                        new Follower(new SplitEnemy(X + 350,Y + 100,1), 50),
                    });
                    App.window.enemies.AddRange(followers);

                    followerAddCounter = 100;
                }
                else
                {
                    followerAddCounter--;
                }
            }
        }

        protected override int GetEXP()
        {
            return 100;
        }

        protected override List<Bullet> ShotBullet()
        {
            return new List<Bullet>
            {
                new Bullet(CenterXForShotBullet,Y,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                new Bullet(CenterXForShotBullet,Y+2 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
                new Bullet(CenterXForShotBullet,Y+4 * bulletRadius,bulletRadius,30,180,5,EnemyTypes.STRAIGHT_ENEMY),
            };
        }

        private static List<Follower> GenerateFollowers()
        {
            return new List<Follower> {
                new Follower(new SplitEnemy(App.window.moveableLeftSidePosition + 50,100,2), 25),
                new Follower(new HexagonEnemy(App.window.moveableLeftSidePosition  + 150,100,2), 150),
                new Follower(new HexagonEnemy(App.window.moveableLeftSidePosition  + 400,100,2), 150),
                new Follower(new SplitEnemy(App.window.moveableLeftSidePosition + 450,100,2), 25),
            };
        }

        protected override void Move()
        {
            switch (patern)
            {
                case PATERN_A:

                    if ((X <= App.window.moveableLeftSidePosition && Speed < 0) || (X >= App.window.moveableRightSidePosition - Width && Speed > 0))
                    {
                        Speed *= -1;
                        actionCount++;
                    }

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

                    if (actionCount >= 4)
                    {
                        actionCount = 0;
                        patern++;
                        patern %= 3;
                    }
                    break;
                case PATERN_B:

                    if ((X <= App.window.moveableLeftSidePosition && Speed < 0) || (X >= App.window.moveableRightSidePosition - Width && Speed > 0))
                    {
                        Speed *= -1;
                        actionCount++;
                    }

                    X += Speed;
                    double r = (App.window.Width -Width) / 2;
                    int dy = (int)(1.5 * Math.Sqrt(r * r - (X - r) * (X - r)) - r) - Y;
                    Y += dy;
                    

                    foreach (Follower follower in followers)
                    {
                        follower.X += Speed;
                        follower.Y += dy;
                        Rect tmp = follower.Rect;
                        tmp.X = follower.X;
                        tmp.Y = follower.Y;
                        follower.Rect = tmp;

                        follower.wrappedEnemy.X += Speed;
                        follower.wrappedEnemy.Y += dy;
                        follower.wrappedEnemy.Rect = tmp;

                    }

                    if (actionCount >= 4)
                    {
                        actionCount = 0;
                        patern++;
                        patern %= 3;

                    }
                    break;
                case PATERN_C:
                    int dx = (int)((App.window.Width - Width) / 8 * Math.Sin(Math.PI / 20 * actionCount) + (App.window.Width - Width) / 2) - X;
                    X += dx;
                    if(Y > 2 + Speed)Y -= Speed;
                    actionCount++;

                    foreach (Follower follower in followers)
                    {
                        follower.X += dx;
                        if (Y > 2 + Speed) follower.Y -= Speed;
                        Rect tmp = follower.Rect;
                        tmp.X = follower.X;
                        tmp.Y = follower.Y;
                        follower.Rect = tmp;

                        follower.wrappedEnemy.X += dx;
                        if (Y > 2 + Speed) follower.wrappedEnemy.Y -= Speed;
                        follower.wrappedEnemy.Rect = tmp;
                    }

                    if (actionCount >= 400)
                    {
                        actionCount = 0;
                        patern++; 
                        patern %= 3;

                        Y = 2;

                        foreach (Follower follower in followers)
                        {
                            follower.Y = 100;
                            Rect tmp = follower.Rect;
                            tmp.Y = follower.Y;
                            follower.Rect = tmp;

                            follower.wrappedEnemy.Y = 100;
                            follower.wrappedEnemy.Rect = tmp;

                        }
                    }
                    break;
            }
        }
    }
}
