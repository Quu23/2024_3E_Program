using System;

namespace ShootingGame
{
    /// <summary>
    /// 使用する画像全てのURIを管理するクラス。
    /// </summary>
    static class ImageUris
    {
        public static readonly Uri BACKGROUND_URI = new Uri("../../../img/background_2.png" , UriKind.RelativeOrAbsolute);

        public static readonly Uri EXP_ORB_URI = new Uri("../../../img/Exp_Orb.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri FAST_RATE_URI = new Uri("../../../img/Fast_Rate.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri PLAYER_URI = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_URI = new Uri("../../../img/Player_Bullet.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri STRAIGHT_ENEMY_URI = new Uri("../../../img/StraightEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SNAKE_ENEMY_URI = new Uri("../../../img/SnakeEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOTGUN_ENEMY_URI = new Uri("../../../img/ShotgunEnemy.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri E_BULLET_URI = new Uri("../../../img/Enemy_Bullet.png", UriKind.RelativeOrAbsolute);

        public static Uri BACKGROUND { get; internal set; }
    }
}
