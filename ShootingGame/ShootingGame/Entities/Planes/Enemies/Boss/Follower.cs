﻿using ShootingGame.Entities.Items;

namespace ShootingGame.Entities.Planes.Enemies.Boss
{
    class Follower : Enemy
    {
        public Enemy wrappedEnemy;

        public readonly int MAX_HP;


        public Follower(Enemy enemy,int MAX_HP) : base(enemy.X, enemy.Y, enemy.Radius, 0, enemy.Img, enemy.Level, 100, Bullet.RADIUS_FOR_SMALL, enemy.MaxBulletCoolTime)
        {
            wrappedEnemy = enemy;
            wrappedEnemy.Speed = 0;
            this.MAX_HP = MAX_HP;
        }

        public override void DeadAction(Player player, List<Enemy> enemies, List<Item> items)
        {
            base.DeadAction(player, enemies, items);
            wrappedEnemy.DeadAction(player, enemies, items);
        }

        public override void Action()
        {
            wrappedEnemy.Action();
        }

        protected override int GetEXP()
        {
            return 10;
        }

        protected override List<Bullet> ShotBullet()
        {
            throw new NotImplementedException();
        }
    }
}