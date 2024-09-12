namespace ShootingGame
{
    /// <summary>
    /// 使用する画像全てのURIを管理するクラス。
    /// </summary>
    static class UtilityUris
    {
        //画像のURI


        public static readonly Uri BACKGROUND_URI        = new Uri("../../../img/background_2.png" , UriKind.RelativeOrAbsolute);

        //アイテム
        public static readonly Uri EXP_ORB_URI           = new Uri("../../../img/Exp_Orb.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri FAST_RATE_OF_SHOT_URI = new Uri("../../../img/Fast_Rate.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri HEALIING_ITEM_URI     = new Uri("../../../img/Healing_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri INVINCIBLE_URI        = new Uri("../../../img/Invincible_Item.png", UriKind.RelativeOrAbsolute);

        //playerとbullet
        public static readonly Uri PLAYER_URI            = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_URI          = new Uri("../../../img/Player_Bullet.png", UriKind.RelativeOrAbsolute);

        //enemy
        public static readonly Uri STRAIGHT_ENEMY_URI    = new Uri("../../../img/StraightEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SNAKE_ENEMY_URI       = new Uri("../../../img/SnakeEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOTGUN_ENEMY_URI     = new Uri("../../../img/ShotgunEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri TRUN_BACK_ENEMY_URI   = new Uri("../../../img/Trunback_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri HEXAGON_ENEMY_URI     = new Uri("../../../img/Hexagon_Enemy.png", UriKind.RelativeOrAbsolute);

        //enemyの弾
        public static readonly Uri E_BULLET_SMALL_URI    = new Uri("../../../img/Enemy_Bullet_SMALL.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri E_BULLET_URI          = new Uri("../../../img/Enemy_Bullet.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri E_BULLET_BIG_URI      = new Uri("../../../img/Enemy_Bullet_BIG.png", UriKind.RelativeOrAbsolute);


        //音楽のURI

        public static readonly Uri BGM_URI               = new Uri("../../../music/bgm/temporary_bgm.mp3", UriKind.RelativeOrAbsolute);

    }
}
