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


        public Plane(int x, int y, int radius, int speed, Image img, int level , int hp) : base(x, y, radius, speed, img)
        {
            Level = level;
            Hp = hp;
        }


        public int Level
        {
            get => level; private set
            {
                level = value;
            }
        }
        public int Hp { get => hp; set => hp = value; }

        /// <summary>
        /// �����̔�s�@�̒e�𐶐����郁�\�b�h�B
        /// </summary>
        /// <returns>��������Bullet�̃��X�g��Ԃ��B�����bullets��add����`�Ŏg���B</returns>
        public abstract List<Bullet> ShotBullet(); 
    }
}