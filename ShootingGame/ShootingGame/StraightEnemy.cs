using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShootingGame
{
    class StraightEnemy : Enemy
    {
        public StraightEnemy(int x, int y, Image img, int level) : base(x, y, 3, 1+level, img, level, 5 + level , 10)
        {
        }

        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            {
                new(X, Y, 1, 1, 180, new Image(), Level, Id.ENEMY)
            };
            return bullets;
        }
    }
}
