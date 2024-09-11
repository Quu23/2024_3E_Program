﻿using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class ShotgunEnemy : Enemy
    {
        public ShotgunEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/level + 5, Images.SHOTGUN_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, 70)
        {
        }

        public override int GetEXP()
        {
            return Level * 2;
        }

        /// <summary>
        /// 放射状に三つの弾を発生させる。進行方向とそこから±60deg方向
        /// </summary>
        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new(X + Radius ,Y , 2 , 8 , 150 , 1,Id.ENEMY),
                new(X + Radius ,Y , 2 , 8 , 180 , 1,Id.ENEMY),
                new(X + Radius ,Y , 2 , 8 ,-150 , 1,Id.ENEMY),
            };

            return bullets;
        }
    }
}
