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
    // todo : 스킬 쿨타임 표시 추가
    public class BossContainer : MonoBehaviour
    {
        private TextMeshProUGUI _requireMonsetText;
        private RectTransform _bossInfoContainer;
        
        [Inject]
        private MonsterSpawnController _monsterSpawnController;
        [Inject]
        private GameController _gameController;

        void Start()
        {
            _requireMonsetText = transform.Find("RequireMonsterText").GetComponent<TextMeshProUGUI>();
            
            _monsterSpawnController.currentMonsterKills
                 .SubscribeToText(_requireMonsetText, kill =>
                 {
                     string tag = _gameController.GetCurrentStage().bossSetting.isBossSpawn ? "Boss!" : "Clear!";
                     return  $"{tag}\n{kill} / {_gameController.GetCurrentStage().monsterSetting.requiredMonsterKills}";
                 });
            
            _bossInfoContainer = transform.Find("BossInfoContainer").GetComponent<RectTransform>();

            _monsterSpawnController.boss.Subscribe(bossMonster =>
            {
                if (bossMonster == null)
                {
                    _bossInfoContainer.gameObject.SetActive(false);
                }
                else
                {
                    _bossInfoContainer.gameObject.SetActive(true);
                    SubscribeHealthBar(_bossInfoContainer, bossMonster);
                }
            });
        }

        
        // UI와 데이터 연결 (uniRX)
        private void SubscribeHealthBar(RectTransform container, BossMonster boss)
        {
            TextMeshProUGUI nameText = container.Find("BossNameText").gameObject.GetComponent<TextMeshProUGUI>();
            nameText.text = boss.status.name;

            RectTransform healthBarContainer = container.Find("HealthBar").GetComponent<RectTransform>();
            Image healthBarImage = healthBarContainer.Find("Bar").GetComponent<Image>();
            TextMeshProUGUI hpText = healthBarContainer.Find("HPText").gameObject.GetComponent<TextMeshProUGUI>();
            
            // 체력 텍스트 표시
            boss.status.tmpHp.SubscribeToText(hpText,
                value => string.Format("{0} / {1}", Mathf.Clamp(value, 0, boss.status.maxHp), boss.status.maxHp));

            // 체력바 사이즈 조절
            boss.status.tmpHp
                .Subscribe(value =>
                {
                    healthBarImage.fillAmount = Mathf.Clamp(((float)boss.status.tmpHp.Value / boss.status.maxHp), 0, 1);
                });

        }
    }

}
