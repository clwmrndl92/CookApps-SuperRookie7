using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class ShortRangeDealerCharacter : Character
    {
        [Inject(Id = "ShortRangeDealer")]
        private Settings _settings;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings);
        }

        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList)
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