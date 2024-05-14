namespace LineUpHeros
{
    // FSM 베이스 스테이트
    public abstract class BaseState
    {
        protected Unit _unit;

        protected BaseState(Unit unit)
        {
            _unit = unit;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }
}