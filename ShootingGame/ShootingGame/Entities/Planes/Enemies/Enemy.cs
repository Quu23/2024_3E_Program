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
                if (x >= y) return -1;
                if (x < y) return 1;
                return 0;
            })) { 
            // 同じ重みはプログラムバグると思うからNG

            //  [重み]      [アイテム] 
                { 15,  CLEAR_ENEMIES_ITEM},           //  1.5% ( 15 / 1000)
                { 14,        DESTROY_ITEM},               //  1.4% ( 14 / 1000)
                {100,             EXP_ORB},                  // 10.0% (100 / 1000)
                {201,     SMALL_HEAL_ITEM},           // 20.1% (201 / 1000)
                {124,     LARGE_HEAL_ITEM},           // 12.4% (124 / 1000)
                { 19,      FULL_HEAL_ITEM},             //  1.9% ( 19 / 1000)
                { 50,     INVINCIBLE_ITEM},             //  5.0% ( 50 / 1000)
                {119,  SCORE_BOOSTER_ITEM},      // 11.9% (119 / 1000)
                {110,   SHOT_RATE_UP_ITEM},       // 11.0% (110 / 1000)
                { 80,       SPEED_UP_ITEM},           //  8.0% ( 80 / 1000)
                { 16,           BOMB_ITEM},             //  1.6% ( 16 / 1000)
                { 51,             OCTPATH},               //  5.1% ( 51 / 1000)
                { 39,     SPEED_DOWN_ITEM},      //  3.9% ( 39 / 1000)
                { 41, SHOT_RATE_DOWN_ITEM},  //  4.1% ( 41 / 1000)
                { 21,        REVERSE_ITEM},          //  2.1% ( 21 / 1000)
            };
        }

        private readonly int totalWeightOfProb;
        private readonly Random random;
        protected int dropProb;

        public Enemy(int x, int y, int radius, int speed, BitmapImage img, int level, int hp, int bulletRadius, int maxBulletCoolTime) : base(x, y, radius, speed, img, level, hp, bulletRadius, maxBulletCoolTime)
        {
            random = new Random();

            SortedDictionary<int, ItemTypes> weightsProb = GetWeightedProbsOfDropItem;

            dropProb = 50;

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
            if (random.Next(100) <= dropProb)
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
