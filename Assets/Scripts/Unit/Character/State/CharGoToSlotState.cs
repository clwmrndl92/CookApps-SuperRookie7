using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    // 자기자리로 귀환 스테이트
    public class CharGotoSlotState : CharacterState
    {
        private const float _EPSILON = 0.05f;

        private Transform _slot;
        public CharGotoSlotState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _character.ChangeAnimationState(EnumState.Character.MOVE);
            _slot = _character.GetSlot();
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            // flip
            _character.FlipToTarget(_slot);
            // move
            Vector3 direction = (_slot.position -_character.position).normalized;
            _character.position += _character.status.moveVelocity * Time.deltaTime * direction;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }
        public override bool CheckChangeState()
        {
            // 체력이 0 이하로 떨어지면 Dead State로 전환
            if (_character.isDead.Value)
            {
                _character.stateMachine.ChangeState(EnumState.Character.DEAD);
                return true;
            }
            // 복귀 했으면 Idle State로 전환
            if (Vector3.Distance(_character.position , _slot.position) <= _EPSILON)
            {
                _character.position = _slot.position;
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            return false;
        }
    }
}