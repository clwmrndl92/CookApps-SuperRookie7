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
    // 캐릭터들 체력바 표시
    public class HealthBarContainer : MonoBehaviour
    {
        public RectTransform tankerHealthBarContainer;
        public RectTransform SRDHealthBarContainer;
        public RectTransform LRDHealthBarContainer;
        public RectTransform healerHealthBarContainer;

        TankerCharacter _tanker;
        ShortRangeDealerCharacter _shortRangeDealer;
        LongRangeDealerCharacter _longRangeDealer;
        HealerCharacter _healer;

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
            
            SubscribeHealthBar(tankerHealthBarContainer, _tanker);
            SubscribeHealthBar(SRDHealthBarContainer, _shortRangeDealer);
            SubscribeHealthBar(LRDHealthBarContainer, _longRangeDealer);
            SubscribeHealthBar(healerHealthBarContainer, _healer);
            
            SubscribeSkillCool(tankerHealthBarContainer, _tanker);
            SubscribeSkillCool(SRDHealthBarContainer, _shortRangeDealer);
            SubscribeSkillCool(LRDHealthBarContainer, _longRangeDealer);
            SubscribeSkillCool(healerHealthBarContainer, _healer);
            
        }
        
        // UI와 데이터 연결 (uniRX)
        private void SubscribeHealthBar(RectTransform container, Unit unit)
        {
            RectTransform healthBarContainer = container.Find("HealthBar").GetComponent<RectTransform>();

            Image barImage = healthBarContainer.Find("Bar").GetComponent<Image>();
            TextMeshProUGUI hpText = healthBarContainer.Find("HPText").gameObject.GetComponent<TextMeshProUGUI>();

            // 체력 텍스트 표시
            unit.status.tmpHp.SubscribeToText(hpText, value => string.Format("{0} / {1}", Mathf.Clamp(value, 0, unit.status.maxHp), unit.status.maxHp));

            // 체력바 사이즈 조절
            unit.status.tmpHp
                .Subscribe(value =>
                {
                    barImage.fillAmount = Mathf.Clamp(((float)unit.status.tmpHp.Value / unit.status.maxHp), 0, 1);
                });
            
            // 죽고 부활할 때 표시되는 텍스트
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
        // UI와 데이터 연결 (uniRX)
        private void SubscribeSkillCool(RectTransform container, Character character)
        {
            CoolTimeImage coolTimeImage = container.Find("CoolTime").GetComponent<CoolTimeImage>();
            
            character.isSkillUse.Subscribe(value =>
            {
                if (value)
                {
                    coolTimeImage.cooltime = character.status.skillCool;
                    coolTimeImage.gameObject.SetActive(true);
                }
                else
                {
                    coolTimeImage.gameObject.SetActive(false);
                }
            });



        }
    }
 
}
