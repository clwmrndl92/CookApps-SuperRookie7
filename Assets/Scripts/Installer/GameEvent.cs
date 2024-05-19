namespace LineUpHeros
{
    public class GameEvent
    {
        public class StageStartSignal
        {
            
        }

        public class MonsterDieSignal
        {
            public MonsterInfo monsterInfo;
        }
        
        public class BossDieSignal
        {
            public BossMonsterInfo bossInfo;
        }
    }
}