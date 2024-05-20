namespace LineUpHeros
{
    // 승리 스테이트
    public class CharVictoryState : CharacterState
    {
        public CharVictoryState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumState.Character.VICTORY);
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
            return false;
        }
    }
}