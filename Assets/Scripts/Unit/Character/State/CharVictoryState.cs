namespace LineUpHeros
{
    // 승리 스테이트
    public class CharVictoryState : BaseState
    {
        public CharVictoryState(Unit unit) : base(unit)
        {
            _unit = unit;
        }

        public override void OnEnterState()
        {
            _unit.ChangeAnimationState(EnumAnimState.Character.VICTORY);
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