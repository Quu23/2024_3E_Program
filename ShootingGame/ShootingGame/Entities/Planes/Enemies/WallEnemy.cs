namespace ShootingGame.Entities.Planes.Enemies
{
    class WallEnemy : Enemy
    {
        public WallEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.WALL_ENEMY_IMAGE, /*LV=*/level, /*hp=*/99999999 + level, Bullet.RADIUS_FOR_MEDIUM, 0)
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
