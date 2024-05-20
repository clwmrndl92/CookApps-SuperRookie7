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
            public Monster monster;
        }
        
        public class BossDieSignal
        {
            public BossMonsterInfo bossInfo;
        }
    }
}