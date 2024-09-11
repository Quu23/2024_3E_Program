using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class ClearEnemiesItem : Item
    {
        public ClearEnemiesItem(int x, int y) : base(x, y, 1, 10, null)
        {
        }

        public override void MakeEffect(Player player)
        {
            App.window.enemies.Clear();
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}