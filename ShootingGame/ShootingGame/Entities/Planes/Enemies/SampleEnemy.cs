using ShootingGame.Entities.Items;
using System.Diagnostics;

namespace ShootingGame.Entities.Planes.Enemies
{
    class SampleEnemy : Enemy
    {
        // コンストラクタ. ここにSampleEnemyがインスタンス化されたときの処理をかく

        private int count = 3;
        private readonly int basicY;
        public SampleEnemy(int x, int y, int level) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.SAMPLE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 + level, Bullet.RADIUS_FOR_MEDIUM, 200)
        {
            basicY = y;
        }

        public SampleEnemy(int x, int y, int level, int count) : base(x, y, /*r=*/20, /*speed=*/ 3, Images.SAMPLE_ENEMY_IMAGE, /*LV=*/level, /*hp=*/1 + level, Bullet.RADIUS_FOR_MEDIUM, 200)
        {
            basicY = y;
            this.count = count;
        }
        protected override int GetEXP()
        {
            return Level * 2;
        }

        protected override List<Bullet> ShotBullet()
        {
            var bullets = new List<Bullet>();
            return bullets;
        }



        public override void DeadAction(Player player, List<Enemy> enemies, List<Item> items)
        {
            base.DeadAction(player, enemies, items);
            
            Random random = new Random();
            count--;
            if (count > 0)
            {
                X = random.Next(App.window.moveableLeftSidePosition, App.window.moveableRightSidePosition);
                enemies.Add(new SampleEnemy(X, Y, 1, count));
            }
            Debug.WriteLine($"カウント:{count}");
        }
    }
}
