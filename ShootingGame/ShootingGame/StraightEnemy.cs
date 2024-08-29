using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class StraightEnemy : Enemy
    {
        public StraightEnemy(int x, int y, int level) : base(x, y, 3, 1+level, new BitmapImage(), level, 5 + level , 10)
        {
        }

        public override int GetEXP()
        {
            return 1;
        }

        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            {
                new(X, Y, 1, 1, 180, Level, Id.ENEMY)
            };
            return bullets;
        }
    }
}
