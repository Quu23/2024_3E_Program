﻿using System.Windows.Media.Imaging;
using ShootingGame.Entities.Planes;

namespace ShootingGame.Entities.Items
{
    class HealingItem : Item
    {
        public HealingItem(int x, int y) : base(x, y, 16, 6, Images.HEALING_ITEM_IMAGE)
        {

        }

        public override void MakeEffect(Player player)
        {
            if (player.Hp < player.MAX_HP - 5)
            {
                player.Hp += 5;
            }
            else
            {
                player.HeelFullOfHp();
            }
        }

        protected override void Move()
        {
            Y += Speed;
        }
    }
}
