using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    /// <summary>
    /// すべての敵クラスの基底クラス。
    /// </summary>
    public abstract class Enemy : Plane
    {
        public Enemy(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime) : base(x, y, radius, speed, img, level, hp, bulletRadius, maxBulletCoolTime)
        {
        }

        protected override void Move()
        {
            Y += Speed;
        }

        /// <summary>
        /// このEnemyを倒したときのアクションを表す。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemies"></param>
        public virtual void DeadAction(Player player,List<Enemy> enemies)
        {
            player.Exp += GetEXP();
            enemies.Remove(this);
        }

        /// <summary>
        /// このEnemyを倒したときに獲得できるEXPを返す。
        /// </summary>
        /// <returns>このEnemyが持つEXP</returns>
        protected abstract int GetEXP();
    }
}
