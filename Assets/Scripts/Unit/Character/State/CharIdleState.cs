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
            base.OnEnterState();
            _character.ChangeAnimationState(EnumState.Character.IDLE);
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
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
            if (_character.isDead.Value)
            {
                _character.stateMachine.ChangeState(EnumState.Character.DEAD);
                return true;
            }
            // 제일 가까운 몬스터가 Skill 범위내에 있고 사용 가능한지 체크, 있으면 SpecialAttack State로 전환
            if (_character.canSkill)
            {
                _character.stateMachine.ChangeState(EnumState.Character.SPECIAL_ATK);
                return true;
            }
            // Detect 범위내에 몬스터가 있는지 체크, 있으면 Move State로 전환
            List<IDamagable> detectList = _character.DetectMonsters(_character.status.detectRange);
            if (detectList.Count != 0)
            {
                // 다른 스테이트로 detectList 전달
                globalVariables.detectTargetList = detectList;
                _character.stateMachine.ChangeState(EnumState.Character.MOVE);
                return true;
            }
            return false;
        }
    }
}