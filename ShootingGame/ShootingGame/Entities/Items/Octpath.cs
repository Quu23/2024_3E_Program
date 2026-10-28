using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{

    class Octputh : TransientItem
    {
        public Octputh(int x, int y) : base(x, y, 32, 10, Images.SPEED_UP_ITEM_IMAGE, StatusEffects.SUMI, 100)
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