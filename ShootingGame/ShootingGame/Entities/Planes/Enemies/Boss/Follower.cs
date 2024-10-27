namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    class Follower : Enemy
    {
        private Enemy wrappedEnemy;

        public readonly int MAX_HP;

        public Follower(Enemy enemy) : base(enemy.X, enemy.Y, enemy.Radius, 0, enemy.Img, enemy.Level, 100, Bullet.RADIUS_FOR_SMALL, enemy.MaxBulletCoolTime)
        {
            wrappedEnemy = enemy;
            wrappedEnemy.Speed = 0;
            MAX_HP = 100;
        }

        public override void Action()
        {
            wrappedEnemy.Action();
        }

        protected override int GetEXP()
        {
            return 10;
        }

        protected override List<Bullet> ShotBullet()
        {
            throw new NotImplementedException();
        }
    }
}
