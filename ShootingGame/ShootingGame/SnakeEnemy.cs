using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class SnakeEnemy : Enemy
    {
        public SnakeEnemy(int x, int y, int level) : base(x, y, 8, 5, new BitmapImage(ImageUris.SNAKE_ENEMY), level, 3, 50)
        {
        }

        public override int GetEXP()
        {
            return Level + 5;
        }

        public override List<Bullet> ShotBullet()
        {
            return
            [
                new(X ,Y ,8, 10,180, 1 ,Id.ENEMY)
            ];
        }

        public override void Move()
        {
            base.Move();
            X = (int)(10 * Speed * Math.Sin(Math.PI / 216 * Y))+60;
        }
    }
}
