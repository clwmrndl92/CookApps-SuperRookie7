using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class LongRangeDealerCharacter : Character
    {
        [Inject(Id = "LongRangeDealer")]
        private Settings _settings;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings);
        }

        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;
            
            atkRangeTargetList[0].TakeDamage((int)(status.atk * 2.5f));
            return true;
        }

    }
}