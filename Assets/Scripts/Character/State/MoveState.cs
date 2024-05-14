namespace LineUpHeros
{
    // Move 스테이트
    public class MoveState : BaseState
    {
        public MoveState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumAnimState.MOVE);
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