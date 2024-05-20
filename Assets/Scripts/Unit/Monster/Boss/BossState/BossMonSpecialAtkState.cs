using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 스킬 사용 스테이트
    public class BossMonSpecialAtkState : BossState
    {
        // 쿨타임 시작 시간
        private float _coolStartTime = -10;
        public bool isCool => Time.time - _coolStartTime < _monster.status.skillCoolTime;
        
        
        private bool _isSpecialAttacking;
        
        private bool canSpecialAttack => !isCool && !_isSpecialAttacking;
        
        private List<IDamagable> _attackTargetList;
        
        public BossMonSpecialAtkState(BossMonster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.BossMonster.IDLE);
        }

        public override void OnUpdateState()
        {
            // 스테이트 전환 체크
            if(CheckChangeState()) return;
            // 공격 실행
            _attackTargetList = _monster.DetectCharacters(_monster.status.detectRange);
            if (canSpecialAttack && _attackTargetList.Count != 0)
            {
                _monster.FlipToTarget(_attackTargetList[0].gameObjectIDamagable.transform);
                _monster.ChangeAnimationState(EnumState.BossMonster.SKILL);
                _isSpecialAttacking = true;
                _coolStartTime = Time.time;
            }
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            _isSpecialAttacking = false;
        }

        public override bool CheckChangeState()
        {
            // 체력이 0 이하로 떨어지면 Dead State로 전환
            if (_monster.isDead.Value)
            {
                _monster.stateMachine.ChangeState(EnumState.BossMonster.DEAD);
                return true;
            }
            // // 쿨타임이면 Idle로 전환
            // if (isCool)
            // {
            //     Debug.Log("isCool true");
            //     _monster.stateMachine.ChangeState(EnumState.Character.IDLE);
            //     return true;
            // }
            return false;
        }
        // Animation 이벤트를 통해 호출되는 함수
        public void SpecialAttack()
        {
            _monster.SpecialAttack();
            _isSpecialAttacking = false;
            // 한번 공격 했으면 Idle state로 전환
            _monster.stateMachine.ChangeState(EnumState.BossMonster.IDLE);
        }
        
    
    }
}