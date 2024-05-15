using System;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class MonsterController
    {
        [Serializable]
        public class MonsterControllerSetting
        {
            public float monsterSpawnPeriod;
            public float monsterMaxSpawnNum;
        }
    }
}