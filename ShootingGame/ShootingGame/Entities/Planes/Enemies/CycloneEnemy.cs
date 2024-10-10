using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    internal class CycloneEnemy : Enemy
    {

        public CycloneEnemy(int x, int y, int level) : base(x, y, /*r=*/16, /*speed=*/3, Images.STRAIGHT_ENEMY_IMAGE, level, level * 3, Bullet.RADIUS_FOR_MEDIUM, 50)
        {
        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, Speed     ,   0, 1, Id.ENEMY),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, Speed + 10,  72, 1, Id.ENEMY),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, Speed + 10, 144, 1, Id.ENEMY),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, Speed + 10, 216, 1, Id.ENEMY),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, Speed + 10, 288, 1, Id.ENEMY),
            };
            
            return bullets;

        }
    }

    class CycloneBullet : Bullet
    {
        /// <summary>
        /// 角度はdeg
        /// </summary>
        private int degree;
        private int moveTime;

        public CycloneBullet(int x, int y, int radius, int speed, int degree, int damage, Id id) : base(x, y, radius, speed, degree, damage, id)
        {
            degree = 0;
            moveTime = 0;
        }

        protected override void Move()
        {
            moveTime++;

            if (moveTime < 10)
            {
                base.Move();
                return;
            }

            Y -= (int)(Speed * Math.Exp(degree * Math.PI / 180) * Math.Cos(degree * Math.PI / 180));
            X += (int)(Speed * Math.Exp(degree * Math.PI / 180) * Math.Sin(degree * Math.PI / 180));

            degree++;

            ChangeRect(X, Y);
        }
    }
}
