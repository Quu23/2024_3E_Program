﻿using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities
{
    /// <summary>
    /// 全ての当たり判定を持つオブジェクトの親クラス。当たり判定を簡単にするために、円で近似している。
    /// </summary>
    abstract public class Entity
    {
        private int x;
        private int y;
        private int radius;
        private int speed;
        private BitmapImage img;
        private Rect rect;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Radius { get => radius; set => radius = value; }
        public int CenterX { get => (X + Radius); }
        public int CenterY { get => (Y + Radius); }
        public int Speed { get => speed; set => speed = value; }
        public BitmapImage Img { get => img; set => img = value; }
        public Rect Rect { get => rect; set => rect = value; }

        public Entity(int x, int y, int radius, int speed, BitmapImage img)
        {
            X = x;
            Y = y;
            Radius = radius;
            Speed = speed;
            Rect = new Rect(X, Y, 2*Radius , 2*Radius);
            Img = img;
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

        public virtual bool IsHit(Entity target)
        {
            if ((CenterX - target.CenterX) * (CenterX - target.CenterX) + (CenterY - target.CenterY) * (CenterY - target.CenterY) < ( Radius + target.Radius) * (Radius + target.Radius))
                return true;
            return false;
        }

        public abstract void Move();

        protected void ChangeRect(int x, int y)
        {
            //構造体の中身をインデクサとかプロパティ経由で変更する際は、一時的に別の変数に移してそれを変更してから再代入しないといけない。
            //構造体をインデクサとかプロパティで取得した場合は、構造体自身ではなくそのコピー（値）が得られるため。
            Rect tmp = Rect;
            tmp.X = x;
            tmp.Y = y;
            Rect = tmp;
        }
    }
}

