using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "StageInfo", menuName = "ScriptableObject/Stage/StageInfo")]
    public class StageInfo : ScriptableObject
    {
        public string name;
        public StageMonsterSetting monsterSetting;
        public StageBossSetting bossSetting;
        
        [Serializable]
        public class StageMonsterSetting
        {
            public int requiredMonsterKills; // 스테이지 클리어 혹은 보스등장에 필요 몬스터 처치수
            
            public float monsterSpawnPeriod;
            public int monsterMaxSpawnNum;
        
            public List<StageMonsterSpawnProbabilty> monsterSpawnProbability;
        }
        [Serializable]
        public class StageBossSetting
        {
            public bool isBossSpawn; // true : 보스를 잡아야 스테이지 클리어
            public MonsterInfo bossMonster;
        }
        
        [Serializable]
        public class StageMonsterSpawnProbabilty
        {
            public MonsterInfo monsterInfo;
            
            [Range(0, 100)]public float probabilty;
        }
    }
}
