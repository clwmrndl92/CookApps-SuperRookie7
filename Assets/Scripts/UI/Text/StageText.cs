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
    // 스테이지 이름 표시
    public class StageText : MonoBehaviour
    {
        private GameController _gameController;
        private TextMeshProUGUI _text;
        
         [Inject]
         public void Construct(GameController gameController)
         {
             _gameController = gameController;
         }
        
        void Start()
        {
            _text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            
            _gameController.currentStage
                .SubscribeToText(_text, _=>_gameController.GetCurrentStage().name);
            
        }
        
    }
}
