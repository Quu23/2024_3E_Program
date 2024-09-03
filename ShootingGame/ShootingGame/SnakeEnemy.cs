﻿using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class SnakeEnemy : Enemy
    {
        public SnakeEnemy(int x, int y, int level) : base(x, y, 8, 10, new BitmapImage(ImageUris.SNAKE_ENEMY), level, 3, 10)
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
            X = (int)(100.0 * Math.Sin(2000*3.14 * Y))+50;
        }
    }
}
