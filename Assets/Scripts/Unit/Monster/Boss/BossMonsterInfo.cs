using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "BossMonsterInfo", menuName = "ScriptableObject/Monster/BossMonsterInfo")]
    public class BossMonsterInfo : MonsterInfo
    {
        public float skillCoolTime;

        public float skillRange;

        // 일반 공격력에 곱해지는 수치
        public float skillDamageMultiplier;
        public int rewardRuby;
    }
}
