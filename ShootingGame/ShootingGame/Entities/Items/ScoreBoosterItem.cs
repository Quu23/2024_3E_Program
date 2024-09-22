using ShootingGame.Entities.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ShootingGame.Entities.Items
{
    class ScoreBoosterItem : TransientItem
    {
        public ScoreBoosterItem(int x, int y) : base(x, y, 5, 7, null,StatusEffects.INCREACE_RATE_OF_SCORE, 15)
        {
        }

       
        

        protected override void Effect(Player player)
        {
            player.increaseRateOfScore = 150;
        }

        protected override void Move()
        {
            X = 7;
            Y += Speed;
        }
    }
}
