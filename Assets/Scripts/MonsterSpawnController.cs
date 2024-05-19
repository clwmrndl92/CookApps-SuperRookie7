using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace LineUpHeros
{
    public class MonsterSpawnController : ITickable, IInitializable
    {
        // Inject
        private GameController _gameController;
        private SignalBus _signalBus;
        private MonsterFactory _monsterFactory;
        
        // 스폰 관련
        private Transform _firstSlot;
        private readonly Vector3 _spawnOffset = new Vector3(15f, 0, 0); // 0번 슬롯 기준
        private float _monsterSpawnTimer;
        
        // 현재 상태
        private int _currentSpawnedMonsters;
        public ReactiveProperty<int> currentMonsterKills = new ReactiveProperty<int>(0);
        public enum EnumSpawnPhase
        {
            Wating,
            Normal,
            Boss
        }
        private EnumSpawnPhase _phase;
        public ReactiveProperty<BossMonster> boss = new ReactiveProperty<BossMonster>();
        
        // stage 정보
        private StageInfo _currentStage => _gameController.GetCurrentStage();
        
        private bool canSpawn => (Time.time - _monsterSpawnTimer) >= _currentStage.monsterSetting.monsterSpawnPeriod
                                 && _currentSpawnedMonsters < _currentStage.monsterSetting.requiredMonsterKills
                                 && _phase == EnumSpawnPhase.Normal;

        public MonsterSpawnController(GameController gameController, MonsterFactory monsterFactory, SignalBus signalBus, CharacterSlots _characterSlots)
        {
            _gameController = gameController;
            _monsterFactory = monsterFactory;
            _signalBus = signalBus;

            _firstSlot = _characterSlots.GetSlot(0);
            
            _signalBus.Subscribe<GameEvent.StageStartSignal>(OnStageStart);
            _signalBus.Subscribe<GameEvent.MonsterDieSignal>(_ => OnMonsterDie());
            _signalBus.Subscribe<GameEvent.BossDieSignal>(_ => OnBossDie());
        }

        public void Initialize()
        {
            _monsterSpawnTimer = float.MinValue;
        }

        public void Tick()
        {
            if (_phase == EnumSpawnPhase.Normal)
            {
                if (canSpawn)
                {
                    MonsterSpawn();
                    _monsterSpawnTimer = Time.time;
                }
                // 몬스터 처치수 채웠을때
                if (currentMonsterKills.Value == _currentStage.monsterSetting.requiredMonsterKills)
                {
                    if (_currentStage.bossSetting.isBossSpawn)
                    {
                        // 보스 소환
                        boss.Value = (BossMonster)_monsterFactory.Create(_currentStage.bossSetting.bossMonster, _firstSlot.position + _spawnOffset);
                        _phase = EnumSpawnPhase.Boss;
                    }
                    else
                    {
                        // 보스 없이 스테이지 클리어
                        StageClear();
                    }
                }
            }
        }

        private void OnStageStart()
        {
            _currentSpawnedMonsters = 0;
            currentMonsterKills.Value = 0;
            _phase = EnumSpawnPhase.Normal;
        }
        private void OnMonsterDie()
        {
            currentMonsterKills.Value++;
            Debug.Log($"{currentMonsterKills} / {_currentSpawnedMonsters} / {_currentStage.monsterSetting.requiredMonsterKills}");
        }

        private void OnBossDie()
        {
            boss.Value = null;
            StageClear();
        }

        private void StageClear()
        {
            _phase = EnumSpawnPhase.Wating;
            _gameController.StageCler();
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
                    if (currentThreshold >= random)
                    {
                        // todo : 스폰 위치 오프셋 설정 하드코딩 ㄴㄴ..
                        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), Random.Range(-0.5f, 0.5f), 0);
                        Vector3 monsterPosition = _firstSlot.position + _spawnOffset + randomOffset;
                        _monsterFactory.Create(spawnProbability.monsterInfo, monsterPosition);
                        break;
                    }
                }
            }
            _currentSpawnedMonsters += spawnNum;

        }
    }
}