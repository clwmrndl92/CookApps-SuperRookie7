using UnityEngine;
using System;
using System.Collections.Generic;
using Zenject;

namespace LineUpHeros
{
    public class KingGoblin : BossMonster
    {
        [Inject(Id = "KingGoblin")]
        private BossMonsterInfo _monsterInfo;
        [Inject]
        private MonsterGlobalSetting _globalSetting;
        [Inject]
        private SignalBus _signalBus;
        protected override void InitStatus()
        {
            _status = new BossMonsterStatus(_monsterInfo, _globalSetting);
        }
        
        public override void Die()
        {
            base.Die();
            _signalBus.Fire(new GameEvent.BossDieSignal() { bossInfo = _monsterInfo });
        }
        
        public override bool SpecialAttack()
        {
            List<IDamagable> targetList = DetectCharacters(status.skillRange);

            // 범위내 모든 적에게 데미지
            foreach (var target in targetList)
            {
                target.TakeDamage((int)(status.atk * status.skillDamageMultiplier));
            }

            return true;
        }

    }
}