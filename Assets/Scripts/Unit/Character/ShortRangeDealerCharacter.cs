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
        private SignalBus _signalBus;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings, _globalSettings);
        }

        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList.Count == 0) return false;
            // 공격 범위내 모든 적에게 데미지
            isSkillUse.OnNext(true);
            foreach (var target in atkRangeTargetList)
            {
                target.TakeDamage((int)(status.atk * status.skillDamageMultiplier));
            }
            return true;
        }

    }
}