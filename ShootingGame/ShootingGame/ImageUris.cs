using System;

namespace ShootingGame
{
    /// <summary>
    /// 使用する画像全てのURIを管理するクラス。
    /// </summary>
    internal class ImageUris
    {
        public static readonly Uri BACKGROUND = new Uri("../../../img/background_2.png" , UriKind.RelativeOrAbsolute);

        public static readonly Uri EXP_ORB = new Uri("../../../img/Exp_Orb.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri PLAYER = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET = new Uri("../../../img/Player_Bullet.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri STRAIGHT_ENEMY = new Uri("../../../img/StraightEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SNAKE_ENEMY = new Uri("../../../img/SnakeEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOTGUN_ENEMY = new Uri("../../../img/ShotgunEnemy.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri E_BULLET = new Uri("../../../img/Enemy_Bullet.png", UriKind.RelativeOrAbsolute);
    }
}
