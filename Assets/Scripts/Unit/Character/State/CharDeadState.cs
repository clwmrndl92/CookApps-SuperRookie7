using UnityEngine;

namespace LineUpHeros
{
    // 죽음 스테이트
    public class CharDeadState : CharacterState
    {
        private float _stateEnterTime;

        public CharDeadState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _stateEnterTime = Time.time;
            _character.ChangeAnimationState(EnumState.Character.DEAD);
        }

        public override void OnUpdateState()
        {
            if(CheckChangeState()) return;
            
            // 0이었던 체력을 부활 시간에 맞게 보간하여 설정
            float progress = Time.time - _stateEnterTime;
            progress /= _character.status.revivalTime;
            progress = progress > 1f ? 1f : progress;
            
            int tmpHp = Mathf.FloorToInt(progress * _character.status.maxHp * (1 - Mathf.Epsilon));
            _character.status.tmpHp = tmpHp;
                
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override bool CheckChangeState()
        {
            // 부활시간이 되었으면 Idle State로 전환
            if (Time.time - _stateEnterTime >= _character.status.revivalTime)
            {
                _character.Revive();
                _character.stateMachine.ChangeState(EnumState.Character.IDLE);
                return true;
            }
            return false;
        }
    }
}