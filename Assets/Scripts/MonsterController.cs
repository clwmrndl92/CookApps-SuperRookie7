using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LineUpHeros
{
    public class MonsterController : ITickable, IInitializable
    {
        private GameController _gameController;
        
        private MonsterControllerSetting _monsterControllerSetting;
        private Monster.Factory _monsterFactory;
         
        private CharacterSlots _characterSlots;
        private Transform _firstSlot;
        private Vector3 _spawnOffset = new Vector3(15f, 0, 0);
        
        private float _monsterSpawnTimer;

        private bool canSpawn => (Time.time - _monsterSpawnTimer) >= _monsterControllerSetting.monsterSpawnPeriod 
                                 && _gameController.state.Value == GameStates.Playing;

        public MonsterController(GameController gameController, Monster.Factory monsterFactory,  
                                MonsterControllerSetting monsterControllerSetting, CharacterSlots characterSlots)
        {
            _gameController = gameController;
            _characterSlots = characterSlots;
            _monsterFactory = monsterFactory;
            _monsterControllerSetting = monsterControllerSetting;
        }

        public void Initialize()
        {
            _monsterSpawnTimer = float.MinValue;
            _firstSlot = _characterSlots.GetSlot(0);
        }

        public void Tick()
        {
            if (canSpawn)
            {
                for (int i = 0; i < Random.Range(1, _monsterControllerSetting.monsterMaxSpawnNum); i++)
                {
                    var monster = _monsterFactory.Create();
                    Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f), 0);
                    monster.transform.position = _firstSlot.position + _spawnOffset + randomOffset;
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