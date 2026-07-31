using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame.Entities.Items
{
    class SampleItem : Item
    {
        public SampleItem(int x, int y) : base(x, y, 16, 7, Images.SAMPLE_ITEM_IMAGE)
        {
        }

        public override void MakeEffect(Player player)
        {
            var temp = new List<Enemy>(); 
            foreach (var enemy in App.window.enemies)
            {
                temp.Add(new MissileEnemy(enemy.X, enemy.Y, 1));
            }
            App.window.enemies.AddRange(temp);
        }

        protected override void Move()
        {
            Y += Speed;
        }

    }
}
