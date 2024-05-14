using System;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class MonsterController
    {
        [Serializable]
        public class Settings
        {
            public float monsterSpawnPeriod;
            public float monsterMaxSpawnNum;
        }
    }
}