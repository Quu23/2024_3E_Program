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
                { 15,  CLEAR_ENEMIES_ITEM},  //10
                { 14,        DESTROY_ITEM},  //4
                {100,             EXP_ORB},  //25
                {201,     SMALL_HEAL_ITEM},  //100
                {124,     LARGE_HEAL_ITEM},  //60
                { 19,      FULL_HEAL_ITEM},  //5
                { 50,     INVINCIBLE_ITEM},  //30
                {119,  SCORE_BOOSTER_ITEM},  //70
                {110,   SHOT_RATE_UP_ITEM},  //50
                { 80,       SPEED_UP_ITEM},  //45
                { 16,           BOMB_ITEM},  //15
                { 51,             OCTPATH},  //43
                { 39,     SPEED_DOWN_ITEM},  //44
                { 41, SHOT_RATE_DOWN_ITEM},  //49
                { 21,        REVERSE_ITEM},  //29
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
