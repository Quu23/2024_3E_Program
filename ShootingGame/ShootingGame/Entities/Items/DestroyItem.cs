using ShootingGame.Entities.Planes;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Items
{
    class DestroyItem : TransientItem
    {
        public DestroyItem(int x, int y) : base(x, y, 64, 10, null, StatusEffects.DESTROY_MODE, 240)
        {
        }

        protected override void Effect(Player player)
        {
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }
    }
}

