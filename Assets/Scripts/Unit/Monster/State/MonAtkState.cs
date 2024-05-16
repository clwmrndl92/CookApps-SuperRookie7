using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class MonAtkState : MonsterState
    {
        // 쿨타임 시작 시간
        private float _coolStartTime = float.MinValue;
        public bool isCool => Time.time - _coolStartTime < _monster.status.atkCool;
        
        private bool _isAttacking;
        private bool canAttack => !isCool && !_isAttacking;
        
        private List<IDamagable> _attackTargetList;
        public MonAtkState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _isAttacking = false;
            _monster.ChangeAnimationState(EnumState.Monster.IDLE); // 정지 애니메이션
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            if (canAttack && _attackTargetList.Count != 0)
            {
                // 일반 공격 실행
                _monster.FlipToTarget(_attackTargetList[0].gameObjectIDamagable);
                _monster.ChangeAnimationState(EnumState.Monster.ATK);
                _isAttacking = true;
                // 일반공격 쿨타임 세팅
                _coolStartTime = Time.time;
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
            // attack 가능 범위내에 캐릭터 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _monster.DetectCharacters(_monster.status.atkRange);
            if (_isAttacking == false && attackList.Count == 0)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return true;
            }
            _attackTargetList = attackList;
            return false;
        }
        
        // Animation Event에서 호출되는 함수
        public void Attack()
        {
            _monster.Attack(_attackTargetList);
            _isAttacking = false;
            // 한번 공격 했으면 Idle state로 전환
            _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
        }
    }
}