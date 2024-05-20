using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LineUpHeros
{
    // Idle 스테이트
    public class BossMonIdleState : BossState
    {

        public BossMonIdleState(BossMonster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Monster.IDLE);
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
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
            if (_monster.isDead.Value)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.DEAD);
                return true;
            }
            // 스킬 사용 가능한지 체크(쿨타임), 가능하면 SpecialAttack State로 전환
            if (_monster.canSkill)
            {
                _monster.stateMachine.ChangeState(EnumState.BossMonster.SKILL);
                return true;
            }
            // Detect 범위내에 캐릭터가 있는지 체크, 있으면 Move State로 전환
            List<IDamagable> detectList = _monster.DetectCharacters(_monster.status.detectRange);
            if (detectList.Count != 0)
            {
                // 다른 스테이트로 detectList 전달
                globalVariables.detectTargetList = detectList;
                _monster.stateMachine.ChangeState(EnumState.Monster.MOVE);
                return true;
            }
            return false;
        }
    }
}