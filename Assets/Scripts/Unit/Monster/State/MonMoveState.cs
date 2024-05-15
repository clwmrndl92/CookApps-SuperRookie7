using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class MonMoveState : MonsterState
    {
        private List<IDamagable> _detectTargetList;
        public MonMoveState(Monster monster) : base(monster)
        {
            _monster = monster;
            _detectTargetList = null;
        }

        public override void OnEnterState()
        {
            _monster.ChangeAnimationState(EnumState.Monster.MOVE);
        }

        public override void OnUpdateState()
        {
            CheckChangeState();
            GameObject target = _detectTargetList[0].gameObjectIDamagable;
            _monster.position += _monster.status.moveVelocity * Time.deltaTime * (target.transform.position - _monster.position).normalized;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void CheckChangeState()
        {
            // Detect 범위내에 캐릭터가 있는지 체크, 없으면 Attack State로 전환
            List<IDamagable> detectList = _monster.DetectCharacters(_monster.status.detectRange);
            if (detectList.Count == 0)
            {
                _detectTargetList = null;
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return;
            }
            // 제일 가까운 캐릭터가 Attck 범위내에 있는지 체크, 있으면 Attack State로 전환
            if (Vector3.Distance(_monster.position, detectList[0].gameObjectIDamagable.transform.position) <= _monster.status.atkRange)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.ATK);
                return;
            }
            _detectTargetList = detectList;
        }
    }
}