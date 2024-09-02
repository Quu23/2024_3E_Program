using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class SnakeEnemy : Enemy
    {
        public SnakeEnemy(int x, int y, int level, int hp) : base(x, y, 4, 1, new BitmapImage(), level, 3, 10)
        {
        }

        public override int GetEXP()
        {
            return 5;
        }

        public override List<Bullet> ShotBullet()
        {
            return
            [
                new(X ,Y ,1, 1,180, 1 ,Id.ENEMY)
            ];
        }

        public override void Move()
        {
            base.Move();
            X += (int)(Speed * Math.Sin(Y));
        }
    }
}
