using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class ShortRangeDealerCharacter : Character
    {
        [Inject(Id = "ShortRangeDealer")]
        private CharacterSetting _settings;
        [Inject]
        private CharacterGlobalSetting _globalSettings;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings, _globalSettings);
        }

        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList.Count == 0) return false;

            foreach (var target in atkRangeTargetList)
            {
                target.TakeDamage((int)(status.atk * 1.0f));
            }
            return true;
        }

    }
}