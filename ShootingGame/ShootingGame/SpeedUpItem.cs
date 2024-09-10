using System.Windows.Media.Imaging;

namespace ShootingGame
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

        public override void Move()
        {
            X = 5;
            Y += 10;
        }
    }
}