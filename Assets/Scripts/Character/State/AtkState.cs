using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    public class AtkState : BaseState
    {
        protected Character _character;
        
        public AtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState(){}
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
