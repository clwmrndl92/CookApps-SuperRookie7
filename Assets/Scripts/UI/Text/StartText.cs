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
    // 게임시작 대기상태때 뜨는 텍스트
    public class StartText : MonoBehaviour
    {
        private GameController _gameController;
        private GameObject _backgroundGameObject;
        
         [Inject]
         public void Construct(GameController gameController)
         {
             _gameController = gameController;
         }
        
        void Start()
        {
            _backgroundGameObject = transform.GetChild(0).gameObject;
            
            _gameController.state
                .Subscribe(value =>
                {
                    if (value == GameStates.WaitingToStart) _backgroundGameObject.SetActive(true);
                    else if(value != GameStates.Waiting) _backgroundGameObject.SetActive(false);;
                });
            
        }
        
    }
}
