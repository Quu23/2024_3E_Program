using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// すべての敵クラスの基底クラス。
    /// </summary>
    abstract public class Enemy : Plane
    {
        public bool isDead;

        public Enemy(int x, int y, int radius, int speed, BitmapImage img, int level ,int hp ,int maxBulletCoolTime) : base(x, y, radius, speed, img, level ,hp ,maxBulletCoolTime)
        {
            isDead = false;
        }

        public override void Move()
        {
            Y += Speed;
            ChangeRect(X, Y);
        }

        /// <summary>
        /// このEnemyを倒したときに獲得できるEXPを返す。
        /// </summary>
        /// <returns>このEnemyが持つEXP</returns>
        public abstract int GetEXP();
    }
}
