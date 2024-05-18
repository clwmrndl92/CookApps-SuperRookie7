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
    public class HealthBarContainer : MonoBehaviour
    {
        // Presenter는 View를 알고 있다(인스펙터를 통해 바인딩 한다)
        public RectTransform tankerHealthBarContainer;
        public RectTransform SRDHealthBarContainer;
        public RectTransform LRDHealthBarContainer;
        public RectTransform healerHealthBarContainer;

        // Model의 변화는 ReactiveProperty를 통해 알 수 있다.
        TankerCharacter _tanker;
        ShortRangeDealerCharacter _shortRangeDealer;
        LongRangeDealerCharacter _longRangeDealer;
        HealerCharacter _healer;
        // private Unit _unit;

        [Inject]
        public void Construct(TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer,
                LongRangeDealerCharacter longRangeDealer, HealerCharacter healer)
        {
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;
        }

        void Start()
        {
            // // Rx는 View와 Model의 사용자 이벤트를 제공한다.
            // MyButton.OnClickAsObservable().Subscribe(_ => enemy.CurrentHp.Value -= 99);
            // MyToggle.OnValueChangedAsObservable().SubscribeToInteractable(MyButton);

            // Model들은 Rx를 통해 Presenter에게 자신의 변화를 알리고, Presenter은 Viw를 업데이트 한다.
            SubscribeHealthBar(tankerHealthBarContainer, _tanker);
            SubscribeHealthBar(SRDHealthBarContainer, _shortRangeDealer);
            SubscribeHealthBar(LRDHealthBarContainer, _longRangeDealer);
            SubscribeHealthBar(healerHealthBarContainer, _healer);

            SubscribeExpBar(tankerHealthBarContainer, _tanker);
            SubscribeExpBar(SRDHealthBarContainer, _shortRangeDealer);
            SubscribeExpBar(LRDHealthBarContainer, _longRangeDealer);
            SubscribeExpBar(healerHealthBarContainer, _healer);

            SubscribeLevelText(tankerHealthBarContainer, _tanker);
            SubscribeLevelText(SRDHealthBarContainer, _shortRangeDealer);
            SubscribeLevelText(LRDHealthBarContainer, _longRangeDealer);
            SubscribeLevelText(healerHealthBarContainer, _healer);
        }

        private void SubscribeHealthBar(RectTransform container, Unit unit)
        {
            RectTransform healthBarContainer = container.Find("HealthBar").GetComponent<RectTransform>();

            RectTransform healthBar = healthBarContainer.Find("Bar").GetComponent<RectTransform>();
            TextMeshProUGUI hpText = healthBarContainer.Find("HPText").gameObject.GetComponent<TextMeshProUGUI>();

            unit.status.tmpHp.SubscribeToText(hpText, value => string.Format("{0} / {1}", Mathf.Clamp(value, 0, unit.status.maxHp), unit.status.maxHp));

            float maxSize = healthBarContainer.sizeDelta.x;
            unit.status.tmpHp
                .Subscribe(value =>
                {
                    healthBar.sizeDelta = new Vector2(maxSize * Mathf.Clamp(((float)unit.status.tmpHp.Value / unit.status.maxHp), 0, 1),
                        healthBar.sizeDelta.y);
                });

            GameObject reviveTextObj = healthBarContainer.transform.Find("ReviveText").gameObject;
            unit.isDead
                .Subscribe(value =>
                {
                    if (value == true)
                    {
                        reviveTextObj.SetActive(true);
                        hpText.gameObject.SetActive(false);
                    }
                    else
                    {
                        reviveTextObj.SetActive(false);
                        hpText.gameObject.SetActive(true);
                    }
                });
        }
        private void SubscribeExpBar(RectTransform container, Unit unit)
        {
            RectTransform expBarContainer = container.Find("ExpBar").GetComponent<RectTransform>();
            RectTransform expBar = expBarContainer.Find("ExpBarImage").GetComponent<RectTransform>();
            float maxSize = expBarContainer.sizeDelta.x;

            unit.status.exp
                .Subscribe(value =>
                {
                    expBar.sizeDelta = new Vector2(maxSize * Mathf.Clamp(((float)unit.status.exp.Value / unit.status.nextExp), 0, 1),
                        expBar.sizeDelta.y);
                });
        }

        private void SubscribeLevelText(RectTransform container, Unit unit)
        {
            TextMeshProUGUI levelText = container.Find("Symbol").Find("LevelText").GetComponent<TextMeshProUGUI>();
            unit.status.level
                .SubscribeToText(levelText,
                    text => "LV " + text.ToString()
                );
        }
    }

}
