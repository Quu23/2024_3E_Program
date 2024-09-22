namespace ShootingGame.Entities.Items
{
    /// <summary>
    /// プレイヤーの状態異常の種類を表すクラス。
    /// </summary>
    public enum StatusEffects
    {
        NORMALE                =  0,
        SPEED_UP               =  1,
        SPEED_DOWN             = -1,
        SHOT_RATE_UP           =  2,
        SHOT_RATE_DOWN         = -2,
        INCREACE_RATE_OF_SCORE = 3,
        INVINCIBLE             =  4,
    }
}
