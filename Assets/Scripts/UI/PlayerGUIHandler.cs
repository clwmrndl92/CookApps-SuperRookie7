using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    // 플레이어 정보, 재화 등 플레이어 관련 UI들
    public class PlayerGUIHandler : MonoBehaviour
    {
        public RectTransform levelInfo;
        public RectTransform expInfo;
        public RectTransform goldInfo;

        private PlayerInfo _playerInfo;
        private FloatingText.Factory _floatingFactory;

        [Inject]
        public void Construct(PlayerInfo playerInfo, FloatingText.Factory floatingFactory)
        {
            _playerInfo = playerInfo;
            _floatingFactory = floatingFactory;
        }

        void Start()
        {
            SubscribeExpBar(expInfo);
            SubscribeLevelText(levelInfo);
            SubscribeGoldText(goldInfo);
        }

        private void SubscribeExpBar(RectTransform container)
        {
            RectTransform expBar = container.Find("ExpBarImage").GetComponent<RectTransform>();
            TextMeshProUGUI expText = container.Find("ExpText").GetComponent<TextMeshProUGUI>();
            float maxSize = expBar.sizeDelta.x;
        
            _playerInfo.exp
                .Subscribe(value =>
                {
                    expBar.sizeDelta = new Vector2(maxSize * Mathf.Clamp(((float)_playerInfo.exp.Value / _playerInfo.nextExp), 0, 1),
                        expBar.sizeDelta.y);
                    expText.text = $"{_playerInfo.exp.Value} / {_playerInfo.nextExp}";
                });
        }
        
        private void SubscribeLevelText(RectTransform container)
        {
            RectTransform levelTagText = container.Find("LevelTagText").GetComponent<RectTransform>();
            TextMeshProUGUI levelText = container.Find("LevelText").GetComponent<TextMeshProUGUI>();
            _playerInfo.level
                .Subscribe(value =>
                {
                    levelText.text = value.ToString();
                    if(value > 1) _floatingFactory.Create().SetText("Level Up!", levelTagText.position, 0x0000FF, isCanvasPos:true);
                });
        }
        private void SubscribeGoldText(RectTransform container)
        {
            TextMeshProUGUI goldText = container.Find("GoldText").GetComponent<TextMeshProUGUI>();
            _playerInfo.gold.SubscribeToText(goldText, value=> value.ToString());
        }
        
    }
}