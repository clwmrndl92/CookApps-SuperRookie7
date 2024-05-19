using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class FlyingEye : Monster
    {
        [Inject(Id = "FlyingEye")]
        private MonsterInfo _monsterInfo;
        [Inject]
        private MonsterGlobalSetting _globalSetting;
        [Inject]
        private SignalBus _signalBus;
        protected override void InitStatus()
        {
            _status = new MonsterStatus(_monsterInfo, _globalSetting);
        }
        public override void Die()
        {
            base.Die();
            _signalBus.Fire(new GameEvent.MonsterDieSignal() { monsterInfo = _monsterInfo });
        }
    }
}