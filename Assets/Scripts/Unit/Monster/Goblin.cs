using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class Goblin : Monster
    {
        [Inject(Id = "Goblin")]
        private MonsterInfo _monsterInfo;
        [Inject]
        private MonsterGlobalSetting _globalSetting;
        protected override void InitStatus()
        {
            _status = new MonsterStatus(_monsterInfo, _globalSetting);
        }

    }
}