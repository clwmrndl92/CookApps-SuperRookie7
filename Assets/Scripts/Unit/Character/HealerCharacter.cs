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
        
        // 힐러만 스킬대상이 동료들이어서 재정의 함
        public bool canSkill
        {
            get
            {
                // 스킬 범위내 동료 체크
                List<IDamagable> characters = DetectCharacters(status.skillRange);
                if (characters.Count == 0) return false;
                bool isInRange = Vector3.Distance(position, characters[0].gameObjectIDamagable.transform.position) <=
                                 status.skillRange;
                // 스킬 쿨타임 체크
                CharSpecialAtkState skillState = (CharSpecialAtkState)_stateMachine.GetState(EnumState.Character.SPECIAL_ATK);
                
                return isInRange && skillState.isCool == false;
            }
        }
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
        
        // 범위내 살아있는 동료 리스트 찾아서 리턴 (거리순 정렬)
        public List<IDamagable> DetectCharacters(float radius)
        { 
            List<IDamagable> aliveCharacterList = new List<IDamagable>();
            foreach (var character in Util.GetDetectDamagableList(position, radius, LayerMasks.Character))
            {
                if (character.isDead) continue;
                aliveCharacterList.Add(character);
            }
            return aliveCharacterList;
        }

    }
}