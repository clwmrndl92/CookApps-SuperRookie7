using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 스킬 사용 스테이트
    public class SpecialAtkState : BaseState
    {
        protected Character _character;
        
        public SpecialAtkState(Character character) : base(character)
        {
            _character = character;
        }

        public override void OnEnterState(){}
        public override void OnUpdateState(){}
        public override void OnFixedUpdateState(){}
        public override void OnExitState(){}
    }
}
