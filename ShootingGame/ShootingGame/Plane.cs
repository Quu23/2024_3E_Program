using System.Windows.Controls;

namespace ShootingGame
{
    abstract class Plane : Entity
    {

        private int level;

        public Plane(int x, int y, int radius, int speed, Image img) : base(x, y, radius, speed, img)
        {
        }

        public int Level { get => level; set => level = value; }
    }
}