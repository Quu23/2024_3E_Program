﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Text;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class SplitEnemy : Enemy
    {
        
        public SplitEnemy(int x, int y, int level) :this(x, y, level, 40)
        {

        }

        private SplitEnemy(int x, int y,int level, int radius) : base(x, y, radius, 3, Images.SPLIT_ENEMY_IMAGE, level, 1 , Bullet.RADIUS_FOR_MEDIUM, 100)
        {
            if (radius <= 10) bulletRadius = Bullet.RADIUS_FOR_SMALL;
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
            if (Radius > 10)
            {
                enemies.Add(new SplitEnemy(X-Radius*2, Y, Level, Radius / 2));
                enemies.Add(new SplitEnemy(X+Radius*2, Y, Level, Radius / 2));
            }
        }
    }
}
