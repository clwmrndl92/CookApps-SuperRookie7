using System;
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
        private InputState _inputState;
        
        private TankerCharacter _tanker;
        private ShortRangeDealerCharacter _shortRangeDealer;
        private LongRangeDealerCharacter _longRangeDealer;
        private HealerCharacter _healer;
        
        public GameController(InputState inputState, TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer,
                              LongRangeDealerCharacter longRangeDealer, HealerCharacter healer) 
        {
            _inputState = inputState;
            
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;
        }

        public ReactiveProperty<GameStates> state { get; private set; } = new ReactiveProperty<GameStates>(GameStates.WaitingToStart);

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
                state.Value = GameStates.WaitingToStart;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            Debug.Log("start Game");
            Assert.That(state.Value == GameStates.WaitingToStart || state.Value == GameStates.GameOver);

            state.Value = GameStates.Playing;
        }
        
        
        private void GameOver()
        {
            Debug.Log("Game over");
            Assert.That(state.Value == GameStates.Playing);

            state.Value = GameStates.GameOver;
        }
    }
}