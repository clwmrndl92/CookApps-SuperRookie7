namespace LineUpHeros
{
    // 죽음 스테이트
    public class MonDeadState : MonsterState
    {
        public MonDeadState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Monster.DEAD);
        }

        public override void OnUpdateState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void CheckChangeState()
        {
            
            
        }
    }
}