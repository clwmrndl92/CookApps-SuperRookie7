namespace LineUpHeros
{
    // Move 스테이트
    public class MonMoveState : MonsterState
    {
        public MonMoveState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Monster.MOVE);
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