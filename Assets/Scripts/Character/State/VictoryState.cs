namespace LineUpHeros
{
    // 승리 스테이트
    public class VictoryState : BaseState
    {
        public VictoryState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumAnimState.VICTORY);
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