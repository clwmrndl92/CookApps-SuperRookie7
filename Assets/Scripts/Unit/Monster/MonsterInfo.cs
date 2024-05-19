using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "MonsterInfo", menuName = "ScriptableObject/Monster/MonsterInfo")]
    public class MonsterInfo : ScriptableObject
    {
        public string name;
        public GameObject prefab;
        public EnumMonsterType type;
        public int rewardExp;
        public int rewardGold;
        public MonsterSetting statusSetting;
    }
    
    [Serializable]
    public class MonsterSetting : StatSettings
    {
        public float baseAtkPerSec;

        public float moveVelocity;
    }
}
