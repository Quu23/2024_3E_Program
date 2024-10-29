
using ShootingGame.Entities.Items;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    class Boss3 : Boss
    {
        int actionCounter = 0;

        int itemDropCounter = 0;

        int laserCounter = 0;

        readonly int[] defaultMoveableSidePositions;

        int patern;

        private const int PATERN_A = 0;
        private const int PATERN_B = 1;
        private const int PATERN_C = 2;
        private const int PATERN_D = 3;
        private const int PATERN_E = 4;
        private const int PATERN_F = 5;

        public Boss3() : base(App.window.moveableLeftSidePosition + 50, 2, 500, 100, 5, Images.STRAIGHT_ENEMY_IMAGE, 2000, Bullet.RADIUS_FOR_BIG, 50, GenerateFollowers())
        {
            defaultMoveableSidePositions = new int[2];
            defaultMoveableSidePositions[0] = App.window.moveableLeftSidePosition;
            defaultMoveableSidePositions[1] = App.window.moveableRightSidePosition;

            patern = PATERN_A;
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

        public override void Action()
        {
            base.Action();

            laserCounter++;
            if(patern == PATERN_C)itemDropCounter++;

            if (laserCounter >= 50)
            {
                for (int i = 0; 0 <= defaultMoveableSidePositions[0] - (App.window.moveableLeftSidePosition + i * 30); i++)
                {
                    if (i > 0 && i % 2 == 1) continue;
                    App.window.enemies.Add(new LaserEnemy(App.window.moveableLeftSidePosition + i * 30, 0, 1));
                    App.window.enemies.Add(new LaserEnemy(App.window.moveableRightSidePosition - 30 - 30 * i, 0, 1));
                }
                laserCounter = 0;
            }

            switch (patern)
            {
                case PATERN_A:
                    break;
                case PATERN_B:
                    break;
                case PATERN_C:
                    //LaserEnemyとおなじ間隔て降らす
                    if (itemDropCounter >= 50)
                    {
                        App.window.items.Add(new SpeedDownItem(defaultMoveableSidePositions[1] - 50,10));
                        itemDropCounter = 0;
                    }
                    break;
                case PATERN_D:
                    double loopCount = (App.window.moveableRightSidePosition - App.window.moveableLeftSidePosition)/30.0;

                    for (int i = 0; i < loopCount && actionCounter == 0; i++)
                    {
                        if (i * 30 + App.window.moveableLeftSidePosition > defaultMoveableSidePositions[1] - 50) break;
                        App.window.enemies.Add(new LaserEnemy(30 * i + App.window.moveableLeftSidePosition, 10,1));
                    }

                    actionCounter++;
                    break;
                case PATERN_E: 
                    break;
                case PATERN_F:
                    actionCounter++;
                    if (App.window.moveableLeftSidePosition < defaultMoveableSidePositions[0]) App.window.moveableLeftSidePosition++;
                    if (App.window.moveableRightSidePosition > defaultMoveableSidePositions[1]) App.window.moveableRightSidePosition--;
                    App.window.unmoveableAreaRect = new Rect(0, 0, App.window.moveableLeftSidePosition, App.window.Height - 15);

                    if (actionCounter % 10 == 0)
                    {
                        App.window.items.Add(new ShotRateDownItem(App.window.player.X - 50, App.window.player.Y));
                        App.window.items.Add(new SpeedDownItem(App.window.player.X + 50, App.window.player.Y));
                    }

                    if (App.window.moveableLeftSidePosition == defaultMoveableSidePositions[0] && App.window.moveableRightSidePosition == defaultMoveableSidePositions[1] && actionCounter > 200)
                    {
                        patern++;
                        patern %= 6;
                        actionCounter=0;
                    }
                    break;
            }
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


            if ((double)Hp / MAX_HP < 0.75) 
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

            if (laserCounter >= 50 && patern == PATERN_F)
            {
                double loopCount = (App.window.moveableRightSidePosition - App.window.moveableLeftSidePosition) / Bullet.RADIUS_FOR_BIG;
                for (int i = 0; i < loopCount; i++)
                {
                    bullets.Add(new Bullet(Bullet.RADIUS_FOR_BIG * i, (int)App.window.Height, bulletRadius, 2, 0,1,EnemyTypes.STRAIGHT_ENEMY));
                }
            }

            return bullets;
        }

        protected override void Move()
        {
            switch (patern)
            {
                case PATERN_A:
                    //幅を100ずつ広げる。
                    if ((X <= App.window.moveableLeftSidePosition && Speed < 0) || (X >= App.window.moveableRightSidePosition - Width && Speed > 0))
                    {
                        Speed *= -1;
                        actionCounter++;
                        App.window.moveableLeftSidePosition  -= 30;
                        App.window.moveableRightSidePosition += 30;
                        App.window.unmoveableAreaRect = new Rect(0, 0, App.window.moveableLeftSidePosition, App.window.Height - 15);
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

                    if (actionCounter >= 5)
                    {
                        actionCounter = 0;
                        Speed *= -1;
                        patern++;
                        patern %= 6;
                    }
                    break;
                case PATERN_B:
                    X -= (int)(Speed * Math.Cos(Math.PI / 180 * 40));
                    Y += (int)(Speed * Math.Sin(Math.PI / 180 * 40));


                    foreach (Follower follower in followers)
                    {
                        follower.X -= (int)(Speed * Math.Cos(Math.PI / 180 * 40));
                        follower.Y += (int)(Speed * Math.Sin(Math.PI / 180 * 40));
                        Rect tmp = follower.Rect;
                        tmp.X = follower.X;
                        tmp.Y = follower.Y;
                        follower.Rect = tmp;

                        follower.wrappedEnemy.X -= (int)(Speed * Math.Cos(Math.PI / 180 * 40));
                        follower.wrappedEnemy.Y += (int)(Speed * Math.Sin(Math.PI / 180 * 40));
                        follower.wrappedEnemy.Rect = tmp;

                    }

                    if (X <= App.window.moveableLeftSidePosition)
                    {
                        patern++;
                        patern %= 6;
                        actionCounter = 0;
                    }

                    break;
                case PATERN_C:
                    actionCounter++;

                    if (actionCounter >= 500)
                    {
                        patern++;
                        patern %= 6;
                        actionCounter = 0;
                    }
                    break;
                case PATERN_D:
                    if (Y > 2 + Speed) Y -= Speed;

                    actionCounter++;

                    foreach (Follower follower in followers)
                    {
                        if (Y > 2 + Speed) follower.Y -= Speed;
                        Rect tmp = follower.Rect;
                        tmp.Y = follower.Y;
                        follower.Rect = tmp;

                        if (Y > 2 + Speed) follower.wrappedEnemy.Y -= Speed;
                        follower.wrappedEnemy.Rect = tmp;
                    }

                    if (actionCounter >= 400)
                    {
                        actionCounter = 0;
                        patern++;
                        patern %= 6;

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
                case PATERN_E:

                    actionCounter++;

                    double radian;

                    double dx;
                    double dy;

                    if (actionCounter < 300)
                    {
                        dx = App.window.player.CenterX - CenterX;
                        dy = App.window.player.CenterY - CenterY;
                    }
                    else
                    {
                        dx = defaultMoveableSidePositions[0] + 50 - X;
                        dy = 2- Y;
                    }
                    radian = Math.Atan2(dy, dx);

                    X += (int)(Speed * Math.Cos(radian));
                    Y += (int)(Speed * Math.Sin(radian));


                    foreach (Follower follower in followers)
                    {
                        follower.X += (int)(Speed * Math.Cos(radian));
                        follower.Y += (int)(Speed * Math.Sin(radian));
                        Rect tmp = follower.Rect;
                        tmp.X = follower.X;
                        tmp.Y = follower.Y;
                        follower.Rect = tmp;

                        follower.wrappedEnemy.X += (int)(Speed * Math.Cos(radian));
                        follower.wrappedEnemy.Y += (int)(Speed * Math.Sin(radian));
                        follower.wrappedEnemy.Rect = tmp;

                    }

                    if (actionCounter > 300 && dx * dx + dy * dy <= 9)
                    {
                        actionCounter = 0;
                        patern++;
                        patern %= 6;

                        X = defaultMoveableSidePositions[0] + 50;
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
                case PATERN_F:
                    ;//何もしない
                    break;
            }
        }
    }
}
