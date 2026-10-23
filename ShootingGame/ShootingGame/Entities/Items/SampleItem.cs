using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame.Entities.Items
{
    class SampleItem : Item
    {
        // コンストラクタ. ここにSampleItemがインスタンス化されたときの処理をかく
        public SampleItem(int x, int y) : base(x, y, 16, 6, Images.HEALING_ITEM_IMAGE)
        {

        }

        public override void MakeEffect(Player player)
        {
            if (player.Hp < player.MAX_HP - 2)
            {
                player.Hp += 2;
            }
            else
            {
                player.HeelFullOfHp();
            }
        }

        protected override void Move()
        {
            Y += Speed;
        }

    }
}
