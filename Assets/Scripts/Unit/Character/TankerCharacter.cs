using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class TankerCharacter : Character
    {
        [Inject(Id = "Tanker")]
        private CharacterSetting _settings;
        [Inject]
        private CharacterGlobalSetting _globalSettings;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings, _globalSettings);
        }
        
        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;

            atkRangeTargetList[0].TakeDamage((int)(status.atk * 1.0f));
            // 기절 만들기
            return true;
        }

    }
}