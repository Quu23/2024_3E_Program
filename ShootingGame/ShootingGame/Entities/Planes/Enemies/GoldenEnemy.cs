namespace ShootingGame.Entities.Planes.Enemies
{
    internal class GoldenEnemy : Enemy
    {
        public GoldenEnemy(int x, int y, int level ) : base(x, y, /*r=*/10,  /*speed=*/10, null, /*LV=*/level, /*hp=*/1, /*bulletRadius=*/Bullet.RADIUS_FOR_SMALL, /*maxBulletCooltime=*/200)
        {
        }

        public override int GetEXP()
        {
            return 100;
        }

        public override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet, Y, bulletRadius, Speed + 3, 180, 1, Id.ENEMY )
            ];
        
        }
    }
}
