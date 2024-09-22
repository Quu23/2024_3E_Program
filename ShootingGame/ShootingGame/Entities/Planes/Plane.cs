using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes
{
    /// <summary>
    /// 自機や敵機などを含めた抽象的な戦闘機クラス
    /// </summary>
    abstract public class Plane : Entity
    {

        private int level;
        private int hp;

        protected int bulletRadius;

        private int bulletCoolTime;
        private int decreaceBulletCoolTime;
        private int maxBulletCoolTime;


        public Plane(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime) : base(x, y, radius, speed, img)
        {
            Level = level;
            Hp = hp;
            BulletCoolTime = 0;
            DecreaceBulletCoolTime = 3;
            this.bulletRadius = bulletRadius;
            MaxBulletCoolTime = maxBulletCoolTime;
        }

        /// <summary>
        /// 弾を出すとき用に、弾の画像のサイズとかを考慮した真ん中のX座標を返す。
        /// </summary>
        public int CenterXForShotBullet
        {
            get => CenterX - bulletRadius;
        }


        public int Level
        {
            get => level; protected set
            {
                level = value;
            }
        }
        public virtual int Hp { get => hp; set => hp = value; }
        public int BulletCoolTime { get => bulletCoolTime; set => bulletCoolTime = value; }
        public int MaxBulletCoolTime { get => maxBulletCoolTime; set => maxBulletCoolTime = value; }
        public int DecreaceBulletCoolTime { get => decreaceBulletCoolTime; set => decreaceBulletCoolTime = value; }

        public override void Action()
        {
            if (BulletCoolTime > 0) BulletCoolTime -= DecreaceBulletCoolTime;
            base.Action();
            if (BulletCoolTime <= 0)
            {
                App.window.bullets.AddRange(ShotBullet());
                BulletCoolTime = MaxBulletCoolTime;
            }
        }

        public virtual bool IsHit(Entity target)
        {
            if ((CenterX - target.CenterX) * (CenterX - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < (Radius + target.Radius) * (Radius + target.Radius))
                return true;
            return false;
        }

        /// <summary>
        /// 自分の飛行機の弾を生成するメソッド。
        /// </summary>
        /// <returns>生成したBulletのリストを返す。これをbulletsにaddする形で使う。</returns>
        protected abstract List<Bullet> ShotBullet();
    }
}