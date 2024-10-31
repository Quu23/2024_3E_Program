using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class ExpOrb : Item
    {
        public ExpOrb(int x, int y) : base(x, y, 8, 10, Images.EXP_ORB_IMAGE)
        {
        }

        public override void MakeEffect(Player player)
        {
            player.Exp += 50;
            player.orbCount++;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
