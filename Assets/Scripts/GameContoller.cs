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
        Waiting,
        Playing,
        GameOver,
        GameClear
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
                case GameStates.Waiting:
                {
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
                case GameStates.GameClear:
                {
                    UpdateGameClear();
                    break;
                }
                default:
                {
                    Assert.That(false);
                    break;
                }
            }
        }

        private void UpdateGameClear()
        {
            Assert.That(state.Value == GameStates.GameClear);
            
            if (_inputState.IsMouseClick)
            {
                state.Value = GameStates.Waiting;
                Time.timeScale = 1;
                _fadeInOutController.StartEffect(() =>
                    {
                        state.Value = GameStates.WaitingToStart;
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    },
                                        0.5f, 1f,0.5f);
            }
        }
        private void UpdateGameOver()
        {
            Assert.That(state.Value == GameStates.GameOver);


            if (_inputState.IsMouseClick)
            {
                Time.timeScale = 1;
                
                state.Value = GameStates.Waiting;
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

            if (_inputState.IsMouseClick)
            {
                state.Value = GameStates.Waiting;
                StartGame();
            }
        }

        private void StartGame()
        {
            StageStart(0);
        }
        
        
        private void GameOver()
        {
            Debug.Log("Game over");
            Time.timeScale = 0;

            state.Value = GameStates.GameOver;
        }
        
        private void GameClear()
        {
            Debug.Log("Game Clear");
            _tanker.Victory();
            _shortRangeDealer.Victory();
            _longRangeDealer.Victory();
            _healer.Victory();
            state.Value = GameStates.GameClear;
        }
        public void StageCler()
        {
            Debug.Log("stage clear " + GetCurrentStage().name);
            state.Value = GameStates.Waiting;
            if (currentStage.Value+1 == _stages.Count)
            {
                GameClear();
                // StageStart(currentStage.Value);
                return;
            }
            StageStart(++currentStage.Value);
        }
        
        public void StageStart(int stageNum)
        {
            _fadeInOutController.StartEffect(() =>
                {
                    state.Value = GameStates.Playing;
                    currentStage.Value = stageNum;
                    _signalBus.Fire<GameEvent.StageStartSignal>();
                    ResetCharacter();
                }
            ,0.5f, 1f,0.5f);
            Debug.Log("stage start " + GetCurrentStage().name);
        }

        private void ResetCharacter()
        {
            _tanker.Reset();
            _shortRangeDealer.Reset();
            _longRangeDealer.Reset();
            _healer.Reset();
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