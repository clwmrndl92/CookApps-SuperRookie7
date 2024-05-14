namespace LineUpHeros
{
    // FSM 베이스 스테이트
    public abstract class BaseState
    {
        protected Character _character;

        protected BaseState(Character character)
        {
            _character = character;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}