﻿namespace ShootingGame.Entities.Planes.Enemies
{
    class StraightEnemy : Enemy
    {
        public StraightEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.STRAIGHT_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 + level, Bullet.RADIUS_FOR_MEDIUM, 200)
        {
        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            {
                new(CenterXForShotBullet, Y, Bullet.RADIUS_FOR_MEDIUM,Speed + 5, 180, Level, EnemyTypes.STRAIGHT_ENEMY)
            };
            return bullets;
        }
    }
}
