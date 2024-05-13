using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 승리 스테이트
    public class VictoryState : BaseState
    {
        protected Character _character;
        
        public VictoryState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState(){}
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
