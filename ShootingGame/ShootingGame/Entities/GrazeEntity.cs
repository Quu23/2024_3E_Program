using ShootingGame.Entities.Planes;
using ShootingGame.Entities.Planes.Enemies.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities
{
    // Playerの周囲のかすり判定を管理するためPlane. プレイヤーの中心に追従する。
    public class GrazeEntity : Plane
    {
        public Player player;

        private int width;
        private int height;

        public int Width { get => width; }
        public int Height { get => height; }

        public GrazeEntity(int x, int y, Player playerForGrazeEntity) : base(x, y, 25, 0, null, 0, 0, 0, 0)
        {
            player = playerForGrazeEntity;

            this.width = 2 * player.Radius + 20;
            this.height = 2 * player.Radius - 30;

            Rect = new Rect(X, Y, width, height);

        }

        protected override void Move()
        {
            X = player.CenterX - Radius;
            Y = player.CenterY - Radius;
        }

        //円と楕円の当たり判定になるので楕円を「内包している複数の円」で近似する。 BOSSと同じ処理.
        public override bool IsHit(Entity target)
        {
            double basis = 0;

            double rw = Width / 2;
            double rh = Height / 2;

            while (basis < rw)
            {
                double ry = rh * Math.Sqrt(1 - basis * basis / (rw * rw));

                double cxr = CenterX + basis;

                if ((cxr - target.CenterX) * (cxr - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                double cxl = CenterX - basis;

                if ((cxl - target.CenterX) * (cxl - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (ry + target.Radius) * (ry + target.Radius)) return true;

                basis += ry;
            }
            return false;
        }

        protected override List<Bullet> ShotBullet()
        {
            return new List<Bullet>();
        }
    }
}
