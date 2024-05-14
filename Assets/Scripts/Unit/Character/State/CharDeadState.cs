using UnityEngine;

namespace LineUpHeros
{
    // 죽음 스테이트
    public class CharDeadState : CharacterState
    {
        public CharDeadState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumState.Character.DEAD);
        }

        public override void OnUpdateState()
        {
            Debug.Log(_character.gameObject.name + " Dead State!");
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }
        public override void CheckChangeState()
        {
            throw new System.NotImplementedException();
        }
    }
}