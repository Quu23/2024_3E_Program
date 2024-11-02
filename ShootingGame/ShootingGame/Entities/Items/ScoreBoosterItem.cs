using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class ScoreBoosterItem : TransientItem
    {
        public ScoreBoosterItem(int x, int y) : base(x, y, 16, 7, Images.SCORE_BOOSTER_ITEM_IMAGE,StatusEffects.SCORE_BOOST, 100)
        {
        }

       
        

        protected override void Effect(Player player)
        {
            player.increaseRateOfScore *= 3;
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
