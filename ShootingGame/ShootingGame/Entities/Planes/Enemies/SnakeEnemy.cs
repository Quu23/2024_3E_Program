namespace ShootingGame.Entities.Planes.Enemies
{
    class SnakeEnemy : Enemy
    {
        public SnakeEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/5, Images.SNAKE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 50)
        {
        }

        public override int GetEXP()
        {
            return Level + 5;
        }

        public override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet ,Y ,8, 10,180, 1 ,Id.ENEMY)
            ];
        }

        public override void Move()
        {
            base.Move();
            X = (int)(10 * Speed * Math.Sin(Math.PI / 216 * Y)) + 60;
        }
    }
}
