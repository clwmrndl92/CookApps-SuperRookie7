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
        protected override void InitStatus()
        {
            _status = new MonsterStatus(_monsterInfo, _globalSetting);
        }

    }
}