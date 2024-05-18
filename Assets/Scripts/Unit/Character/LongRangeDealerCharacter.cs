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

        protected override void InitStatus()
        {
            Debug.Log(_playerInfo);
            _status = new CharacterStatus(_settings,_globalSettings, _playerInfo);
        }

        public override bool Attack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;
            
            ArrowProjectile arrow = _arrowFactory.Create();
            arrow.FireProjectile(this, atkRangeTargetList[0], status.atk);
            
            return true;

        }
        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList == null || atkRangeTargetList.Count == 0) return false;

            ArrowProjectile arrow = _arrowFactory.Create();
            arrow.FireProjectile(this, atkRangeTargetList[0], (int)(status.atk * 2.5f));
            
            return true;
        }

    }
}