using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame.Entities.Items
{
    class SampleItem : Item
    {
        // コンストラクタ. ここにSampleItemがインスタンス化されたときの処理をかく
        public SampleItem(int x, int y) : base(x, y, 16, 7, Images.SAMPLE_ITEM_IMAGE)
        {
        }

        // アイテムを取った時の効果を書く
        public override void MakeEffect(Player player)
        {
            // ここでは敵を全てミサイルを撃つ敵に変える処理を書く


            var temp = new List<Enemy>(); 
            foreach (var enemy in App.window.enemies)
            {
                temp.Add(new MissileEnemy(enemy.X, enemy.Y, 1));
            }
            App.window.enemies.AddRange(temp);
        }

        // アイテムの移動処理を書く
        protected override void Move()
        {
            // ここでは単純に下に落ちるだけの処理
            Y += Speed;
        }

    }
}
