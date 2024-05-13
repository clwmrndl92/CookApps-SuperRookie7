using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
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
