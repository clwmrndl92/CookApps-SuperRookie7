using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class TankerCharacter : Character
    {
        [Inject(Id = "Tanker")]
        private TankerSetting _settings;
        [Inject]
        private SignalBus _signalBus;
        

        protected override void InitStatus()
        {
            _status = new CharacterStatus(_settings, _globalSettings);
        }
        
        public override bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            if (atkRangeTargetList.Count == 0) return false;
            // 공격 대상에게 스턴
            isSkillUse.OnNext(true);
            atkRangeTargetList[0].TakeDamage((int)(status.atk * status.skillDamageMultiplier));
            atkRangeTargetList[0].TakeStun(_settings.stunTime);
            return true;
        }

    }
    
    [Serializable]
    public class TankerSetting : CharacterSetting
    {
        // 스킬 관련 변수 추가
        public float stunTime;
    }

}