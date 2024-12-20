﻿namespace ShootingGame.Entities.Planes.Enemies
{
    class SplashEnemy : Enemy
    {
        private int degree;
        int sign;

        public SplashEnemy(int x, int y,  int level) : base(x, y, 20, 1, Images.SPLASH_ENEMY_IMAGE, level, 2 + level ,Bullet.RADIUS_FOR_SMALL ,30)
        {
            degree = 150;
            sign = 1;
        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>() {
                new Bullet(CenterXForShotBullet,Y,bulletRadius,Speed+5,degree,1,EnemyTypes.SPLASH_ENEMY),
            };

            if (degree <= 150 )
            {
                sign = 1; 
            }
            else if(degree >= 210)
            {
                sign = -1; 
            }

            degree += 10 * sign;

            return bullets;
        }
    }
}
