using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class Goblin : Monster
    {
        [Inject(Id = "Goblin")]
        private MonsterSetting _monsterSetting;
        [Inject]
        private MonsterGlobalSetting _globalSetting;
        protected override void InitStatus()
        {
            _status = new MonsterStatus(_monsterSetting, _globalSetting);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.maxHp);
        }

        public override void AnimEventAttack()
        {
            BaseState attackState = _stateMachine.GetState(EnumState.Monster.ATK);
            if (_stateMachine.currentState == attackState)
            {
                ((MonAtkState) attackState).Attack();
            }
        }

    }
}