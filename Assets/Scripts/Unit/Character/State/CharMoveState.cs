using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class CharMoveState : CharacterState
    {
        public CharMoveState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumState.Character.MOVE);
        }

        public override void OnUpdateState()
        {
            Debug.Log(_character.gameObject.name + " Move State!");
            CheckChangeState();
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
            if (attackList.Count != 0)
            {
                ((CharacterFSMGlobalParameter)_character.stateMachine.parameters).attackTargetList = attackList;
                _character.stateMachine.ChangeState(EnumState.Character.ATK);
                Debug.Log(_character.gameObject.name + " change to Attck State!");
            }
        }
    }
}