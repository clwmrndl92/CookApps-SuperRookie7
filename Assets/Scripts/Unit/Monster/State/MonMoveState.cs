using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class MonMoveState : MonsterState
    {
        private List<IDamagable> _detectTargetList;
        private float _epsilon = 0.1f;
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
            if(CheckChangeState()) return;
            GameObject target = _detectTargetList[0].gameObjectIDamagable;

            if (Vector3.Distance(_monster.position, target.transform.position) > _monster.status.atkRange)
            {
                _monster.position += _monster.status.moveVelocity * Time.deltaTime *
                                     (target.transform.position - _monster.position).normalized;
            }

            if (Mathf.Abs(_monster.position.y - target.transform.position.y) > _epsilon)
            {
                int direction = target.transform.position.y < _monster.position.y ? -1 : 1;
                _monster.position += _monster.status.moveVelocity * Time.deltaTime * direction * Vector3.up;
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
            // Detect 범위내에 캐릭터가 있는지 체크, 없으면 Attack State로 전환
            List<IDamagable> detectList = _monster.DetectCharacters(_monster.status.detectRange);
                Debug.Log(detectList.Count);
            if (detectList.Count == 0)
            {
                _detectTargetList = null;
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return true;
            }
            // 제일 가까운 캐릭터가 Attck 범위내에 있는지 체크, 있으면 Attack State로 전환
            // if (Vector3.Distance(_monster.position, detectList[0].gameObjectIDamagable.transform.position) <= _monster.status.atkRange)
            Vector3 targetPosition = detectList[0].gameObjectIDamagable.transform.position;
            if (Mathf.Abs(_monster.position.y-targetPosition.y) <= _epsilon 
                && Vector3.Distance(_monster.position, targetPosition) <= _monster.status.atkRange)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.ATK);
                return true;
            }
            _detectTargetList = detectList;
            return false;
        }
    }
}