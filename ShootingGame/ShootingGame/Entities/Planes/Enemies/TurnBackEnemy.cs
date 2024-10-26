using System.Windows;

namespace ShootingGame.Entities.Planes.Enemies
{
    class TurnBackEnemy : Enemy
    {
        public TurnBackEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/5, Images.TRUN_BACK_ENEMY_IMAGE, /*LV=*/level, /*hp=*/2 + level, /*bulletRadius=*/Bullet.RADIUS_FOR_MEDIUM, 70)
        {
        }

        protected override int GetEXP()
        {
            return Level + 5;
        }

        protected override List<Bullet> ShotBullet()
        {
            return
            [
                new(X ,Y ,bulletRadius, 10, 90, 1,EnemyTypes.TURNBACK_ENEMY),
                new(X ,Y ,bulletRadius, 10,270, 1,EnemyTypes.TURNBACK_ENEMY)
            ];
        }

        protected override void Move()
        {
            base.Move();
            
            double displayHeight = SystemParameters.PrimaryScreenHeight;

            if (Y > displayHeight * 0.7 && Speed > 2)
            {
                Speed = 2;
            }
            else if (Y > displayHeight * 0.8)
            { 
                Speed = -5; 
            } 
        }
    }
}
