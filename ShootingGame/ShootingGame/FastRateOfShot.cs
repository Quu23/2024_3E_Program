

using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class FastRateOfShot : TransientItem
    {
        public FastRateOfShot(int x, int y) : base(x, y, 4, 6, Images.FAST_RATE_OF_SHOT_IMAGE,10)
        {
        }

        public override void MakeEffect(Player player)
        {
            player.MaxBulletCoolTime /= 2;
        }
        public override void CancelEffect(Player player)
        {
            player.MaxBulletCoolTime *= 2;
        }
        public override void Move()
        {
            Y+=Speed;
        }
    }
}
