using UnityEditor.UI;
using UnityEngine;

namespace LineUpHeros
{
    // 죽음 스테이트
    public class MonDeadState : MonsterState
    {
        private readonly float _monRemovalTime = 5f;
        private float _stateEnterTime;
        public MonDeadState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _stateEnterTime = Time.time;
            _monster.ChangeAnimationState(EnumState.Monster.DEAD);
        }

        public override void OnUpdateState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override bool CheckChangeState()
        {
            // Dead state로 전환된 이후 시간이 경과했으면 몬스터 제거
            if (Time.time - _stateEnterTime >= _monRemovalTime)
            {
                // todo : 몬스터 시체 제거
            }
            return true;
        }
    }
}