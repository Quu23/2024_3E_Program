using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    internal class CycloneEnemy : Enemy
    {
        private int degree;

        public CycloneEnemy(int x, int y, int level) : base(x, y, /*r=*/16, /*speed=*/3, null, level, level * 3, Bullet.RADIUS_FOR_MEDIUM, 200)
        {
            degree = 0;

        }

        protected override int GetEXP()
        {
            return Level * 3;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>()
            {
                new(CenterXForShotBullet, Y, bulletRadius, Speed+10,   0, 1, Id.ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+10,  72, 1, Id.ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+10, 144, 1, Id.ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+10, 216, 1, Id.ENEMY),
                new(CenterXForShotBullet, Y, bulletRadius, Speed+10, 288, 1, Id.ENEMY),
            };
            
            return bullets;

        }
        protected override void Move()
        {
            Y -= (int)(Speed * Math.Pow(2, degree * Math.PI / 180) * Math.Cos(degree * Math.PI / 180));
            X += (int)(Speed * Math.Pow(2, degree * Math.PI / 180) * Math.Sin(degree * Math.PI / 180));
            ChangeRect(X, Y);
        }
    }
}
