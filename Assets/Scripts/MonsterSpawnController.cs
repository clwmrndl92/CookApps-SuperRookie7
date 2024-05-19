using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LineUpHeros
{
    public class MonsterSpawnController : ITickable, IInitializable
    {
        private GameController _gameController;
        private SignalBus _signalBus;
        private MonsterFactory _monsterFactory;
        
        private Transform _firstSlot;
        private readonly Vector3 _spawnOffset = new Vector3(15f, 0, 0); // 0번 슬롯 기준

        private float _monsterSpawnTimer;
        private int _currentSpawnedMonsters;
        
        // stage 정보
        private StageInfo _currentStage;
        
        private bool canSpawn => (Time.time - _monsterSpawnTimer) >= _currentStage.monsterSetting.monsterSpawnPeriod
                                 && _currentSpawnedMonsters < _currentStage.monsterSetting.requiredMonsterKills
                                 && _gameController.state.Value == GameStates.Playing;

        public MonsterSpawnController(GameController gameController, MonsterFactory monsterFactory, SignalBus signalBus)
        {
            _gameController = gameController;
            _monsterFactory = monsterFactory;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<GameEvent.StageStartSignal>(OnStageStart);
        }

        public void Initialize()
        {
            _monsterSpawnTimer = float.MinValue;
        }

        public void Tick()
        {
            if (canSpawn)
            {
                MonsterSpawn();
                _monsterSpawnTimer = Time.time;
            }
        }

        private void OnStageStart()
        {
            _currentStage = _gameController.GetCurrentStage();
            _currentSpawnedMonsters = 0;
        }

        private void MonsterSpawn()
        {
            var monsterSpawnList = _currentStage.monsterSetting.monsterSpawnProbability;
            float total = monsterSpawnList.Sum(spawnProbability => spawnProbability.probabilty);

            // 몇마리 스폰할지 랜덤돌려서 스폰
            int remainMonsterNum = _currentStage.monsterSetting.requiredMonsterKills - _currentSpawnedMonsters;
            int spawnNum = Random.Range(1, Mathf.Min(_currentStage.monsterSetting.monsterMaxSpawnNum, remainMonsterNum));
            for (int i = 0; i < spawnNum; i++)
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