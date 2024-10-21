
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    abstract class Boss : Enemy
    {

        protected List<Enemy> followers;

        public Boss(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime, List<Enemy> followers) : base(x, y, radius, speed, img, level, hp, bulletRadius, maxBulletCoolTime)
        {
            this.followers = followers;
            foreach (Enemy enemy in followers)
            {
                hp += enemy.Hp;
            }
        }

        public override void Action()
        {
            base.Action();

            foreach (Enemy enemy in followers)
            {
                enemy.Action();
            }
        }
    }
}
