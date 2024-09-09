using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class StraightEnemy : Enemy
    {
        public StraightEnemy(int x, int y, int level) : base(x, y, /*r=*/8, /*speed=*/1 + level, Images.STRAIGHT_ENEMY_IMAGE, /*LV=*/level, /*hp=*/5 + level , 50)
        {
        }

        public override int GetEXP()
        {
            return Level * 3;
        }

        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>
            {
                new(X, Y, 8, Level+5, 180, Level, Id.ENEMY)
            };
            return bullets;
        }
    }
}
