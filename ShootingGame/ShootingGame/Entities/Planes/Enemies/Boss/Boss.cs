
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    abstract class Boss : Enemy
    {

        private int width;
        private int height;

        protected List<Enemy> followers;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public new int CenterX { get => X + Width / 2; }
        public new int CenterY { get => Y + Height / 2; }

        /// <summary>
        /// BossのHPはfollowerのHPも含まれている。逆に考えれば、followerのHPが減るとき、BOSSのHPも減らさなければならない。
        /// </summary>
        public Boss(int x, int y, int width, int height,int speed, BitmapImage img, int hp, int bulletRadius, int maxBulletCoolTime, List<Enemy> followers) : base(x, y, radius, speed, img, 1, hp, bulletRadius, maxBulletCoolTime)
        {
            if (height > width) throw new ArgumentException("BOSSの横半径は縦半径より長くなる必要があります。");

            this.width = width;
            this.height = height;

            Rect = new Rect(X, Y, width, height);

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

        //円と楕円の当たり判定になるので楕円を「内包している複数の円」で近似する。
        public override bool IsHit(Entity target)
        {
            double basis = 0;

            double rw = Width / 2;
            double rh = Height / 2;

            while (basis < rw)
            {
                double ry = rw * Math.Sqrt(1 - basis * basis / (rh * rh));

                double cxr = CenterX + basis;

                if ((cxr - target.CenterX) *  (cxr - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                double cxl = CenterX - basis;

                if ((cxl - target.CenterX) * (cxl - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                basis += ry;
            }
            return false;
        }
    }
}
