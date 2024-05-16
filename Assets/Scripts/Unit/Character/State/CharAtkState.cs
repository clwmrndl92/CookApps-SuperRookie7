using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class CharAtkState : CharacterState
    {
        private float _coolStartTime;
        private bool canAttack => !_isCool && !_isAttacking;
        private bool _isCool;
        private bool _isAttacking;
        private List<IDamagable> _attackTargetList;
        public CharAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            Debug.Log("Character attack");
            _coolStartTime = Time.time;
            _isCool = false;
            _isAttacking = false;
            _attackTargetList = new List<IDamagable>();
        }

        public override void OnUpdateState()
        {
            // 스테이트 전환 체크
            if(CheckChangeState()) return;
            // 일반공격 쿨타임 계산
            if (Time.time - _coolStartTime >= _character.status.atkCool)
            {
                _isCool = false;
            }
            // 일반공격 실행
            if (canAttack && _attackTargetList.Count != 0)
            {
                _character.FlipToTarget(_attackTargetList[0].gameObjectIDamagable);
                _character.ChangeAnimationState(EnumState.Character.ATK);
                _isAttacking = true;
                _coolStartTime = Time.time;
                _isCool = true;
            }
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
            if (_character.isDead)
            {
                _character.stateMachine.ChangeState(EnumState.Character.DEAD);
                return true;
            }
            // attack 범위내에 몬스터가 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _character.DetectMonsters(_character.status.atkRange);
            if (_isAttacking == false && attackList.Count == 0)
            {
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            _attackTargetList = attackList;
            return false;
        }
        public void Attack()
        {
            _character.Attack(_attackTargetList);
            _isAttacking = false;
        }
    }
}