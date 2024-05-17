using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Stun 스테이트
    public class MonStunState : MonsterState
    {
        public float stunTime = 5f;
        private float _stateEnterTime;

        private Vector3 _prevScale;
        private float _stunScale = 0.7f;
        private Color _stunColor = new Color(169, 105, 0);
        public MonStunState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            _stateEnterTime = Time.time;
            // todo : 스턴 이펙트 켜주기
            // _monster;
            _monster.ChangeAnimationState(EnumState.Monster.STUN);
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
            // todo : 스턴 이펙트 꺼주기
            _monster.ChangeSpriteColor(Color.white);
            _monster.scale = _prevScale;
        }

        public override bool CheckChangeState()
        {
            // 스턴 시간 지나면 Idle state로 전환
            if (Time.time - _stateEnterTime >= stunTime)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return true;
            }
            return false;
        }
    }
}