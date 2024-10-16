using System.Collections;
using System.Collections.Generic;
using ModestTree;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    // 게임 오버시 표시되는 텍스트
    public class GameOverText : MonoBehaviour
    {
        private GameController _gameController;
        private GameObject _textGameObject;
        
         [Inject]
         public void Construct(GameController gameController)
         {
             _gameController = gameController;
         }
        
        void Start()
        {
            _textGameObject = transform.GetChild(0).gameObject;
            
            _gameController.state
                .Subscribe(value =>
                {
                    if (value == GameStates.GameOver) _textGameObject.SetActive(true);
                    else _textGameObject.SetActive(false);
                });

        }
        
    }
}
