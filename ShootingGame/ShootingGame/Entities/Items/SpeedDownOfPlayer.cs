using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    internal class SpeedDownOfPlayer : TransientItem
    {
        public SpeedDownOfPlayer(int x, int y, BitmapImage img) : base(x, y, 4, 6, img, 5)
        {
        }

        public override void CancelEffect(Player player)
        {
            player.Speed += 2;
        }

        public override void MakeEffect(Player player)
        {
            player.Speed -= 2;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
