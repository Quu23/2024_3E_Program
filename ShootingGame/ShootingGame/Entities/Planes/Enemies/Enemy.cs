using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    /// <summary>
    /// すべての敵クラスの基底クラス。
    /// </summary>
    abstract public class Enemy : Plane
    {
        public Enemy(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime) : base(x, y, radius, speed, img, level, hp, bulletRadius, maxBulletCoolTime)
        {
        }

        protected override void Move()
        {
            Y += Speed;
        }

        /// <summary>
        /// このEnemyを倒したときに獲得できるEXPを返す。
        /// </summary>
        /// <returns>このEnemyが持つEXP</returns>
        public abstract int GetEXP();
    }
}
