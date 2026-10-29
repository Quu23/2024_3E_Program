using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{

    class Octpath : TransientItem
    {
        public Octpath(int x, int y) : base(x, y, 32, 10, Images.OCTPATH_ITEM_IMAGE, StatusEffects.SUMI, 100)
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