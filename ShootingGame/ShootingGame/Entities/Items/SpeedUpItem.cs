using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{

    class SpeedUpItem : Item
    {
        public SpeedUpItem(int x, int y) : base(x, y, 8, 10, Images.SPEED_UP_ITEM_IMAGE)
        {
        }

        public override void MakeEffect(Player player)
        {
            player.defaultSpeed += 2;
            player.Speed = player.defaultSpeed;
        }

        protected override void Move()
        {
            X = 5;
            Y += Speed;
        }
    }
}