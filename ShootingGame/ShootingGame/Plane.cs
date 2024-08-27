using System.Windows.Controls;

namespace ShootingGame
{
    /// <summary>
    /// ���@��G�@�Ȃǂ��܂߂����ۓI�Ȑ퓬�@�N���X
    /// </summary>
    abstract public class Plane : Entity
    {

        private int level;

        public Plane(int x, int y, int radius, int speed, Image img, int level) : base(x, y, radius, speed, img)
        {
            Level = level;
        }

        public int Level { get => level; set => level = value; }

        /// <summary>
        /// �����̔�s�@�̒e�𐶐����郁�\�b�h�B
        /// </summary>
        /// <returns>��������Bullet�̃��X�g��Ԃ��B�����bullets��add����`�Ŏg���B</returns>
        public abstract List<Bullet> ShotBullet(); 
    }
}