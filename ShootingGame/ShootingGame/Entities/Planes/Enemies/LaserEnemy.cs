﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Planes.Enemies
{
    class LaserEnemy : Enemy
    {
        public LaserEnemy(int x, int y, int level) : base(x, y, 20, 1, Images.STRAIGHT_ENEMY_IMAGE, level, 1, Bullet.RADIUS_FOR_MEDIUM, 100)
        {
        }

        protected override int GetEXP()
        {
            return 100;
        }

        protected override List<Bullet> ShotBullet()
        {
            return new List<Bullet>()
            {
                new LaserBullet(CenterXForShotBullet,Y + Radius, LaserBullet.STRAIGHT_DEGREE,bulletRadius)
            };
        }
        protected override void Move()
        {
            X = X + Speed;
        }
    }

    class LaserBullet : Bullet
    {
        public static readonly int STRAIGHT_DEGREE  = 180;
        public static readonly int LEFT_SIDE_DEGREE = 270;
        public static readonly int RIGHT_SIDE_DEGREE = 90;

        public LaserBullet(int x, int y, int degree,int radius) : base(x, y, radius, 80, degree,10000000, EnemyTypes.LASER_ENEMY)
        {
            Rect tmp = Rect;

            tmp.Height = 1000;
            tmp.Width  = 8;

            if (degree == LEFT_SIDE_DEGREE || degree == RIGHT_SIDE_DEGREE)
            {

                tmp.Height = 8;
                tmp.Width  = 1000;

            }

            Rect = tmp;
        }
    }
}
