

using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class FastReat : Item
    {
        public FastReat(int x, int y,  BitmapImage img) : base(x, y, 3, 6, null)
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
