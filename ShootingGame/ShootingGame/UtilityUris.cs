﻿using System.Windows.Media.Imaging;

namespace ShootingGame
{
    /// <summary>
    /// 使用する画像全てのURIを管理するクラス。
    /// </summary>
    static class UtilityUris
    {
        //画像のURI

        //ゲームタイトルなど
        public static readonly Uri START_BACKGROUND_URI  = new Uri("../../../img/start.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri BACKGROUND1_URI       = new Uri("../../../img/background0.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri BACKGROUND2_URI       = new Uri("../../../img/background_2.png" , UriKind.RelativeOrAbsolute);
        public static readonly Uri BACKGROUND3_URI       = new Uri("../../../img/background3.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri TITLE_URI             = new Uri("../../../img/Title.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri MODE_SELECT_TEXT_URI  = new Uri("../../../img/Mode_Selection.png",UriKind.RelativeOrAbsolute);
        public static readonly Uri GAMEOVER_URI          = new Uri("../../../img/Gameover.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri GAMECLEAR_URI         = new Uri("../../../img/Gameclear.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri STUFF_CREDIT_URI      = new Uri("../../../img/Stuff_Credit.png", UriKind.RelativeOrAbsolute);


        //アイテム
        public static readonly Uri EXP_ORB_URI             = new Uri("../../../img/Exp_Orb.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOT_RATE_UP_ITEM_URI   = new Uri("../../../img/Shot_Rate_Up_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOT_RATE_DOWN_ITEM_URI = new Uri("../../../img/Shot_Rate_Down_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SPEED_UP_ITEM_URI       = new Uri("../../../img/Speed_Up_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SPEED_DOWN_ITEM_URI     = new Uri("../../../img/Speed_Down_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri HEALIING_ITEM_URI       = new Uri("../../../img/Healing_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri INVINCIBLE_ITEM_URI     = new Uri("../../../img/Invincible_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri CLEAR_ENEMIES_ITEM_URI  = new Uri("../../../img/Clear_Enemies_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri DESTROY_ITEM_URI        = new Uri("../../../img/Destroy_Item.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SCORE_BOOSTER_ITEM_URI  = new Uri("../../../img/Score_Booster_Item.png", UriKind.RelativeOrAbsolute);

        //playerとbullet
        public static readonly Uri PLAYER_URI            = new Uri("../../../img/Player.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_URI          = new Uri("../../../img/Player_Bullet.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_SMALL_URI    = new Uri("../../../img/Player_Bullet_SMALL.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_BIG_URI      = new Uri("../../../img/Player_Bullet_BIG.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri P_BULLET_DESTROY_URI  = new Uri("../../../img/Player_Destroy_Bullet.png", UriKind.RelativeOrAbsolute);

        //enemy
        public static readonly Uri STRAIGHT_ENEMY_URI    = new Uri("../../../img/StraightEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SNAKE_ENEMY_URI       = new Uri("../../../img/SnakeEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOTGUN_ENEMY_URI     = new Uri("../../../img/ShotgunEnemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri TRUN_BACK_ENEMY_URI   = new Uri("../../../img/Trunback_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri HEXAGON_ENEMY_URI     = new Uri("../../../img/Hexagon_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri GOLDEN_ENEMY_URI      = new Uri("../../../img/Golden_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri MISSILE_ENEMY_URI     = new Uri("../../../img/Missile_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri BIG_ENEMY_URI         = new Uri("../../../img/Big_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SPLIT_ENEMY_URI       = new Uri("../../../img/Split_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SPLASH_ENEMY_URI      = new Uri("../../../img/Splash_Enemy.png",UriKind.RelativeOrAbsolute);
        public static readonly Uri CYCLONE_ENEMY_URI     = new Uri("../../../img/Cyclone_Enemy.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri LASER_ENEMY_URI       = new Uri("../../../img/Laser_Enemy.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri BOSS1_URI = new Uri("../../../img/Boss1.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri BOSS2_URI = new Uri("../../../img/Boss2.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri BOSS3_URI = new Uri("../../../img/Boss3.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri FOLLOWER_URI = new Uri("../../../img/Follower.png", UriKind.RelativeOrAbsolute);

        //enemyの弾
        public static readonly Uri E_BULLET_SMALL_URI    = new Uri("../../../img/Enemy_Bullet_SMALL.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri E_BULLET_URI          = new Uri("../../../img/Enemy_Bullet.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri E_BULLET_BIG_URI      = new Uri("../../../img/Enemy_Bullet_BIG.png", UriKind.RelativeOrAbsolute);

        //状態異常等のアイコン
        public static readonly Uri SPEED_UP_ICON_URI       = new Uri("../../../img/Icon_Speedup.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SPEED_DOWN_ICON_URI     = new Uri("../../../img/Icon_Speeddown.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOT_RATE_UP_ICON_URI   = new Uri("../../../img/Icon_Rateup.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOT_RATE_DOWN_ICON_URI = new Uri("../../../img/Icon_Ratedown.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri INCINCIBLE_ICON_URI     = new Uri("../../../img/Icon_Invincible.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SCORE_BOOST_ICON_URI    = new Uri("../../../img/Icon_Scoreboost.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri DESTOROY_ICON_URI       = new Uri("../../../img/Icon_Destroy.png", UriKind.RelativeOrAbsolute);

        //武器アイコン
        public static readonly Uri DEFAULT_ICON_URI = new Uri("../../../img/Icon_Default.png",UriKind.RelativeOrAbsolute);
        public static readonly Uri BOUND_ICON_URI   = new Uri("../../../img/Icon_Bound.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri SHOTGUN_ICON_URI = new Uri("../../../img/Icon_Shotgun.png", UriKind.RelativeOrAbsolute);
        public static readonly Uri HOMING_ICON_URI  = new Uri("../../../img/Icon_Homing.png", UriKind.RelativeOrAbsolute);

        public static readonly Uri PROHIBITED_ICON_URI = new Uri("../../../img/Prohibited.png", UriKind.RelativeOrAbsolute);



        //音楽のURI


        public static readonly Uri BGM1_URI = new Uri("../../../music/bgm/bgm1.mp3", UriKind.RelativeOrAbsolute);
        public static readonly Uri BGM2_URI = new Uri("../../../music/bgm/bgm2.mp3", UriKind.RelativeOrAbsolute);
        public static readonly Uri BGM3_URI = new Uri("../../../music/bgm/temporary_bgm.mp3", UriKind.RelativeOrAbsolute); 

        public static readonly Uri BOSS1_BGM_URI = new Uri("../../../music/bgm/music_boss1.mp3", UriKind.RelativeOrAbsolute);
        public static readonly Uri BOSS2_BGM_URI = new Uri("../../../music/bgm/music_boss2.mp3", UriKind.RelativeOrAbsolute);
        public static readonly Uri BOSS3_BGM_URI = new Uri("../../../music/bgm/music_boss3.mp3", UriKind.RelativeOrAbsolute);
        public static readonly Uri BOSS4_BGM_URI = new Uri("../../../music/bgm/music_boss.mp3", UriKind.RelativeOrAbsolute);

    }
}
