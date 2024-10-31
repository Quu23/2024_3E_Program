using ShootingGame.Entities.Planes;
using System.Media;

namespace ShootingGame
{
    /// <summary>
    /// 効果音を管理するメソッド群
    /// </summary>
    public class UtilitySE
    {
        private static SoundPlayer ENEMY_HIT_SE = new SoundPlayer("../../../music/se/bullet_hit.wav");
        private static SoundPlayer PLAYER_HIT_SE = new SoundPlayer("../../../music/se/player_hit.wav");
        private static SoundPlayer ENEMY_DEAD_SE = new SoundPlayer("../../../music/se/enemy_dead.wav");
        private static SoundPlayer BOSS_DEAD_SE  = new SoundPlayer("../../../music/se/boss_dead.wav");
        private static SoundPlayer GET_ITEM_SE   = new SoundPlayer("../../../music/se/get_item.wav");
        private static SoundPlayer GET_ORB_SE    = new SoundPlayer("../../../music/se/get_orb.wav");
        private static SoundPlayer SHOT_SE       = new SoundPlayer("../../../music/se/shot.wav");

        public static void PlayEnemyHitSE()
        {
            ENEMY_HIT_SE.Play();
        }
        public static void PlayPlayerHitSE()
        {
            PLAYER_HIT_SE.Play();
        }
        public static void PlayEnemyDeadSE()
        {
            ENEMY_DEAD_SE.Play();
        }
        public static void PlayBossDeadSE()
        {
            BOSS_DEAD_SE.Play();
        }
        public static void PlayGetItemSE()
        {
            GET_ITEM_SE.Play();
        }
        public static void PlayGetOrbSE()
        {
            GET_ORB_SE.Play();
        }
        public static void PlayShotSE()
        {
            SHOT_SE.Play();
        }

    }
}
