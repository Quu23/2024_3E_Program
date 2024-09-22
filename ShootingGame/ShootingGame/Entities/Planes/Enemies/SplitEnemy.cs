using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class SplitEnemy : Enemy
    {
        private readonly int START_HP;
        public SplitEnemy(int x, int y,int level, int hp) : base(x, y, 20, 5, null, level, 20 , Bullet.RADIUS_FOR_MEDIUM, 20)
        {
            START_HP = hp;
        }

        protected override int GetEXP()
        {
            return 10;
        }

        protected override List<Bullet> ShotBullet()
        {
            return new List<Bullet>() {
                new Bullet(CenterXForShotBullet,Y,bulletRadius,Speed+5,180,3,Id.ENEMY),     
            };
        }

        public override void DeadAction(Player player, List<Enemy> enemies)
        {
            base.DeadAction(player, enemies);
            if (START_HP > 1)
            {
                enemies.Add(new SplitEnemy(X-Radius, Y, Level, START_HP / 2));
                enemies.Add(new SplitEnemy(X+Radius, Y, Level, START_HP / 2));
            }
        }
    }
}
