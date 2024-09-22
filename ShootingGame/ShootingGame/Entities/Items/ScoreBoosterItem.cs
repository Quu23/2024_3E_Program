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
        public ScoreBoosterItem(int x, int y, int radius, 10, BitmapImage img, StatusEffects EFFECT_KIND, int EFFECT_IIME) : base(x, y, radius, speed, img, EFFECT_KIND, EFFECT_IIME)
        {
        }

       
        

        protected override void Effect(Player player)
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
