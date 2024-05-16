using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class MonStunState : MonsterState
    {
        public float stunTime = 5f;
        private float _stateEnterTime;
        public MonStunState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            Debug.Log("monsterStun!");
            _stateEnterTime = Time.time;
            // todo : 스턴 이펙트 켜주기
            _monster.ChangeAnimationState(EnumState.Monster.STUN);
        }

        public override void OnUpdateState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
            Debug.Log("monsterStun 해제");
            // todo : 스턴 이펙트 꺼주기
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