namespace LineUpHeros
{
    // 스킬 사용 스테이트
    public class CharSpecialAtkState : CharacterState
    {
        public CharSpecialAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumState.Character.SPECIAL_ATK);
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

        public override bool CheckChangeState()
        {
            throw new System.NotImplementedException();
        }
    }
}