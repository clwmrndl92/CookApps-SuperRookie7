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
    // 게임 클리어시 표시되는 텍스트
    public class GameClearText : MonoBehaviour
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
                    if (value == GameStates.GameClear) _textGameObject.SetActive(true);
                    else _textGameObject.SetActive(false);
                });

        }
        
    }
}
