using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{

    class SpeedUpItem : Item
    {
        public SpeedUpItem(int x, int y) : base(x, y, 4, 10, null)
        {
        }

        public override void MakeEffect(Player player)
        {
            player.defaultSpeed += 5;
            player.Speed = player.defaultSpeed;
        }

        protected override void Move()
        {
            X = 5;
            Y += Speed;
        }
    }
}