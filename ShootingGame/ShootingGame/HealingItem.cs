﻿using System.Windows.Media.Imaging;

namespace ShootingGame
{
    internal class HealingItem : Item
    {
        public HealingItem(int x, int y, BitmapImage img) : base(x, y, 4, 6, Images.HEALING_ITEM_IMAGE)
        {
            throw new NotImplementedException("HealingItem's Image is null");
        }

        public override void MakeEffect(Player player)
        {
            if (player.Hp < player.MAX_HP - 5){
                player.Hp += 5;
            }else { 
                player.HeelFullOfHp(); 
            }
        }

        public override void Move()
        {
            Y += Speed;
        }
    }
}
