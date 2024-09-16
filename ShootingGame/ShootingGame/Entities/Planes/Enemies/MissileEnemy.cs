using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    internal class MissileEnemy : Enemy
    {
        private int maxChaseTimer;
        private int chaseTimer;
        private int moveDirection;

        public MissileEnemy(int x, int y, int level) : base(x, y, /*r=*/20,  /*speed=*/3, null, /*LV=*/level, /*hp=*/2, /*bulletRadius=*/Bullet.RADIUS_FOR_SMALL, /*maxBulletCooltime=*/60)
        {
            maxChaseTimer = 10;
        }

        public override int GetEXP()
        {
            return Level * 3;
        }

        public override List<Bullet> ShotBullet()
        {
            return
            [
                new(CenterXForShotBullet, Y ,bulletRadius, Speed+5,moveDirection,1, Id.ENEMY )
            ];
        }
        public override void Move()
        {
            double dx = App.window.player.CenterX - CenterX;
            double dy = App.window.player.CenterY - CenterY;

            moveDirection = (int)(180 - (Math.Atan2(dx, dy) * (180 / Math.PI)));
        }
    }
}
