using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    /// <summary>
    /// ゲーム中の全てのアイテムの基底クラス。
    /// </summary>
    public abstract class Item : Entity
    {
        protected Item(int x, int y, int radius, int speed, BitmapImage img) : base(x, y, radius, speed, img)
        {
        }

        /// <summary>
        /// そのアイテムの効果。
        /// </summary>
        /// <param name="target">アイテムが作用する機体</param>
        public abstract void MakeEffect(Player player);
    }
}
