using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class InvincibleItem : TransientItem
    {
        public InvincibleItem(int x, int y, int radius, int speed, BitmapImage img, int EFFECT_IIME) : base(x, y, radius, speed, img, EFFECT_IIME)
        {
        }

        public override void CancelEffect(Player player)
        {
            throw new NotImplementedException();
        }

        public override void MakeEffect(Player player)
        {
            throw new NotImplementedException();
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
