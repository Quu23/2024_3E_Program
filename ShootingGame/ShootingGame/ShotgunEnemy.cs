using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class ShotgunEnemy : Enemy
    {
        public ShotgunEnemy(int x, int y, int level) : base(x, y, 3, level, new BitmapImage(), level, 3, 10)
        {
        }

        public override int GetEXP()
        {
            return 3;
        }

        /// <summary>
        /// 放射状に三つの弾を発生させる。進行方向とそこから±60deg方向
        /// </summary>
        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new(X ,Y , 2 , 1 , 120 , 1,Id.ENEMY),
                new(X ,Y , 2 , 1 , 180 , 1,Id.ENEMY),
                new(X ,Y , 2 , 1 ,-120 , 1,Id.ENEMY),
            };

            return bullets;
        }
    }
}
