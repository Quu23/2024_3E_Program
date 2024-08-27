﻿using System.Windows.Controls;

namespace ShootingGame
{
    /// <summary>
    /// 全ての当たり判定を持つオブジェクトの親クラス。当たり判定を簡単にするために、円で近似している。
    /// </summary>
    public class Entity
    {

        int x;
        int y;
        int radius;
        int speed;
        Image img;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Radius { get => radius; set => radius = value; }
        public int Speed { get => speed; set => speed = value; }
        public Image Img { get => img;}

        public Entity(int x, int y, int radius, int speed, Image img)
        {
            X = x;
            Y = y;
            Radius = radius;
            Speed = speed;
            this.img = img;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())return false;
            
            Entity e = (Entity)obj;
            if( e.X != X || e.Y != Y || e.Radius != Radius || e.Speed != Speed)return false;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y , Radius , Speed);
        }

        public bool IsHit(Entity target)
        {
            if ((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y) < ( Radius + target.Radius) * (Radius + target.Radius))
            {
                return true;
            }
            return false;
        }
    }
}


