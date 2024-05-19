using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using ModestTree;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    public class UpgradeContainer : MonoBehaviour
    {
        private UpgradeInfo _info;

        private void Start()
        {

        }

        public void SubscribeUpgradeInfo(UpgradeInfo info)
        {
            _info = info;

            // 화면에 보이는 텍스트
            TextMeshProUGUI infoText = transform.Find("Info").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = transform.Find("UpgradeButton").Find("CostText").GetComponent<TextMeshProUGUI>();

            // 골드/루비 이미지 선택
            Transform goldImage = transform.Find("UpgradeButton").Find("CostText").Find("GoldImage");
            Transform rubyImage = transform.Find("UpgradeButton").Find("CostText").Find("RubyImage");
            switch (info.costType)
            {
                case UpgradeCostType.Gold:
                    goldImage.gameObject.SetActive(true);
                    rubyImage.gameObject.SetActive(false);
                    break;
                case UpgradeCostType.Ruby:
                    goldImage.gameObject.SetActive(false);
                    rubyImage.gameObject.SetActive(true);
                    break;
            }

            // title은 고정
            // info / cost는 변경 가능
            titleText.text = info.title;
            info.cost.SubscribeToText(costText, value =>
                value.ToString()
            );
            info.info.SubscribeToText(infoText, value =>
                value
            );

            // 버튼 클릭시 업그레이드 함수 호출
            Button upgradeButton = transform.Find("UpgradeButton").GetComponent<Button>();
            upgradeButton.onClick.AddListener(() =>
            {
                info.TryUpgrade();
            });
        }


    }
}