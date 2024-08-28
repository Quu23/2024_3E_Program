using System.Windows.Controls;

namespace ShootingGame
{
    /// <summary>
    /// ���@��G�@�Ȃǂ��܂߂����ۓI�Ȑ퓬�@�N���X
    /// </summary>
    abstract public class Plane : Entity
    {

        private int level;
        private int hp;
        private int bulletCoolTime;
        private int maxBulletCoolTime;


        public Plane(int x, int y, int radius, int speed, Image img, int level , int hp ,int maxBulletCoolTime ) : base(x, y, radius, speed, img)
        {
            Level = level;
            Hp = hp;
            BulletCoolTime = 0;
            this.maxBulletCoolTime = maxBulletCoolTime;
        }


        public int Level
        {
            get => level; private set
            {
                level = value;
            }
        }
        public int Hp { get => hp; set => hp = value; }
        public int BulletCoolTime { get => bulletCoolTime; set => bulletCoolTime = value; }

        /// <summary>
        /// �����̔�s�@�̒e�𐶐����郁�\�b�h�B
        /// </summary>
        /// <returns>��������Bullet�̃��X�g��Ԃ��B�����bullets��add����`�Ŏg���B</returns>
        public abstract List<Bullet> ShotBullet(); 
    }
}