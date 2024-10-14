using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{

    class SpeedUpItem : TransientItem
    {
        public SpeedUpItem(int x, int y) : base(x, y, 16, 10, Images.SPEED_UP_ITEM_IMAGE, StatusEffects.SPEED_UP , 50)
        {
        }

        protected override void Effect(Player player)
        {
            player.defaultSpeed += 2;
            player.Speed = player.defaultSpeed;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}