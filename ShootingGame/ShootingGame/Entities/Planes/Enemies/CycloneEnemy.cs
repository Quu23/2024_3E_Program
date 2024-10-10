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

        public CycloneEnemy(int x, int y, int level) : base(x, y, /*r=*/16, /*speed=*/3, Images.STRAIGHT_ENEMY_IMAGE, level, level * 3, Bullet.RADIUS_FOR_MEDIUM, 200)
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
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius,   0, 1, Id.ENEMY, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius,  72, 1, Id.ENEMY, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 144, 1, Id.ENEMY, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 216, 1, Id.ENEMY, this),
                new CycloneBullet(CenterXForShotBullet, Y, bulletRadius, 288, 1, Id.ENEMY, this),
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

        private readonly int firstDegree;

        int realX;
        int realY;


        //回転行列の係数 a = cos θ , y = sin θ
        double a;
        double b;

        CycloneEnemy parent;

        public CycloneBullet(int x, int y, int radius, int first_degree, int damage, Id id, CycloneEnemy en) : base(x, y, radius, 3, first_degree, damage, id)
        {
            degree = 1;
            moveTime = 0;
            firstDegree = first_degree;

            a = Math.Cos(first_degree * Math.PI / 180);
            b = Math.Sin(first_degree * Math.PI / 180);

            parent = en;
            realX = x;
            realY = y;
        }

        protected override void Move()
        {
            moveTime++;

            //if (moveTime < 10)
            //{
            //    base.Move();
            //    return;
            //}

            realY -= (int)(Speed * Math.Exp(degree * Math.PI / 180) * Math.Cos(degree * Math.PI / 180));
            realX += (int)(Speed* Math.Exp(degree * Math.PI / 180) * Math.Sin(degree * Math.PI / 180));

            int tmpX = realX - parent.CenterX;
            int tmpY = realY - parent.CenterY;

            X = (int)(a * tmpX - b * tmpY) + parent.CenterX;
            Y = (int)(a * tmpX + a * tmpY) + parent.CenterY;

            degree+=5;

            if ((degree > 90 && Speed >= 3 ) || (degree > 135 && Speed >= 2))
            {
                Speed--;
            }

            ChangeRect(X, Y);
        }
    }
}
