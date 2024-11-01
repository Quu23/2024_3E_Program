using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class ShotRateUpItem : TransientItem
    {
        public ShotRateUpItem(int x, int y) : base(x, y, 16, 6, Images.SHOT_RATE_UP_ITEM_IMAGE, StatusEffects.SHOT_RATE_UP, 100)
        {
        }

        protected override void Effect(Player player)
        {
            if(player.DecreaceBulletCoolTime< player.normalStatus[3] * 8)
            {
                player.DecreaceBulletCoolTime *= 2;
            }
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
