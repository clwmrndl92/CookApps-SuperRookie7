namespace LineUpHeros
{
    // 일반 공격 스테이트
    public abstract class CharacterState : BaseState
    {
        protected FSMCharacterGlobalParameter _globalParameter;
        protected Character _character;
        protected CharacterState(Character character)
        {
            _character = character;
            _globalParameter = (FSMCharacterGlobalParameter)_character.stateMachine.parameters;
        }
    }
}