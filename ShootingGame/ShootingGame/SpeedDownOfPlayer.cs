using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class SpeedDownOfPlayer : TransientItem
    {
        public SpeedDownOfPlayer(int x, int y, BitmapImage img, int EFFECT_IIME) : base(x, y, 4, 6, img, EFFECT_IIME)
        {
        }

        public override void CancelEffect(Player player)
        {
            player.Speed += 2;
        }

        public override void MakeEffect(Player player)
        {
            player.Speed -= 2;
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}
