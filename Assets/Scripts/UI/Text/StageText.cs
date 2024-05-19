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
