using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using Zenject;

namespace LineUpHeros
{
    public class KingFlyingEye : BossMonster
    {
        [Inject(Id = "KingFlyingEye")]
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

            if (targetList.Count == 0) return false;
            
            // 한명 대상 즉사기
            var target = targetList[0];
            Vector3 targetPosition = target.gameObjectIDamagable.transform.position;
            StartCoroutine(SpecialAttackCoroutine(target.gameObjectIDamagable));
            target.TakeDamage((int)(status.atk * status.skillDamageMultiplier));

            return true;
        }

        IEnumerator SpecialAttackCoroutine(GameObject character)
        {
            float speed = 4;
            float gravity = -8;
            float prevY = character.transform.position.y;
            while (true)
            {
                speed  += gravity * Time.deltaTime;
                character.transform.position = character.transform.position.Y(character.transform.position.y + speed* Time.deltaTime);
                if (prevY >= character.transform.position.y) break;
                yield return null;
            }
        }

    }
}