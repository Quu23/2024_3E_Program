﻿using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class ShotRateUpItem : TransientItem
    {
        public ShotRateUpItem(int x, int y) : base(x, y, 8, 6, Images.SHOT_RATE_UP_ITEM_IMAGE, StatusEffects.SHOT_RATE_UP,10)
        {
        }

        protected override void Effect(Player player)
        {
            player.DecreaceBulletCoolTime *= 2;
        }
        public override void CancelEffect(Player player)
        {
            player.DecreaceBulletCoolTime /= 2;
        }
        protected override void Move()
        {
            Y += Speed;
        }
    }
}
