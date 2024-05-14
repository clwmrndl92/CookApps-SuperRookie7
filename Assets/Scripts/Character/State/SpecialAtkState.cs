namespace LineUpHeros
{
    // 스킬 사용 스테이트
    public class SpecialAtkState : BaseState
    {
        public SpecialAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumAnimState.SPECIAL_ATK);
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