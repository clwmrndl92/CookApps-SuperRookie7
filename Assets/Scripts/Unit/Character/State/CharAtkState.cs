using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class CharAtkState : CharacterState
    {
        private float _timer;
        private List<IDamagable> _attackTargetList;
        public CharAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _timer = 0;
            _attackTargetList = null;
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            if (_timer <= 0)
            {
                _character.ChangeAnimationState(EnumState.Character.ATK);
                if (_character.Attack(_attackTargetList))
                {
                    _timer = _character.status.atkCool;
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

        public override bool CheckChangeState()
        {
            // 체력이 0 이하로 떨어지면 Dead State로 전환
            if (_character.isDead)
            {
                _character.stateMachine.ChangeState(EnumState.Character.DEAD);
                return true;
            }
            // attack 범위내에 몬스터가 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _character.DetectMonsters(_character.status.atkRange);
            if (attackList.Count == 0)
            {
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            _attackTargetList = attackList;
            return false;
        }
    }
}