using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    // 강화창 관련 UI
    public class UpgradeGUIHandler : MonoBehaviour
    {
        // todo : 닫기 버튼 만들기
        public GameObject upgradePanel;
        public Button upgradePanelButton;
        
        private RectTransform _wallet;
        private Button _rubyButton;
        private Button _goldButton;
        private RectTransform _rubyUpgrade;
        private RectTransform _goldUpgrade;

        private PlayerInfo _playerInfo;

        [Inject]
        public void Construct(PlayerInfo playerInfo)
        {
            _playerInfo = playerInfo;
        }

        void Start()
        {

            _wallet = upgradePanel.transform.Find("Wallet").GetComponent<RectTransform>();
            _goldButton = upgradePanel.transform.Find("GoldButton").GetComponent<Button>();
            _rubyButton = upgradePanel.transform.Find("RubyButton").GetComponent<Button>();
            _goldUpgrade = upgradePanel.transform.Find("GoldUpgrade").GetComponent<RectTransform>();
            _rubyUpgrade = upgradePanel.transform.Find("RubyUpgrade").GetComponent<RectTransform>();
            
            upgradePanel.SetActive(false);
            upgradePanelButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    upgradePanel.SetActive(!upgradePanel.activeSelf);
                });
            
            SubscribeUpgradeTabs();
            SubscribeWallet(_wallet);
            // SubscribeGoldUpgrade(_goldUpgrade);
        }

        private void SubscribeUpgradeTabs()
        {
            _rubyButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _goldUpgrade.gameObject.SetActive(false);
                    _rubyUpgrade.gameObject.SetActive(true);
                });
            
            _goldButton.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _rubyUpgrade.gameObject.SetActive(false);
                    _goldUpgrade.gameObject.SetActive(true);
                });
        }

        private void SubscribeWallet(RectTransform container)
        {
            TextMeshProUGUI goldText = container.Find("Gold").transform.Find("GoldText").GetComponent<TextMeshProUGUI>();
            // TextMeshProUGUI rubyText = container.Find("Ruby").transform.Find("RubyText").GetComponent<TextMeshProUGUI>();
            _playerInfo.gold.SubscribeToText(goldText, value=> value.ToString());
        }
        
        private void SubscribeGoldUpgrade(RectTransform container)
        {
            // todo : 실제 강화와 연결
        }
        
    }
}