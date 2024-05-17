using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LineUpHeros
{
    public class MonsterController : ITickable, IInitializable
    {
        private MonsterControllerSetting _monsterControllerSetting;
        private Monster.Factory _monsterFactory;

        private float _monsterSpawnTimer;

        private bool canSpawn => (Time.time - _monsterSpawnTimer) >= _monsterControllerSetting.monsterSpawnPeriod;

        public MonsterController(Monster.Factory monsterFactory,  MonsterControllerSetting monsterControllerSetting)
        {
            _monsterFactory = monsterFactory;
            _monsterControllerSetting = monsterControllerSetting;
        }

        public void Initialize()
        {
            _monsterSpawnTimer = float.MinValue;
        }

        public void Tick()
        {
            if (canSpawn)
            {
                for (int i = 0; i < Random.Range(1, _monsterControllerSetting.monsterMaxSpawnNum); i++)
                {
                    _monsterFactory.Create();
                }
                _monsterSpawnTimer = Time.time;
            }
        }
        
        [Serializable]
        public class MonsterControllerSetting
        {
            public float monsterSpawnPeriod;
            public float monsterMaxSpawnNum;
        }
    }
}