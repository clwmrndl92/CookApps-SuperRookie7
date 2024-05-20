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

            // todo : 스킬 업그레이드 되도록 수정
            isSkillUse.Value = true;
            atkRangeTargetList[0].TakeDamage((int)(status.atk * 1.0f));
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