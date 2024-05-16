namespace LineUpHeros
{
    // 일반 공격 스테이트
    public  abstract class MonsterState : BaseState
    {
        
        protected Monster _monster;
        // 몬스터 스테이트머신 공용 변수
        protected FsmMonsterGlobalVariables globalVariables;
        protected MonsterState(Monster monster)
        {
            _monster = monster;
            globalVariables = (FsmMonsterGlobalVariables)_monster.stateMachine.globalVariables;
        }
    }
}