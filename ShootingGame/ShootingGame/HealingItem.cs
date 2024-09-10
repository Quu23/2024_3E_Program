using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class HealingItem : Item
    {
        public HealingItem(int x, int y, BitmapImage img) : base(x, y, 4, 6, Images.HEALING_ITEM_IMAGE)
        {
            throw new NotImplementedException("HealingItem's Image is null");
        }

        public override void MakeEffect(Player player)
        {
            player.Hp += 5;
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}
