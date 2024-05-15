using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // Move 스테이트
    public class CharMoveState : CharacterState
    {
        private List<IDamagable> _detectTargetList;
        public CharMoveState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumState.Character.MOVE);
            _detectTargetList = null;
        }

        public override void OnUpdateState()
        {
            CheckChangeState();
            // todo : _detectTargetList 사용하여 이동 목표 방향 설정 추가
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }
        public override void CheckChangeState()
        {   
            // Detect 범위내에 몬스터가 있는지 체크, 없으면 Attack State로 전환
            // todo : detect range설정
            List<IDamagable> detectList = _character.DetectMonsters(10);
            if (detectList.Count == 0)
            {
                _detectTargetList = null;
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return;
            }
            // 제일 가까운 몬스터가 Attck 범위내에 있는지 체크, 있으면 Attack State로 전환
            if (Vector3.Distance(_character.position, detectList[0].gameObjectIDamagable.transform.position) <= _character.status.atkRange)
            {
                _character.stateMachine.ChangeState(EnumState.Character.ATK);
                return;
            }
            _detectTargetList = detectList;
        }
    }
}