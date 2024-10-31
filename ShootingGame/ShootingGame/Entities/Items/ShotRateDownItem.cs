using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class ShotRateDownItem : TransientItem
    {
        public ShotRateDownItem(int x, int y) : base(x, y, 16, 6, Images.SHOT_RATE_DOWN_ITEM_IMAGE, StatusEffects.SHOT_RATE_DOWN, 50)
        {
        }

        protected override void Effect(Player player)
        {
            player.DecreaceBulletCoolTime = 0;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
