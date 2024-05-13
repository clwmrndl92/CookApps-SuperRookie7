using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 죽음 스테이트
    public class DeadState : BaseState
    {
        protected Character _character;
        
        public DeadState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState(){}
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
