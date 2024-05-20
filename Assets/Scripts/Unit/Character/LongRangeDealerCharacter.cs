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
        private ArrowProjectile.Factory _arrowFactory;
        [Inject]
        private SignalBus _signalBus;

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings,_globalSettings);
        }

        public override bool Attack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;
            // 화살 발사
            ArrowProjectile arrow = _arrowFactory.Create();
            arrow.FireProjectile(this, atkRangeTargetList[0], status.atk);
            
            return true;

        }
        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList == null || atkRangeTargetList.Count == 0) return false;
            // 더 센 화살 발사
            isSkillUse.OnNext(true);
            ArrowProjectile arrow = _arrowFactory.Create();
            arrow.FireProjectile(this, atkRangeTargetList[0], (int)(status.atk * status.skillDamageMultiplier));
            
            return true;
        }

    }
}