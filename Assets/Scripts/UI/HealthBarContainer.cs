using System.Collections;
using System.Collections.Generic;
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
        public RectTransform tankerHealthBar;
        public RectTransform SRDHealthBar;
        public RectTransform LRDHealthBar;
        public RectTransform healerHealthBar;

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
            SubscribeHealthBar(tankerHealthBar, _tanker);
            SubscribeHealthBar(SRDHealthBar, _shortRangeDealer);
            SubscribeHealthBar(LRDHealthBar, _longRangeDealer);
            SubscribeHealthBar(healerHealthBar, _healer);
        }

        private void SubscribeHealthBar(RectTransform healthBar, Unit unit)
        {
            TextMeshProUGUI hpText =healthBar.transform.Find("HPText").gameObject.GetComponent<TextMeshProUGUI>();
            unit.status.tmpHp.SubscribeToText(hpText, value=>string.Format("{0} / {1}", Mathf.Clamp(value, 0, unit.status.maxHp), unit.status.maxHp));
            
            RectTransform bar = healthBar.transform.Find("Bar").gameObject.GetComponent<RectTransform>();
            float maxSize = healthBar.sizeDelta.x;
            unit.status.tmpHp
                .Subscribe(value =>
                {
                    bar.sizeDelta = new Vector2(maxSize * Mathf.Clamp(((float)unit.status.tmpHp.Value / unit.status.maxHp),0,1),
                        bar.sizeDelta.y);
                });
            
            GameObject reviveTextObj =healthBar.transform.Find("ReviveText").gameObject;
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
        
    }
}
