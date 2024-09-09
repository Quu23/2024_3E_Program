//using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class ExpOrb : Item
    {
        public ExpOrb(int x, int y) : base(x, y, 2, 15, Images.EXP_ORB_IMAGE)
        {
        }

        public override void CancelEffect(Player player)
        {
            ;
        }

        public override void MakeEffect(Player player)
        {
            player.Exp += 50;
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}
