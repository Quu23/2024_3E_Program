namespace ShootingGame.Entities.Planes.Enemies
{
    class SnakeEnemy : Enemy
    {
        public SnakeEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/5, Images.SNAKE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/2 + level, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 80)
        {
        }

        protected override int GetEXP()
        {
            return Level + 5;
        }

        protected override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet ,Y ,Bullet.RADIUS_FOR_MEDIUM, 10,180, 1 ,Id.ENEMY)
            ];
        }

        protected override void Move()
        {
            base.Move();
            X = (int)(20 * Speed * Math.Sin(Math.PI / 216 * Y)) + 60;
        }
    }
}
