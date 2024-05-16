using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class CharMoveState : CharacterState
    {
        private List<IDamagable> _detectTargetList;
        private float _epsilon = 0.05f;
        public CharMoveState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            Debug.Log("Character move");
            _character.ChangeAnimationState(EnumState.Character.MOVE);
            _detectTargetList = null;
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            GameObject target = _detectTargetList[0].gameObjectIDamagable;
            _character.position += _character.status.moveVelocity * Time.deltaTime * (target.transform.position - _character.position).normalized;
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
            // Detect 범위내에 몬스터가 있는지 체크, 없으면 Idle State로 전환
            List<IDamagable> detectList = _character.DetectMonsters(_character.status.detectRange);
            if (detectList.Count == 0)
            {
                _detectTargetList = null;
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            // 제일 가까운 몬스터가 Attck 범위내에 있는지 체크, 있으면 Attack State로 전환
            if (Vector3.Distance(_character.position, detectList[0].gameObjectIDamagable.transform.position) <= _character.status.atkRange)
            {
                _character.stateMachine.ChangeState(EnumState.Character.ATK);
                return true;
            }
            _detectTargetList = detectList;
            return false;
        }
    }
}