using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    internal class ClearEnemiesItem : Item
    {
        public ClearEnemiesItem(int x, int y) : base(x, y, 16, 10, Images.CLEAR_ENEMIES_ITEM_IMAGE)
        {
        }

        public override void MakeEffect(Player player)
        {
            App.window.enemies.Clear();
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}