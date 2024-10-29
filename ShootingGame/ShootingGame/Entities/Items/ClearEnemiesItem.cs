using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies.Boss;

namespace ShootingGame.Entities.Items
{
    internal class ClearEnemiesItem : Item
    {
        public ClearEnemiesItem(int x, int y) : base(x, y, 16, 10, Images.CLEAR_ENEMIES_ITEM_IMAGE)
        {
        }

        public override void MakeEffect(Player player)
        {
            Boss b = null;

            if (App.window.enemies[0] is Boss)
            {
                b = (Boss)App.window.enemies[0];
            }
            App.window.enemies.Clear();

            if (b == null) return;

            b.followers.Clear();
            App.window.enemies.Add(b);
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}