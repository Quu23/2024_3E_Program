using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class TurnBackEnemy : Enemy
    {
        public TurnBackEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/20, null, /*LV=*/level, /*hp=*/3, 50)
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
                new(X ,Y ,8, 10,90, 1 ,Id.ENEMY),
                new(X ,Y ,8, 10,270, 1 ,Id.ENEMY)
            ];
        }

        public override void Move()
        {
            base.Move();
           Y =  (Y - 1 / Y * Y);
            if (Y < 3)
            {
                Y = -20;
                Y = -(Y - 1 / Y * Y);
            }
        }
    }
}
