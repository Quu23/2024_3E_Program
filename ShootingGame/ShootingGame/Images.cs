using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 画像を管理するヘルパークラス。
    /// </summary>
    static class Images
    {
        //背景とタイトル
        public static readonly BitmapImage BACKGROUND_IMAGE         = new BitmapImage(UtilityUris.BACKGROUND_URI);
        public static readonly BitmapImage TITLE_IMAGE              = new BitmapImage(UtilityUris.TITLE_URI);
        public static readonly BitmapImage MODE_SELECT_TEXT_IMAGE   = new BitmapImage(UtilityUris.MODE_SELECT_TEXT_URI);
        public static readonly BitmapImage GAMEOVER_IMAGE           = new BitmapImage(UtilityUris.GAMEOVER_URI);
        public static readonly BitmapImage GAMECLEAR_IMAGE          = new BitmapImage(UtilityUris.GAMECLEAR_URI);


        //アイテム
        public static readonly BitmapImage EXP_ORB_IMAGE            = new BitmapImage(UtilityUris.EXP_ORB_URI);
        public static readonly BitmapImage SHOT_RATE_UP_ITEM_IMAGE  = new BitmapImage(UtilityUris.SHOT_RATE_UP_ITEM_URI);
        public static readonly BitmapImage SPEED_UP_ITEM_IMAGE      = new BitmapImage(UtilityUris.SPEED_UP_ITEM_URI);
        public static readonly BitmapImage SPEED_DOWN_ITEM_IMAGE    = new BitmapImage(UtilityUris.SPEED_DOWN_ITEM_URI);
        public static readonly BitmapImage HEALING_ITEM_IMAGE       = new BitmapImage(UtilityUris.HEALIING_ITEM_URI);
        public static readonly BitmapImage INVINCIBLE_ITEM_IMAGE    = new BitmapImage(UtilityUris.INVINCIBLE_ITEM_URI);
        public static readonly BitmapImage CLEAR_ENEMIES_ITEM_IMAGE = new BitmapImage(UtilityUris.CLEAR_ENEMIES_ITEM_URI);

        //プレイヤー
        public static readonly BitmapImage PLAYER_IMAGE             = new BitmapImage(UtilityUris.PLAYER_URI);
        public static readonly BitmapImage PLAYER_BULLET_IMAGE      = new BitmapImage(UtilityUris.P_BULLET_URI);

        //敵キャラ
        public static readonly BitmapImage STRAIGHT_ENEMY_IMAGE     = new BitmapImage(UtilityUris.STRAIGHT_ENEMY_URI);
        public static readonly BitmapImage SNAKE_ENEMY_IMAGE        = new BitmapImage(UtilityUris.SNAKE_ENEMY_URI);
        public static readonly BitmapImage SHOTGUN_ENEMY_IMAGE      = new BitmapImage(UtilityUris.SHOTGUN_ENEMY_URI);
        public static readonly BitmapImage TRUN_BACK_ENEMY_IMAGE    = new BitmapImage(UtilityUris.TRUN_BACK_ENEMY_URI);
        public static readonly BitmapImage HEXAGON_ENEMY_IMAGE      = new BitmapImage(UtilityUris.HEXAGON_ENEMY_URI);
        public static readonly BitmapImage GOLDEN_ENEMY_IMAGE       = new BitmapImage(UtilityUris.GOLDEN_ENEMY_URI);
        public static readonly BitmapImage MISSILE_ENEMY_IMAGE      = new BitmapImage(UtilityUris.MISSILE_ENEMY_URI);
        public static readonly BitmapImage BIG_ENEMY_IMAGE          = new BitmapImage(UtilityUris.BIG_ENEMY_URI);
        public static readonly BitmapImage SPLIT_ENEMY_IMAGE        = new BitmapImage(UtilityUris.SPLIT_ENEMY_URI);
        public static readonly BitmapImage SPLASH_ENEMY_IMAGE       = new BitmapImage(UtilityUris.SPLASH_ENEMY_URI);

        /// <summary>
        /// 敵の弾 Sサイズ :  8x 8 (r=4)
        /// </summary>
        public static readonly BitmapImage ENEMY_BULLET_SMALL_IMAGE = new BitmapImage(UtilityUris.E_BULLET_SMALL_URI);
        /// <summary>
        /// 敵の弾 Sサイズ : 16x16 (r=8)
        /// </summary>
        public static readonly BitmapImage ENEMY_BULLET_IMAGE       = new BitmapImage(UtilityUris.E_BULLET_URI);
        /// <summary>
        /// 敵の弾 Sサイズ : 32x32 (r=16)
        /// </summary>
        public static readonly BitmapImage ENEMY_BULLET_BIG_IMAGE   = new BitmapImage(UtilityUris.E_BULLET_BIG_URI);

        //状態異常のアイコン
        public static readonly BitmapImage SPEED_UP_ICON_IMAGE       = new BitmapImage(UtilityUris.SPEED_UP_ICON_URI);
        public static readonly BitmapImage SPEED_DOWN_ICON_IMAGE     = new BitmapImage(UtilityUris.SPEED_DOWN_ICON_URI);
        public static readonly BitmapImage SHOT_RATE_UP_ICON_IMAGE   = new BitmapImage(UtilityUris.SHOT_RATE_UP_ICON_URI);
        public static readonly BitmapImage SHOT_RATE_DOWN_ICON_IMAGE = new BitmapImage(UtilityUris.SHOT_RATE_DOWN_ICON_URI);
        public static readonly BitmapImage INCINCIBLE_ICON_IMAGE     = new BitmapImage(UtilityUris.INCINCIBLE_ICON_URI);

    }
}
