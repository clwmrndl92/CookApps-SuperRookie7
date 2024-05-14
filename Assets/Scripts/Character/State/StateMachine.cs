using System.Collections.Generic;

namespace LineUpHeros
{
    // FSM 상태 Enum
    public enum EnumState
    {
        Idle,
        Move,
        Atk,
        SpecialAtk,
        Dead,
        Hurt,
        Victory
    }

    // 스테이트 추가, 제거, 전환 하는 스테이트머신 클래스
    public class StateMachine
    {
        // 생성된 스테이트 담는 딕셔너리
        private readonly Dictionary<EnumState, BaseState> _states = new();

        public StateMachine(EnumState stateName, BaseState state)
        {
            AddState(stateName, state);
            currentState = GetState(stateName);
        }

        public BaseState currentState { get; private set; }

        public void AddState(EnumState stateName, BaseState state)
        {
            _states.TryAdd(stateName, state);
        }

        public BaseState GetState(EnumState stateName)
        {
            return _states.GetValueOrDefault(stateName, null);
        }

        public void DeleteState(EnumState removeStateName)
        {
            if (_states.ContainsKey(removeStateName)) _states.Remove(removeStateName);
        }

        public void ChangeState(EnumState nextStateName)
        {
            currentState?.OnExitState();
            if (_states.TryGetValue(nextStateName, out var newState)) currentState = newState;
            currentState?.OnEnterState();
        }

        public void UpdateState()
        {
            currentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            currentState?.OnFixedUpdateState();
        }
    }
}