using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class MonAtkState : MonsterState
    {
        private float _timer;
        private List<IDamagable> _attackTargetList;
        public MonAtkState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _timer = 0;
            _attackTargetList = null;
        }

        public override void OnUpdateState()
        {
            CheckChangeState();
            if (_timer <= 0)
            {
                _monster.ChangeAnimationState(EnumState.Monster.ATK);
                if (_monster.Attack(_attackTargetList))
                {
                    _timer = _monster.status.atkCool;
                }
            }
            _timer -= Time.deltaTime;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override void CheckChangeState()
        {
            // attack 범위내에 캐릭터 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _monster.DetectCharacters(_monster.status.atkRange);
            if (attackList.Count == 0)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return;
            }
            _attackTargetList = attackList;
        }
    }
}