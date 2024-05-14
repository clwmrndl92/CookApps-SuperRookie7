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
            _character.ChangeAnimationState(EnumState.Character.ATK);
            _timer = 0;
            _attackTargetList = null;
        }
        
        public void OnEnterState(List<IDamagable> attackTargetList)
        {
            OnEnterState();
            _attackTargetList = attackTargetList;
        }

        public override void OnUpdateState()
        {
            // todo : 로직 수정 필요
            CheckChangeState();
            if (_timer <= 0)
            {
                if (_attackTargetList == null)
                {
                    _attackTargetList = Util.GetDetectDamagableList(_character.position, _character.status.atkRange, LayerMasks.Monster);
                }
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
                ((CharacterFSMGlobalParameter)_character.stateMachine.parameters).attackTargetList = null;
                _character.stateMachine.ChangeState(EnumState.Character.MOVE);
                Debug.Log(_character.gameObject.name + " change to Move State!");
            }
        }
    }
}