namespace LineUpHeros
{
    // Idle 스테이트
    public class MonIdleState : MonsterState
    {
        public MonIdleState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Monster.IDLE);
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
            throw new System.NotImplementedException();
        }
    }
}