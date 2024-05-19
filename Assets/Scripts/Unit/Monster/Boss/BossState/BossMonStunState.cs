using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Stun 스테이트
    public class BossMonStunState : MonsterState
    {
        public float stunTime = 5f;
        private float _stateEnterTime;

        // 스턴 이펙트 관련 변수
        private Vector3 _prevScale;
        private float _stunScale = 0.7f;
        private Color _stunColor = new Color(169, 105, 0);
        
        public BossMonStunState(BossMonster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _stateEnterTime = Time.time;
            _monster.ChangeAnimationState(EnumState.BossMonster.STUN);
            // 스턴 이펙트 켜기
            _monster.ChangeSpriteColor(_stunColor);
            _prevScale = _monster.scale;
            _monster.scale = _monster.scale.Y(_monster.scale.y * _stunScale);
        }

        public override void OnUpdateState()
        {
            if (CheckChangeState()) return;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            // 스턴 이펙트 끄기
            _monster.ChangeSpriteColor(Color.white);
            _monster.scale = _prevScale;
        }

        public override bool CheckChangeState()
        {
            // 체력이 0 이하로 떨어지면 Dead State로 전환
            if (_monster.isDead.Value)
            {
                _monster.stateMachine.ChangeState(EnumState.BossMonster.DEAD);
                return true;
            }
            // 스턴 시간 지나면 Idle state로 전환
            if (Time.time - _stateEnterTime >= stunTime)
            {
                _monster.stateMachine.ChangeState(EnumState.BossMonster.IDLE);
                return true;
            }
            return false;
        }
    }
}