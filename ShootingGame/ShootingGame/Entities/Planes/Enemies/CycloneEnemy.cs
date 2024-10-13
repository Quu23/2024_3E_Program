using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class CycloneEnemy : Enemy
    {

        public CycloneEnemy(int x, int y, int level) : base(x, y, /*r=*/16, /*speed=*/3, Images.CYCLONE_ENEMY_IMAGE, level, level * 3, Bullet.RADIUS_FOR_MEDIUM, 200)
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
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius,  45, 1, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 135, 1, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 225, 1, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 315, 1, this),
                //new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 288, 1, Id.ENEMY, this),
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

        int realX;
        int realY;

        //回転行列の係数 a = cos θ , y = sin θ
        double a;
        double b;

        CycloneEnemy parent;

        public CycloneBullet(int x, int y, int radius, int first_degree, int damage, CycloneEnemy en) : base(x, y, radius, 5, first_degree, damage, EnemyTypes.CYCLONE_ENEMY)
        {
            degree = 1;
            moveTime = 0;

            a = Math.Cos(-first_degree * Math.PI / 180);
            b = Math.Sin(-first_degree * Math.PI / 180);

            parent = en;

            realX = x;
            realY = y;
        }

        protected override void Move()
        {
            moveTime++;

            realY -= (int)(Speed * Math.Exp(degree * Math.PI / 180) * Math.Cos(degree * Math.PI / 180));
            realX += (int)(Speed * Math.Exp(degree * Math.PI / 180) * Math.Sin(degree * Math.PI / 180));

            int tmpX = realX - parent.X;
            int tmpY = realY - parent.Y;

            X = (int)(a * tmpX - b * tmpY) + parent.X;
            Y = (int)(b * tmpX + a * tmpY) + parent.Y;

            degree+=5;

            if ((degree > 90 && Speed >= 5 ) || (degree > 135 && Speed >= 4) || (degree > 150 && Speed >= 3) || (degree > 180 && Speed >= 2))
            {
                Speed--;
            }

            ChangeRect(X, Y);
        }
    }
}
