using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class InvincibleItem : TransientItem
    {
        public InvincibleItem(int x, int y) : base(x, y, 8, 6, Images.INVINCIBLE_ITEM_IMAGE, StatusEffects.INVINCIBLE, 100)
        {
        }

        protected override void Effect(Player player)
        {
            //無敵状態にする、というのはステータス上に何も影響を与えない。
            ;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}



