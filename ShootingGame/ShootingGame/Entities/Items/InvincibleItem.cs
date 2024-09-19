using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    internal class InvincibleItem : TransientItem
    {
        public InvincibleItem(int x, int y, int EFFECT_IIME) : base(x, y, 8, 6, Images.INVINCIBLE_ITEM_IMAGE, StatusEffects.INVINCIBLE, 5)
        {
        }

        protected override void Effect(Player player)
        {
            //無敵状態にする、というのはステータス上に何も影響を与えない。
            ;
        }

        public override void CancelEffect(Player player)
        {
            //ステータス上に何も影響を与えてないので、何もしなくてよい。
            ;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}



