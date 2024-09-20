using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
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

        private readonly StatusEffects EFFECT_KIND;

        protected TransientItem(int x, int y, int radius, int speed, BitmapImage img, StatusEffects EFFECT_KIND, int EFFECT_IIME) : base(x, y, radius, speed, img)
        {
            this.EFFECT_IIME = EFFECT_IIME;
            this.EFFECT_KIND = EFFECT_KIND;
        }

        public override sealed void MakeEffect(Player player)
        {
            Effect(player);
            player.status[EFFECT_KIND] += EFFECT_IIME;

        }

        /// <summary>
        /// 持続効果を表す。例えば、スピードを上げるならSpeed += 1;など。
        /// </summary>
        /// <param name="player"></param>
        protected abstract void Effect(Player player);
    }
}
