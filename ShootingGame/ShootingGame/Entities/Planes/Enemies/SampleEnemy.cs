namespace ShootingGame.Entities.Planes.Enemies
{
    class SampleEnemy : Enemy
    {
        // コンストラクタ. ここにSampleEnemyがインスタンス化されたときの処理をかく
        public SampleEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.SAMPLE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 + level, Bullet.RADIUS_FOR_MEDIUM, 200)
        {
        }

        protected override int GetEXP()
        {
            return 500_0000; // 経験値は1000万に設定!!!!
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
