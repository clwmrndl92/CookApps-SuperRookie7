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
            _character.ChangeAnimationState(EnumState.Character.IDLE);
        }

        public override void OnUpdateState()
        {
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
            // Detect 범위내에 몬스터가 있는지 체크, 있으면 Move State로 전환
            // todo : 일단 하드코딩 나중에 바꿔야됨, detect range 설정
            List<IDamagable> detectList = _character.DetectMonsters(10);
            if (detectList.Count != 0)
            {
                _character.stateMachine.ChangeState(EnumState.Character.MOVE);
            }
        }
    }
}