using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 効果時間がある、効果が一時的であるアイテムの基底クラス
    /// </summary>
    abstract class TransientItem : Item
    {
        /// <summary>
        /// アイテムの効果時間
        /// </summary>
        public readonly int EFFECT_IIME; 

        protected TransientItem(int x, int y, int radius, int speed, BitmapImage img, int EFFECT_IIME) : base(x, y, radius, speed, img)
        {
            this.EFFECT_IIME = EFFECT_IIME;
        }

        /// <summary>
        /// アイテムの効果を切る。効果時間を過ぎたら呼び出す。
        /// </summary>
        /// <param name="player"></param>
        public abstract void CancelEffect(Player player);
    }
}
