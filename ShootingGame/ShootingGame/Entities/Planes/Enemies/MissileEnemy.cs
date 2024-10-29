using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    internal class MissileEnemy : Enemy
    {
        private int chaseTimer;
        private int moveDirection;

        public MissileEnemy(int x, int y, int level) : base(x, y, /*r=*/20,  /*speed=*/10, Images.MISSILE_ENEMY_IMAGE.Clone(), /*LV=*/level, /*hp=*/1 + level, /*bulletRadius=*/Bullet.RADIUS_FOR_SMALL, /*maxBulletCooltime=*/100)
        {
            //追尾時間設定。調整求。
            chaseTimer = 150;
        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet, Y ,bulletRadius, Speed+5,moveDirection,Level, EnemyTypes.MISSILE_ENEMY)
            ];
        }
        protected override void Move()
        {
            double radian;

            if (chaseTimer >= 0)
            {
                chaseTimer--;

                double dx = App.window.player.CenterX - CenterX;
                double dy = App.window.player.CenterY - CenterY;

                radian = Math.PI - (Math.Atan2(dx, dy));
                moveDirection = (int)(radian * (180 / Math.PI));
            }
            else
            {
                radian = moveDirection * (Math.PI / 180);
            }

            X += (int)(1.3 * Speed * Math.Sin(radian));
            Y -= (int)(Speed * Math.Cos(radian)); 
        }
    }
}
