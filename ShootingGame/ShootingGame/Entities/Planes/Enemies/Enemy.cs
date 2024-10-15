using ShootingGame.Entities.Items;
using System.Windows.Media.Imaging;
using static ShootingGame.Entities.Items.ItemTypes;

namespace ShootingGame.Entities.Planes.Enemies
{
    /// <summary>
    /// すべての敵クラスの基底クラス。
    /// </summary>
    public abstract class Enemy : Plane
    {

        private SortedDictionary<int,ItemTypes> GetWeightedProbsOfDropItem
        {  
            get => new SortedDictionary<int, ItemTypes>(Comparer<int>.Create((int x, int y) =>
            {
                if (x > y) return -1;
                if (x < y) return 1;
                return 0;
            })) { 
            // 同じ重みはプログラムバグると思うからNG

            //  [重み]      [アイテム] 
                {  5, CLEAR_ENEMIES_ITEM},
                {  1,       DESTROY_ITEM},
                { 10,            EXP_ORB},
                { 70,       HEALING_ITEM},
                {  7,    INVINCIBLE_ITEM},
                { 20, SCORE_BOOSTER_ITEM},
                { 30,  SHOT_RATE_UP_ITEM},
                { 25,      SPEED_UP_ITEM},
            };
        }

        private readonly int totalWeightOfProb;
        private readonly Random random;


        public Enemy(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime) : base(x, y, radius, speed, img, level, hp, bulletRadius, maxBulletCoolTime)
        {
            random = new Random();

            SortedDictionary<int, ItemTypes> weightsProb = GetWeightedProbsOfDropItem;

            foreach (var item in weightsProb)
            {
                totalWeightOfProb += item.Key;
            }
        }

        protected override void Move()
        {
            Y += Speed;
        }

        /// <summary>
        /// このEnemyを倒したときのアクションを表す。
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemies"></param>
        public virtual void DeadAction(Player player,List<Enemy> enemies,List<Item> items)
        {
            player.Exp += GetEXP();
            MainWindow.score += player.increaseRateOfScore * GetEXP();

            // 50/100でドロップ
            if (random.Next(100) >= 50)
            {
                items.Add(DropItem());
            }
            enemies.Remove(this);
        }

        // todo:余裕があったら二分探索で実装する。そのためには重み付き確率はTupleで管理したほうが良さそう。
        private Item DropItem()
        {
            SortedDictionary<int, ItemTypes> weightsProb = GetWeightedProbsOfDropItem;

            double randPos = totalWeightOfProb * random.NextDouble();

            foreach (var item in weightsProb)
            {
                if (randPos < item.Key)
                {
                    return UtilityGenerater.GenerateItem(item.Value, X, Y);
                }
                randPos -= item.Key;
            }
            return UtilityGenerater.GenerateItem(weightsProb.Max().Value, X, Y);

        }

        /// <summary>
        /// このEnemyを倒したときに獲得できるEXPを返す。
        /// </summary>
        /// <returns>このEnemyが持つEXP</returns>
        protected abstract int GetEXP();
    }
}
