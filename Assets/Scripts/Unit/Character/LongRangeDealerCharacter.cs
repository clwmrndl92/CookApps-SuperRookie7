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
            // todo : 스킬 업그레이드 되도록 수정
            isSkillUse.Value = true;
            ArrowProjectile arrow = _arrowFactory.Create();
            arrow.FireProjectile(this, atkRangeTargetList[0], (int)(status.atk * 2.5f));
            
            return true;
        }

    }
}