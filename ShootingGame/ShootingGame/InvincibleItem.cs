using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class InvincibleItem : TransientItem
    {
        public InvincibleItem(int x, int y,  int EFFECT_IIME) : base(x, y, 2, 6,null, 5)
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
            Y += Speed;
   }
}
