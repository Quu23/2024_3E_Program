using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class SplashEnemy : Enemy
    {
        private int degree;
        int sign;

        public SplashEnemy(int x, int y,  int level) : base(x, y, 20, 3, Images.SPLASH_ENEMY_IMAGE, level, 5 ,Bullet.RADIUS_FOR_SMALL ,30)
        {
            degree = 150;
            sign = 1;
        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>() {
                new Bullet(CenterXForShotBullet,Y,bulletRadius,Speed+5,degree,3,Id.ENEMY),
            };
            if (degree <= 150 )
            {
                sign = 1; 
            }
            else if(degree >= 210)
            {
                sign = -1; 
            }

            degree += 10 * sign;
            return bullets;
        }
    }
}
