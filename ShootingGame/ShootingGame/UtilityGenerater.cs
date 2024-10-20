using ShootingGame.Entities.Items;
using ShootingGame.Entities.Planes.Enemies;

namespace ShootingGame
{
    public class UtilityGenerater
    {
        private UtilityGenerater() {}

        public static Enemy GenerateEnemy(EnemyTypes enemyType,int x,int y,int level)
        {
            switch (enemyType)
            {
                case EnemyTypes.BIG_ENEMY:
                    return new BigEnemy(x, y, level);
                case EnemyTypes.GOLDEN_ENEMY:
                    return new GoldenEnemy(x, y, level);
                case EnemyTypes.HEXAGON_ENEMY:
                    return new HexagonEnemy(x, y, level);
                case EnemyTypes.MISSILE_ENEMY:
                    return new MissileEnemy(x, y, level);
                case EnemyTypes.SHOTGUN_ENEMY:
                    return new ShotgunEnemy(x, y, level);
                case EnemyTypes.SNAKE_ENEMY:
                    return new SnakeEnemy(x, y, level);
                case EnemyTypes.SPLASH_ENEMY:
                    return new SplashEnemy(x, y, level);
                case EnemyTypes.SPLIT_ENEMY:
                    return new SplitEnemy(x, y, level);
                case EnemyTypes.STRAIGHT_ENEMY:
                    return new StraightEnemy(x, y, level);
                case EnemyTypes.TURNBACK_ENEMY:
                    return new TurnBackEnemy(x, y, level);
                case EnemyTypes.CYCLONE_ENEMY:
                    return new CycloneEnemy(x, y, level);
                case EnemyTypes.LASER_ENEMY:
                    return new LaserEnemy(x, y, level);
                default:
                    throw new ArgumentException($"存在しないEnemyTypeです。enemyType={enemyType}");
            }
        }

        public static Item GenerateItem(ItemTypes itemType, int x, int y)
        {
            switch (itemType)
            {
                case ItemTypes.CLEAR_ENEMIES_ITEM:
                    return new ClearEnemiesItem(x, y);
                case ItemTypes.DESTROY_ITEM:
                    return new DestroyItem(x, y);
                case ItemTypes.EXP_ORB:
                    return new ExpOrb(x, y);
                case ItemTypes.HEALING_ITEM:
                    return new HealingItem(x, y);
                case ItemTypes.INVINCIBLE_ITEM:
                    return new InvincibleItem(x, y);
                case ItemTypes.SCORE_BOOSTER_ITEM:
                    return new ScoreBoosterItem(x, y);
                case ItemTypes.SHOT_RATE_DOWN_ITEM:
                    return new ShotRateDownItem(x, y);
                case ItemTypes.SHOT_RATE_UP_ITEM:
                    return new ShotRateUpItem(x, y);
                case ItemTypes.SPEED_DOWN_ITEM:
                    return new SpeedDownItem(x, y);
                case ItemTypes.SPEED_UP_ITEM:
                    return new SpeedUpItem(x, y);
                default:
                    throw new ArgumentException($"存在しないItemTypeです。itemType={itemType}");
            }
        }
        }
}
