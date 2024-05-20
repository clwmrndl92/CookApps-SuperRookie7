using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class CharAtkState : CharacterState
    {
        // 쿨타임 시작 시간
        public float coolStartTime = float.MinValue;
        public bool isCool => Time.time - coolStartTime < _character.status.atkCool;
        
        private bool _isAttacking;
        private bool canAttack => !isCool && !_isAttacking;
        
        private List<IDamagable> _attackTargetList;
        public CharAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _isAttacking = false;
            _attackTargetList = new List<IDamagable>();
            _character.ChangeAnimationState(EnumState.Character.IDLE);
        }

        public override void OnUpdateState()
        {
            // 스테이트 전환 체크
            if(CheckChangeState()) return;
            // 일반공격 실행
            if (canAttack && _attackTargetList.Count != 0)
            {
                _character.FlipToTarget(_attackTargetList[0].gameObjectIDamagable.transform);
                _character.ChangeAnimationState(EnumState.Character.ATK);
                _isAttacking = true;
                coolStartTime = Time.time;
            }
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            _isAttacking = false;
        }

        public override bool CheckChangeState()
        {
            // 체력이 0 이하로 떨어지면 Dead State로 전환
            if (_character.isDead.Value)
            {
                _stateMachine.ChangeState(EnumState.Character.DEAD);
                return true;
            }
            // attack 범위내에 몬스터가 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _character.DetectMonsters(_character.status.atkRange);
            if (_isAttacking == false && attackList.Count == 0)
            {
                _stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            _attackTargetList = attackList;
            return false;
        }
        // Animation 이벤트를 통해 호출되는 함수
        public void Attack()
        {
            _character.Attack(_attackTargetList);
            _isAttacking = false;
            // 한번 공격 했으면 Idle state로 전환
            _stateMachine.ChangeState(EnumState.Character.IDLE);
        }
    }
}