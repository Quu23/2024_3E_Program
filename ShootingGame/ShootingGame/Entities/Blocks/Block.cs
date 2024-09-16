using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Blocks
{
    /// <summary>
    ///     全ての障害物の基底クラス。
    /// </summary>
    abstract class Block : Entity
    {
        protected Block(int x, int y, int radius, int speed, BitmapImage img) : base(x, y, radius, speed, img)
        {
        }
    }
}
