
using ShootingGame.Entities.Items;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    abstract class Boss : Enemy
    {
        public readonly int MAX_HP;

        private int width;
        private int height;

        public List<Follower> followers;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public new int CenterX { get => X + Width / 2; }
        public new int CenterY { get => Y + Height / 2; }

        public new int CenterXForShotBullet {  get => X + Width / 2 - bulletRadius; }

        /// <summary>
        /// BossのHPはfollowerのHPも含まれている。逆に考えれば、followerのHPが減るとき、BOSSのHPも減らさなければならない。
        /// </summary>
        public Boss(int x, int y, int width, int height,int speed, BitmapImage img, int MAX_HP,int bulletRadius, int maxBulletCoolTime, List<Follower> followers) : base(x, y, (int)(Math.Sqrt(width * width + height * height)/2), speed, img, 1, MAX_HP, bulletRadius, maxBulletCoolTime)
        {
            if (height > width) throw new ArgumentException("BOSSの横半径は縦半径より長くなる必要があります。");

            this.width = width;
            this.height = height;

            Rect = new Rect(X, Y, width, height);

            this.followers = followers;
            foreach (Follower f in followers)
            {
                Hp += f.Hp;
            }

            this.MAX_HP = Hp;
        }

        public List<Enemy> GetFollowers()
        {
            List<Enemy> list = new List<Enemy>();
            foreach (Follower f in followers)
            {
                list.Add(f);
            }
            return list;
        }


        public override void DeadAction(Player player, List<Enemy> enemies, List<Item> items)
        {
            base.DeadAction(player, enemies, items);
            foreach (Follower follower in followers)
            {
                enemies.Remove(follower);
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
                double ry = rh * Math.Sqrt(1 - basis * basis / (rw * rw));

                double cxr = CenterX + basis;

                if ((cxr - target.CenterX) *  (cxr - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                double cxl = CenterX - basis;

                if ((cxl - target.CenterX) * (cxl - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                basis += ry;
            }
            return false;
        }

        public override bool IsOutOfWindow()
        {
            if (X < App.window.moveableLeftSidePosition - Width || X > App.window.moveableRightSidePosition || Y < -Height || Y > SystemParameters.PrimaryScreenHeight) return true;
            return false;
        }
    }
}
