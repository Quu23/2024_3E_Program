using ShootingGame.Entities.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame.Entities.Items
{
    internal class BombItem : TransientItem
    {
        public BombItem(int x, int y) : base(x, y, 16, 10, Images.BOMB_ITEM_IMAGE, StatusEffects.BOMB_MODE, 50)
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
