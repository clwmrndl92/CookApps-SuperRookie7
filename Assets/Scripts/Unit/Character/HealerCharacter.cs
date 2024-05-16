using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class HealerCharacter : Character
    {
        [Inject(Id = "Healer")]
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
            
            // 가장 체력이 낮은 아군 찾기
            IDamagable minHpTarget = null;
            int minHp = Int32.MaxValue;
            foreach (var target in atkRangeTargetList)
            {
                if (target.status.tmpHp == target.status.maxHp) continue;
                if (minHp > target.status.tmpHp)
                {
                    minHp = target.status.tmpHp;
                    minHpTarget = target;
                }
            }
            // 풀피 아닐때만 치유스킬 사용
            if (minHpTarget != null && minHpTarget.status.tmpHp < minHpTarget.status.maxHp)
            {
                minHpTarget?.TakeHeal((int)(status.atk * 2.5f));
                return true;
            }
            return false;
        }


    }
}