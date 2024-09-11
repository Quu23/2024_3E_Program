using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class HexagonEnemy : Enemy
    {
        public HexagonEnemy(int x, int y, int level) : base(x, y, /*r=8*/8, /*speed=*/level + 5, null, /*LV=*/level, /*hp=*/3, 70)
        {
        }

        public override int GetEXP()
        {
            return Level * 2;
        }


        /// <summary>
        /// 放射状に六つの弾を発生させる。進行方向とそこから±60deg方向
        /// </summary>
        public override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new(X, Y, 2, 8,   0, 1, Id.ENEMY),
                new(X, Y, 2, 8,  60, 1, Id.ENEMY),
                new(X, Y, 2, 8, 120, 1, Id.ENEMY),
                new(X, Y, 2, 8, 180, 1, Id.ENEMY),
                new(X, Y, 2, 8, 240, 1, Id.ENEMY),
                new(X, Y, 2, 8, 300, 1, Id.ENEMY)
            };

            return bullets;

        }
    }
}