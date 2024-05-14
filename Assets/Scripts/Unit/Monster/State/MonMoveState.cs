namespace LineUpHeros
{
    // Move 스테이트
    public class MonMoveState : BaseState
    {
        public MonMoveState(Unit unit) : base(unit)
        {
            _unit = unit;
        }

        public override void OnEnterState()
        {
            _unit.ChangeAnimationState(EnumAnimState.Monster.MOVE);
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
    }
}