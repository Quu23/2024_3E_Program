using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    internal class InvincibleItem : TransientItem
    {
        public InvincibleItem(int x, int y, int EFFECT_IIME) : base(x, y, 2, 6, null, 5)
        {
        }

        public override void CancelEffect(Player player)
        {
            player.isInvincible = false;
        }

        public override void MakeEffect(Player player)
        {
            player.isInvincible = true;
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}



