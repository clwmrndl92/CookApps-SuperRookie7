using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Idle 스테이트
    public class MonIdleState : MonsterState
    {
        public MonIdleState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            Debug.Log("monster IDle state");
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
            // Detect 범위내에 캐릭터가 있는지 체크, 있으면 Move State로 전환
            List<IDamagable> detectList = _monster.DetectCharacters(_monster.status.detectRange);
            if (detectList.Count != 0)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.MOVE);
                return true;
            }
            return false;
        }
    }
}