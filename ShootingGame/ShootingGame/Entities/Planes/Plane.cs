using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes
{
    /// <summary>
    /// ���@��G�@�Ȃǂ��܂߂����ۓI�Ȑ퓬�@�N���X
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
            DecreaceBulletCoolTime = 1;
            this.bulletRadius = bulletRadius;
            MaxBulletCoolTime = maxBulletCoolTime;
        }

        /// <summary>
        /// �e���o���Ƃ��p�ɁA�e�̉摜�̃T�C�Y�Ƃ����l�������^�񒆂�X���W��Ԃ��B
        /// </summary>
        public int CenterXForShotBullet
        {
            get => X + Radius - bulletRadius;
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
        public int MaxBulletCoolTime { get => maxBulletCoolTime; set => maxBulletCoolTime = value; }
        public int DecreaceBulletCoolTime { get => decreaceBulletCoolTime; set => decreaceBulletCoolTime = value; }

        /// <summary>
        /// �����̔�s�@�̒e�𐶐����郁�\�b�h�B
        /// </summary>
        /// <returns>��������Bullet�̃��X�g��Ԃ��B�����bullets��add����`�Ŏg���B</returns>
        public abstract List<Bullet> ShotBullet();
    }
}