using System.Windows.Controls;

namespace ShootingGame
{
    class Plane : Entity
    {

        int level;

        public Plane(int x, int y, int radius, int speed, Image img) : base(x, y, radius, speed, img)
        {
        }
    }
}