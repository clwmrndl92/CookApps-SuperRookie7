namespace LineUpHeros
{
    public abstract class CharacterState : BaseState
    {
        protected Character _character;
        protected StateMachine _stateMachine;
        // 스테이트 머신 공용 변수
        protected FSMCharacterGlobalVariables globalVariables;
        protected CharacterState(Character character)
        {
            _character = character;
            _stateMachine = _character.stateMachine;
            globalVariables = (FSMCharacterGlobalVariables)_character.stateMachine.globalVariables;
        }
    }
}