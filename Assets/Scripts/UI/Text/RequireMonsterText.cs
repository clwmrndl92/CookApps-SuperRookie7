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
    public class RequireMonsterText : MonoBehaviour
    {
        [Inject]
        private MonsterSpawnController _monsterSpawnController;
        [Inject]
        private GameController _gameController;
        private TextMeshProUGUI _text;
        
        void Start()
        {
            _text = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            _monsterSpawnController.currentMonsterKills
                .SubscribeToText(_text, kill =>
                {
                    string tag = _gameController.GetCurrentStage().bossSetting.isBossSpawn ? "Boss!" : "Clear!";
                    return  $"{tag}\n{kill} / {_gameController.GetCurrentStage().monsterSetting.requiredMonsterKills}";
                });
            
        }
        
    }
}
