﻿using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class CharMoveState : CharacterState
    {
        private List<IDamagable> _detectTargetList;
        
        private const float _EPSILON = 0.2f;
        public CharMoveState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _character.ChangeAnimationState(EnumState.Character.MOVE);
            _detectTargetList = globalVariables.detectTargetList;
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            // flip
            GameObject target = _detectTargetList[0].gameObjectIDamagable;
            _character.FlipToTarget(target.transform);
            // move
            Vector3 direction = (target.transform.position - _character.position).normalized;
            _character.position += _character.status.moveVelocity * Time.deltaTime * direction;
            
            // 공격 대상 앞으로(같은 줄) 가기, y축 이동
            bool isTargetInSameLine = Mathf.Abs(_character.position.y - target.transform.position.y) <= _EPSILON;
            if (isTargetInSameLine == false)
            {
                int dirY = target.transform.position.y < _character.position.y ? -1 : 1;
                _character.position += _character.status.moveVelocity * Time.deltaTime * dirY * Vector3.up;
            }
            
            _detectTargetList = _character.DetectMonsters(_character.status.detectRange);
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
            // Detect 범위내에 몬스터가 있는지 체크, 없으면 Idle State로 전환
            if (_detectTargetList.Count == 0)
            {
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            // 제일 가까운 몬스터가 Attack 범위내에 있는지 체크, 있으면 Attack State로 전환
            GameObject target = _detectTargetList[0].gameObjectIDamagable;
            bool canAttack = Vector3.Distance(_character.position, target.transform.position) <= _character.status.atkRange;
            if (canAttack)
            {
                _character.stateMachine.ChangeState(EnumState.Character.ATK);
                return true;
            }
            return false;
        }
    }
}