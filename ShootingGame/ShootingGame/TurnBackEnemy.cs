using System.Windows.Media.Imaging;

namespace ShootingGame
{
    class TurnBackEnemy : Enemy
    {
        public TurnBackEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/5, Images.TRUN_BACK_ENEMY_IMAGE, /*LV=*/level, /*hp=*/3, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUS,50)
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
                new(X ,Y ,bulletRadius, 10, 90, 1,Id.ENEMY),
                new(X ,Y ,bulletRadius, 10,270, 1,Id.ENEMY)
            ];
        }

        public override void Move()
        {
            base.Move();
            if (Y > 820) { Speed = -5; }
            if (Y > 750 && Speed > 2) 
            {   
                Speed = 2;
                
            }
        }
    }
}
