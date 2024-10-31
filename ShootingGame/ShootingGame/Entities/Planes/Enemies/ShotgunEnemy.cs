namespace ShootingGame.Entities.Planes.Enemies
{
    class ShotgunEnemy : Enemy
    {
        public ShotgunEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/3, Images.SHOTGUN_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 200)
        {
        }

        protected override int GetEXP()
        {
            return Level * 2;
        }

        /// <summary>
        /// 放射状に三つの弾を発生させる。進行方向とそこから±60deg方向
        /// </summary>
        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new(CenterXForShotBullet ,Y , bulletRadius , 8 , 150 , Level,EnemyTypes.SHOTGUN_ENEMY),
                new(CenterXForShotBullet ,Y , bulletRadius , 8 , 180 , Level,EnemyTypes.SHOTGUN_ENEMY),
                new(CenterXForShotBullet ,Y , bulletRadius , 8 ,-150 , Level,EnemyTypes.SHOTGUN_ENEMY),
            };

            return bullets;
        }
    }
}
