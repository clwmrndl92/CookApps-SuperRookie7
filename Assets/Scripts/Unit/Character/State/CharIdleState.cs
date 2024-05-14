using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LineUpHeros
{
    // Idle 스테이트
    public class CharIdleState : CharacterState
    {
        public CharIdleState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            Debug.Log(_character.gameObject.name + " Idle Enter!");
            _character.ChangeAnimationState(EnumState.Character.IDLE);
            ((CharacterFSMGlobalParameter)_character.stateMachine.parameters).detectTargetList = null;
        }

        public override void OnUpdateState()
        {
            Debug.Log(_character.gameObject.name + " Idle State!");
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
            // todo : 일단 하드코딩 나중에 바꿔야됨, detect range 설정
            List<IDamagable> detectList = _character.DetectMonsters(10);
            if (detectList.Count != 0)
            {
                ((CharacterFSMGlobalParameter)_character.stateMachine.parameters).detectTargetList = detectList;
                _character.stateMachine.ChangeState(EnumState.Character.MOVE);
                Debug.Log(_character.gameObject.name + " change to Move State!");
            }
        }
    }
}