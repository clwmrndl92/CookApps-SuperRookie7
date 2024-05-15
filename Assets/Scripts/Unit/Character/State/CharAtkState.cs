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
            // todo : 로직 수정 필요
            CheckChangeState();
            if (_timer <= 0)
            {
                _character.ChangeAnimationState(EnumState.Character.ATK);
                _character.Attack(_attackTargetList);
                _timer = _character.status.atkCool;
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
            List<IDamagable> attackList = _character.DetectMonsters(_character.status.atkRange);
            if (attackList.Count == 0)
            {
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return;
            }
            _attackTargetList = attackList;
        }
    }
}