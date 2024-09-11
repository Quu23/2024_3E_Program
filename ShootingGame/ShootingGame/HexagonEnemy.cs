using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class HexagonEnemy : Enemy
    {
        public HexagonEnemy(int x, int y, int level) : base(x, y, /*r=8*/8, /*speed=*/level + 5, null, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM,70)
        {
            throw new NotImplementedException("Image is null!!");
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
                // todo:X,Yの微調整
                new(X, Y, 2, bulletRadius,   0, 1, Id.ENEMY),
                new(X, Y, 2, bulletRadius,  60, 1, Id.ENEMY),
                new(X, Y, 2, bulletRadius, 120, 1, Id.ENEMY),
                new(X, Y, 2, bulletRadius, 180, 1, Id.ENEMY),
                new(X, Y, 2, bulletRadius, 240, 1, Id.ENEMY),
                new(X, Y, 2, bulletRadius, 300, 1, Id.ENEMY)
            };

            return bullets;

        }
    }
}