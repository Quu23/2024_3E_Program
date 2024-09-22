using ShootingGame.Entities.Planes;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Items
{
    class DestroyItem : TransientItem
    {
        public DestroyItem(int x, int y) : base(x, y, 16, 10, Images.DESTROY_ITEM_IMAGE, StatusEffects.DESTROY_MODE, 60)
        {
        }

        protected override void Effect(Player player)
        {
           
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}

