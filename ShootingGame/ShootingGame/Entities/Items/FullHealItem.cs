using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class FullHealItem : Item
    {
        public FullHealItem(int x, int y) : base(x, y, 16, 6, Images.HEALING_ITEM_IMAGE)
        {

        }

        public override void MakeEffect(Player player)
        {
            player.HeelFullOfHp();
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
