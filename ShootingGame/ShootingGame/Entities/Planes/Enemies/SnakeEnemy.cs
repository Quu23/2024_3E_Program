namespace ShootingGame.Entities.Planes.Enemies
{
    class SnakeEnemy : Enemy
    {
        //sinカーブの軸(y軸)の座標
        private readonly int basicX;

        public SnakeEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/5, Images.SNAKE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 +level, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 80)
        {
            basicX = x;
        }

        protected override int GetEXP()
        {
            return Level + 5;
        }

        protected override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet ,Y ,Bullet.RADIUS_FOR_MEDIUM, 10,180, Level ,EnemyTypes.SNAKE_ENEMY)
            ];
        }

        protected override void Move()
        {
            base.Move();
            X = (int)(20 * Speed * Math.Sin(Math.PI / 216 * Y)) + basicX;
        }
    }
}
