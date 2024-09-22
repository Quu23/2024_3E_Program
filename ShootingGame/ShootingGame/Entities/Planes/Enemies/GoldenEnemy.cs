namespace ShootingGame.Entities.Planes.Enemies
{
    class GoldenEnemy : Enemy
    {
        public GoldenEnemy(int x, int y, int level ) : base(x, y, /*r=*/10,  /*speed=*/15, Images.GOLDEN_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1, /*bulletRadius=*/Bullet.RADIUS_FOR_SMALL, /*maxBulletCooltime=*/200)
        {
        }

        protected override int GetEXP()
        {
            return 100;
        }

        protected override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet, Y, bulletRadius, Speed + 3, 180, 1, Id.ENEMY )
            ];
        
        }
    }
}
