using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class SmallHealItem : Item
    {
        public SmallHealItem(int x, int y) : base(x, y, 16, 6, Images.SMALL_HEAL_ITEM_IMAGE)
        {

        }

        public override void MakeEffect(Player player)
        {
            if (player.Hp < player.MAX_HP - ( 2 + player.Level ))
            {
                player.Hp += ( 2 + player.Level );
            }
            else
            {
                player.HeelFullOfHp();
            }
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
