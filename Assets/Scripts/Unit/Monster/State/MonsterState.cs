namespace LineUpHeros
{
    // 일반 공격 스테이트
    public  abstract class MonsterState : BaseState
    {
        
        protected Monster _monster;
        protected MonsterState(Monster monster)
        {
            _monster = monster;
        }
    }
}