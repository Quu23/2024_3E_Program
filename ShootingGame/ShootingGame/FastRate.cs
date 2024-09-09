

using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class FastRate : Item
    {
        public FastRate(int x, int y,  BitmapImage img) : base(x, y, 4, 6, null)
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
