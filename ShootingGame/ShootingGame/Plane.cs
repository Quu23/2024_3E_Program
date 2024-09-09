using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 自機や敵機などを含めた抽象的な戦闘機クラス
    /// </summary>
    abstract public class Plane : Entity
    {

        private int level;
        private int hp;
        private int bulletCoolTime;
        private int decreaceBulletCoolTime;
        private int maxBulletCoolTime;


        public Plane(int x, int y, int radius, int speed, BitmapImage img, int level , int hp ,int maxBulletCoolTime ) : base(x, y, radius, speed, img)
        {
            Level = level;
            Hp = hp;
            BulletCoolTime = 0;
            DecreaceBulletCoolTime = 1;
            this.MaxBulletCoolTime = maxBulletCoolTime;
        }


        public int Level
        {
            get => level; protected set
            {
                level = value;
            }
        }
        public int Hp { get => hp; set => hp = value; }
        public int BulletCoolTime { get => bulletCoolTime; set => bulletCoolTime = value; }
        public int MaxBulletCoolTime { get => maxBulletCoolTime;  set => maxBulletCoolTime = value; }
        public int DecreaceBulletCoolTime { get => decreaceBulletCoolTime; set => decreaceBulletCoolTime = value; }

        /// <summary>
        /// 自分の飛行機の弾を生成するメソッド。
        /// </summary>
        /// <returns>生成したBulletのリストを返す。これをbulletsにaddする形で使う。</returns>
        public abstract List<Bullet> ShotBullet(); 
    }
}