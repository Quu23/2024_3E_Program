using ShootingGame.Entities.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame.Entities.Items
{
    class ReverseItem : TransientItem
    {
        public ReverseItem(int x, int y) : base(x, y, 16, 5, Images.REVERSE_ITEM_IMAGE, StatusEffects.REVERSE_CONTROLS, 100)
        {
        }

        protected override void Effect(Player player) 
        {
            
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
