using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    public abstract class BaseState
    {
        protected Character _character;
        
        public BaseState(Character character)
        {
            _character = character;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }

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
    
    
    public class StateMachine
    {
        public BaseState currentState { get; private set; }
        private readonly Dictionary<EnumState, BaseState> _states = new Dictionary<EnumState, BaseState>();

        public StateMachine(EnumState stateName, BaseState state)
        {
            AddState(stateName, state);
            currentState = GetState(stateName);
        }

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
            if (_states.ContainsKey(removeStateName))
            {
                _states.Remove(removeStateName);
            }
        }

        public void ChangeState(EnumState nextStateName)
        {
            currentState?.OnExitState();
            if (_states.TryGetValue(nextStateName, out BaseState newState))
            {
                currentState = newState;
            }
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
