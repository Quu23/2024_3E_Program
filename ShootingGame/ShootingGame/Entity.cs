namespace ShootingGame
{
    /// <summary>
    /// 全ての当たり判定を持つオブジェクトの親クラス。当たり判定を簡単にするために、円で近似している。
    /// </summary>
    public class Entity
    {

        int x, y;
        int radius;
        int speed;

        public Entity(int x, int y, int radius, int speed)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.speed = speed;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())return false;
            
            Entity e = (Entity)obj;
            if( e.x != x || e.y != y || e.radius != radius || e.speed != speed)return false;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y , radius , speed);
        }

        public bool IsHit(Entity target)
        {
            if ((x - target.x) * (x - target.x) + (y - target.y) * (y - target.y) < ( radius + target.radius) * (radius + target.radius))
            {
                return true;
            }
            return false;
        }
    }
}


