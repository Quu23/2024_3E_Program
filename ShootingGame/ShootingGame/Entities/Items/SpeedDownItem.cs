using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class SpeedDownItem : TransientItem
    {
        public SpeedDownItem(int x, int y) : base(x, y, 8, 6, Images.SPEED_DOWN_ITEM_IMAGE,  StatusEffects.SPEED_DOWN,5)
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
