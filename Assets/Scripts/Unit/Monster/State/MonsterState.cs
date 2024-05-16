namespace LineUpHeros
{
    // 일반 공격 스테이트
    public  abstract class MonsterState : BaseState
    {
        
        protected Monster _monster;
        protected FSMMonsterGlobalParameter _globalParameter;
        protected MonsterState(Monster monster)
        {
            _monster = monster;
            _globalParameter = (FSMMonsterGlobalParameter)_monster.stateMachine.parameters;
        }
    }
}