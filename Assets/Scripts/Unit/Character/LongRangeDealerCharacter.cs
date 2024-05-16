using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class LongRangeDealerCharacter : Character
    {
        [Inject(Id = "LongRangeDealer")]
        private CharacterSetting _settings;
        [Inject]
        private CharacterGlobalSetting _globalSettings;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings,_globalSettings);
        }

        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList.Count == 0) return false;
            
            atkRangeTargetList[0].TakeDamage((int)(status.atk * 2.5f));
            return true;
        }

    }
}