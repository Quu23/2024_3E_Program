using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class BigEnemy : Enemy
    {
        public BigEnemy(int x, int y, int level) : base(x, y, 40, 1, null, level, 10, Bullet.RADIUS_FOR_BIG, 100)
        {
        }

        protected override int GetEXP()
        {
            return Level * 5;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>() { 
                new Bullet(CenterXForShotBullet,Y,bulletRadius,Speed+5,180,3,Id.ENEMY),
            };
            return bullets;
        }
    }
}
