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
                    Debug.Log(value);
                    if (value == GameStates.GameOver) _textGameObject.SetActive(true);
                    else _textGameObject.SetActive(false);
                });

        }
        
    }
}
