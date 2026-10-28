using ShootingGame.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame.Entities.Planes.Enemies
{
    class WarpEnemy : Enemy
    {
        private int count = 2;
        public WarpEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/3, Images.SHOTGUN_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 200)
        {
        }

        protected override int GetEXP()
        {
            return Level * 2;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            {
                new(CenterXForShotBullet, Y, Bullet.RADIUS_FOR_MEDIUM,Speed + 5, 180, Level, EnemyTypes.STRAIGHT_ENEMY)
            };
            return bullets;
        }

      

        public override void DeadAction(Player player, List<Enemy> enemies, List<Item> items)
        {
            base.DeadAction(player, enemies, items);
            Random random = new Random();
            if (count > 0)
            {
                count--;
                X = random.Next(App.window.moveableLeftSidePosition, App.window.moveableRightSidePosition);
                enemies.Add(new WarpEnemy(X, Y, 1));
            }
            
        }
    }
        
}
