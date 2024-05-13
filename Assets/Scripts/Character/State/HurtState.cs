using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 피격 스테이트
    public class HurtState : BaseState
    {
        protected Character _character;
        
        public HurtState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState(){}
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
