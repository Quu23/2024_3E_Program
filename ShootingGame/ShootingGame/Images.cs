﻿using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 画像を管理するヘルパークラス。
    /// </summary>
    static class Images
    {
        public static readonly BitmapImage BACKGROUND_IMAGE        = new BitmapImage(ImageUris.BACKGROUND_URI);

        public static readonly BitmapImage EXP_ORB_IMAGE           = new BitmapImage(ImageUris.EXP_ORB_URI);
        public static readonly BitmapImage FAST_RATE_OF_SHOT_IMAGE = new BitmapImage(ImageUris.FAST_RATE_OF_SHOT_URI);
        public static readonly BitmapImage HEALING_ITEM_IMAGE      = new BitmapImage(ImageUris.HEALIING_ITEM_URI);
        public static readonly BitmapImage INVINCIBLE_ITEM_IMAGE   = new BitmapImage(ImageUris.INVINCIBLE_URI);

        public static readonly BitmapImage PLAYER_IMAGE            = new BitmapImage(ImageUris.PLAYER_URI);
        public static readonly BitmapImage PLAYER_BULLET_IMAGE     = new BitmapImage(ImageUris.P_BULLET_URI);

        public static readonly BitmapImage STRAIGHT_ENEMY_IMAGE    = new BitmapImage(ImageUris.STRAIGHT_ENEMY_URI);
        public static readonly BitmapImage SNAKE_ENEMY_IMAGE       = new BitmapImage(ImageUris.SNAKE_ENEMY_URI);
        public static readonly BitmapImage SHOTGUN_ENEMY_IMAGE     = new BitmapImage(ImageUris.SHOTGUN_ENEMY_URI);
        public static readonly BitmapImage TRUCKBACK_ENEMY_IMAGE = new BitmapImage(ImageUris.TRUNBACK_ENEMY_URI);

        public static readonly BitmapImage ENEMY_BULLET_IMAGE      = new BitmapImage(ImageUris.E_BULLET_URI);
    }
}
