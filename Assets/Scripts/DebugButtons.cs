using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class DebugButtons : MonoBehaviour
    {
        // 디버그용 에디터 버튼
        #if UNITY_EDITOR
        [Inject]
        private GameController controller;
        [Inject]
        private MonsterSpawnController _monsterSpawnController;

        public void NextStage()
        {
            controller.StageCler();
        }

        public void Boss()
        {
            _monsterSpawnController.currentMonsterKills.Value =
                controller.GetCurrentStage().monsterSetting.requiredMonsterKills;
        }
        #endif
    }
}
