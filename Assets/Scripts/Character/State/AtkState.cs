namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class AtkState : BaseState
    {
        public AtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumAnimState.ATK);
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