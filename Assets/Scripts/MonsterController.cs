using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LineUpHeros
{
    public class MonsterController : ITickable, IInitializable
    {
        private GameController _gameController;

        // private Monster.Factory _monsterFactory;
        private MonsterFactory _monsterFactory;

        private CharacterSlots _characterSlots;
        private Transform _firstSlot;
        private readonly Vector3 _spawnOffset = new Vector3(15f, 0, 0); // 0번 슬롯 기준

        private float _monsterSpawnTimer;

        private StageInfo currentStage => _gameController.GetCurrentStage();

        private bool canSpawn => (Time.time - _monsterSpawnTimer) >= currentStage.monsterSetting.monsterSpawnPeriod
                                 && _gameController.state.Value == GameStates.Playing;

        public MonsterController(GameController gameController, MonsterFactory monsterFactory,
            CharacterSlots characterSlots)
        {
            _gameController = gameController;
            _characterSlots = characterSlots;
            _monsterFactory = monsterFactory;
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
                MonsterSpawn();
                _monsterSpawnTimer = Time.time;
            }
        }

        private void MonsterSpawn()
        {
            var monsterSpawnList = currentStage.monsterSetting.monsterSpawnProbability;
            float total = monsterSpawnList.Sum(spawnProbability => spawnProbability.probabilty);

            // 몇마리 스폰할지 랜덤돌려서 스폰
            for (int i = 0; i < Random.Range(1, currentStage.monsterSetting.monsterMaxSpawnNum); i++)
            {
                float random = Random.Range(0, total);
                float currentThreshold = 0;
                // 어떤 몬스터를 스폰할지 스폰확률에 따라 스폰
                foreach (var spawnProbability in monsterSpawnList)
                {
                    currentThreshold += spawnProbability.probabilty;
                    if (random <= currentThreshold)
                    {
                        Monster monster = _monsterFactory.Create(spawnProbability.monsterInfo);
                        // todo : 스폰 위치 오프셋 설정 하드코딩 ㄴㄴ..
                        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f), 0);
                        monster.transform.position = _firstSlot.position + _spawnOffset + randomOffset;
                    }
                }
            }

        }
    }
}