using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 스테이트 추가, 제거, 전환 하는 스테이트머신 클래스
    public class StateMachine
    {
        // FSM 전역 변수 담는 클래스
        public FSMGlobalParameter parameters;
        // 생성된 스테이트 담는 딕셔너리
        private readonly Dictionary<string, BaseState> _states = new();

        public StateMachine(string stateName, BaseState state)
        {
            parameters = new FSMGlobalParameter();
            AddState(stateName, state);
            currentState = GetState(stateName);
        }
        public StateMachine(string stateName, BaseState state, FSMGlobalParameter fsmParameters)
        {
            parameters = fsmParameters;
            AddState(stateName, state);
            currentState = GetState(stateName);
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
            Debug.Log("changeState " + nextStateName);
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

    public class FSMGlobalParameter
    {
    }
}