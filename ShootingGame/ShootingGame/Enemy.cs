using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShootingGame
{
    abstract public class Enemy : Plane
    {

        public Enemy(int x, int y, int radius, int speed, Image img, int level ,int hp ,int maxBulletCoolTime) : base(x, y, radius, speed, img, level ,hp ,maxBulletCoolTime)
        {
        }

        public override void Move()
        {
            Y -= Speed;
        }

        /// <summary>
        /// このEnemyを倒したときに獲得できるEXPを返す。
        /// </summary>
        /// <returns>このEnemyが持つEXP</returns>
        public abstract int GetEXP();
    }
}
