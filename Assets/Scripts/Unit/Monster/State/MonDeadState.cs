namespace LineUpHeros
{
    // 죽음 스테이트
    public class MonDeadState : BaseState
    {
        public MonDeadState(Unit unit) : base(unit)
        {
            _unit = unit;
        }

        public override void OnEnterState()
        {
            _unit.ChangeAnimationState(EnumAnimState.Monster.DEAD);
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