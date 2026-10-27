namespace ShootingGame.Entities.Planes.Enemies
{
    class SampleEnemy : Enemy
    {
        // コンストラクタ. ここにSampleEnemyがインスタンス化されたときの処理をかく
        public SampleEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.STRAIGHT_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 + level, Bullet.RADIUS_FOR_MEDIUM, 0)
        {
        }

        protected override int GetEXP()
        {
            return Level * 0;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>();
            return bullets;
        }
    }
}
