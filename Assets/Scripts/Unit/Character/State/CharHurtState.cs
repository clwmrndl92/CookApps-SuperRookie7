using UnityEngine;

namespace LineUpHeros
{
    // 피격 스테이트
    public class CharHurtState : CharacterState
    {
        public CharHurtState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _character.ChangeAnimationState(EnumState.Character.HURT);
        }

        public override void OnUpdateState()
        {
            
            Debug.Log(_character.gameObject.name + " Hurt State!");
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }
        public override bool CheckChangeState()
        {
            throw new System.NotImplementedException();
        }
    }
}