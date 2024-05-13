using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    public class IdleState : BaseState
    {
        protected Character _character;
        
        public IdleState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState()
        {
            _character.ChangeAnimationState(EnumAnimState.IDLE);
        }
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
