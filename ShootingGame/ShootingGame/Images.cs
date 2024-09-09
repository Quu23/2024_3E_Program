using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 画像を管理するヘルパークラス。
    /// </summary>
    static class Images
    {
        public static readonly BitmapImage BACKGROUND_IMAGE = new BitmapImage(ImageUris.BACKGROUND);

        public static readonly BitmapImage EXP_ORB_IMAGE = new BitmapImage(ImageUris.EXP_ORB);

        public static readonly BitmapImage PLAYER_IMAGE = new BitmapImage(ImageUris.PLAYER);
        public static readonly BitmapImage PLAYER_BULLET_IMAGE = new BitmapImage(ImageUris.P_BULLET);

        public static readonly BitmapImage STRAIGHT_ENEMY_IMAGE = new BitmapImage(ImageUris.STRAIGHT_ENEMY);
        public static readonly BitmapImage SNAKE_ENEMY_IMAGE = new BitmapImage(ImageUris.SNAKE_ENEMY);
        public static readonly BitmapImage SHOTGUN_ENEMY_IMAGE = new BitmapImage(ImageUris.SHOTGUN_ENEMY);

        public static readonly BitmapImage ENEMY_BULLET_IMAGE = new BitmapImage(ImageUris.E_BULLET);
    }
}
