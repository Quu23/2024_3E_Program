using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class SpeedDownItem : TransientItem
    {
        public SpeedDownItem(int x, int y, BitmapImage img) : base(x, y, 8, 6, img,  StatusEffects.SPEED_DOWN,5)
        {
        }

        protected override void Effect(Player player)
        {
            player.Speed -= 2;
        }

        public override void CancelEffect(Player player)
        {
            player.Speed += 2;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
