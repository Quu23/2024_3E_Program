

using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class HealingItem : Item
    {
        public HealingItem(int x, int y, BitmapImage img) : base(x, y, 3, 6, null)
        {
        }

        public override void MakeEffect(Player player)
        {
            player.HeelFullOfHp();
        }

        public override void CancelEffect(Player player)
        {
            ;
        }


        public override void Move()
        {
            Y += Speed;
        }
    }
}
