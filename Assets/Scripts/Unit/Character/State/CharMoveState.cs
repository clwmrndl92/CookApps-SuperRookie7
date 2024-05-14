namespace LineUpHeros
{
    // Move 스테이트
    public class CharMoveState : BaseState
    {
        public CharMoveState(Unit unit) : base(unit)
        {
            _unit = unit;
        }

        public override void OnEnterState()
        {
            _unit.ChangeAnimationState(EnumAnimState.Character.MOVE);
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