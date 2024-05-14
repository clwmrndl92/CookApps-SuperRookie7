namespace LineUpHeros
{
    // 스킬 사용 스테이트
    public class CharSpecialAtkState : BaseState
    {
        public CharSpecialAtkState(Unit unit) : base(unit)
        {
            _unit = unit;
        }

        public override void OnEnterState()
        {
            _unit.ChangeAnimationState(EnumAnimState.Character.SPECIAL_ATK);
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