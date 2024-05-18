using UnityEngine;

namespace LineUpHeros
{
    // 죽음 스테이트
    public class MonDeadState : MonsterState
    {
        private readonly float _monRemovalTime = 2f;
        private float _stateEnterTime;

        private Color _color;
        public MonDeadState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _stateEnterTime = Time.time;
            _monster.ChangeAnimationState(EnumState.Monster.DEAD);
            _color = Color.white;
            _monster.collider.isTrigger = true;
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            
            _color.a = Mathf.Lerp(1, 0, (Time.time - _stateEnterTime) / _monRemovalTime);
            _monster.ChangeSpriteColor(_color);
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            _monster.ChangeSpriteColor(Color.white);
            _monster.collider.isTrigger = false;
        }

        public override bool CheckChangeState()
        {
            // Dead state로 전환된 이후 시간이 경과했으면 몬스터 제거
            if (Time.time - _stateEnterTime >= _monRemovalTime)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                _monster.DespawnMonster();
                return true;
            }
            return false;
        }
    }
}