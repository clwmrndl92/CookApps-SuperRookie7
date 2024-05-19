using System;
using System.Collections.Generic;
using ModestTree;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace LineUpHeros
{
    public enum GameStates
    {
        WaitingToStart,
        Playing,
        GameOver
    }

    public class GameController : IInitializable, ITickable, IDisposable
    {
        // input
        private InputState _inputState;
        // characters
        private TankerCharacter _tanker;
        private ShortRangeDealerCharacter _shortRangeDealer;
        private LongRangeDealerCharacter _longRangeDealer;
        private HealerCharacter _healer;
        // stage
        private List<StageInfo> _stages;
        // signal
        private SignalBus _signalBus;

        [Inject]
        private FadeInOutController _fadeInOutController;
        
        public GameController(InputState inputState, SignalBus signalBus, Settings settings,
            TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer, LongRangeDealerCharacter longRangeDealer, HealerCharacter healer) 
        {
            _inputState = inputState;
            _signalBus = signalBus;
            _stages = settings.stages;
            
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;

        }

        // game info
        public ReactiveProperty<GameStates> state { get; private set; } = new ReactiveProperty<GameStates>(GameStates.WaitingToStart);
        public ReactiveProperty<int> currentStage = new ReactiveProperty<int>(0);
        public void Dispose()
        {
        }

        public void Initialize()
        {
        }

        public void Tick()
        {
            switch (state.Value)
            {
                case GameStates.WaitingToStart:
                {
                    UpdateStarting();
                    break;
                }
                case GameStates.Playing:
                {
                    UpdatePlaying();
                    break;
                }
                case GameStates.GameOver:
                {
                    UpdateGameOver();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        private void UpdateGameOver()
        {
            Assert.That(state.Value == GameStates.GameOver);


            if (_inputState.IsMouseClick)
            {
                state.Value = GameStates.Playing;
                Time.timeScale = 1;
                // _sceneChangeController.SceneReload();
                StageStart(currentStage.Value);
            }
        }

        private void UpdatePlaying()
        {
            Assert.That(state.Value == GameStates.Playing);
            if (_tanker.isDead.Value && _shortRangeDealer.isDead.Value && _longRangeDealer.isDead.Value && _healer.isDead.Value)
                GameOver();
        }

        private void UpdateStarting()
        {
            Assert.That(state.Value == GameStates.WaitingToStart);

            if (_inputState.IsMouseClick) StartGame();
        }

        private void StartGame()
        {
            Assert.That(state.Value == GameStates.WaitingToStart || state.Value == GameStates.GameOver);
            state.Value = GameStates.Playing;
            StageStart(0);
        }
        
        
        private void GameOver()
        {
            // todo : 스테이지 생기면 게임오버 다른방식으로
            Debug.Log("Game over");
            Assert.That(state.Value == GameStates.Playing);
            Time.timeScale = 0;

            state.Value = GameStates.GameOver;
        }
        
        public void StageCler()
        {
            Debug.Log("stage clear " + GetCurrentStage().name);
            if (currentStage.Value+1 == _stages.Count)
            {
                // todo : 게임 클리어?
                StageStart(currentStage.Value);
                return;
            }
            StageStart(++currentStage.Value);
        }
        
        public void StageStart(int stageNum)
        {
            _fadeInOutController.StartEffect(() =>
                {
                    currentStage.Value = stageNum;
                    _signalBus.Fire<GameEvent.StageStartSignal>();
                    ResetCharacter();
                }
            ,0.5f, 1f,0.5f);
            Debug.Log("stage start " + GetCurrentStage().name);
        }

        private void ResetCharacter()
        {
            _tanker.status.tmpHp.Value = _tanker.status.maxHp;
            _shortRangeDealer.status.tmpHp.Value = _shortRangeDealer.status.maxHp;
            _longRangeDealer.status.tmpHp.Value = _longRangeDealer.status.maxHp;
            _healer.status.tmpHp.Value = _healer.status.maxHp;
        }
        
        public StageInfo GetCurrentStage()
        {
            return _stages[currentStage.Value];
        }
        
        [Serializable]
        public class Settings
        {
            public List<StageInfo> stages;
        }  
    }
}