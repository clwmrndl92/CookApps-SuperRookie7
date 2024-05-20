namespace LineUpHeros
{
    // 시그널 클래스 모음
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