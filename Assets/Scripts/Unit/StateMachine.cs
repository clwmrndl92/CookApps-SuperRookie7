using System.Collections.Generic;

namespace LineUpHeros
{
    // 스테이트 추가, 제거, 전환 하는 스테이트머신 클래스
    public class StateMachine
    {
        // 생성된 스테이트 담는 딕셔너리
        private readonly Dictionary<string, BaseState> _states = new();

        public StateMachine(string stateName, BaseState state)
        {
            AddState(stateName, state);
            currentState = GetState(stateName);
            currentState.OnEnterState();
        }

        public BaseState currentState { get; private set; }

        public void AddState(string stateName, BaseState state)
        {
            _states.TryAdd(stateName, state);
        }

        public BaseState GetState(string stateName)
        {
            return _states.GetValueOrDefault(stateName, null);
        }

        public void DeleteState(string removeStateName)
        {
            if (_states.ContainsKey(removeStateName)) _states.Remove(removeStateName);
        }

        public void ChangeState(string nextStateName)
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