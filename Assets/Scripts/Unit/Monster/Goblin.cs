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
    }
}