using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class SpeedDownItem : TransientItem
    {
        public SpeedDownItem(int x, int y) : base(x, y, 16, 6, Images.SPEED_DOWN_ITEM_IMAGE,  StatusEffects.SPEED_DOWN, 100)
        {
        }

        protected override void Effect(Player player)
        {
            // Speedが負になることもあるけど、その場合上下左右のキーが逆になるからむしろ面白いかも。
            player.defaultSpeed -= 2;
            player.Speed = player.defaultSpeed;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
