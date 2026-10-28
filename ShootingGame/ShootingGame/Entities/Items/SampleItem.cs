using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame.Entities.Items
{
    class SampleItem : TransientItem
    {
        // コンストラクタ. ここにSampleItemがインスタンス化されたときの処理をかく
        public SampleItem(int x, int y) : base(x, y, 16, 5, Images.OCTPATH_ITEM_IMAGE, StatusEffects.SUMI, 100)
        {
        }

        protected override void Effect(Player player)
        {
            
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
