namespace ShootingGame.Entities.Planes.Enemies
{
    class HexagonEnemy : Enemy
    {
        public HexagonEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.HEXAGON_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_BIG, 400)
        {
        }

        protected override int GetEXP()
        {
            return Level * 2;
        }


        /// <summary>
        /// 放射状に六つの弾を発生させる。進行方向とそこから±60deg方向
        /// </summary>
        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                // todo:X,Yの微調整
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5,   0, 1, EnemyTypes.HEXAGON_ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5,  60, 1, EnemyTypes.HEXAGON_ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5, 120, 1, EnemyTypes.HEXAGON_ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5, 180, 1, EnemyTypes.HEXAGON_ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5, 240, 1, EnemyTypes.HEXAGON_ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+5, 300, 1, EnemyTypes.HEXAGON_ENEMY)
            };

            return bullets;

        }
    }
}