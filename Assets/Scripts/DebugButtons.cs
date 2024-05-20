using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class DebugButtons : MonoBehaviour
    {
        [Inject]
        private GameController controller;
        [Inject]
        private MonsterSpawnController _monsterSpawnController;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void NextStage()
        {
            controller.StageCler();
        }

        public void Boss()
        {
            _monsterSpawnController.currentMonsterKills.Value =
                controller.GetCurrentStage().monsterSetting.requiredMonsterKills;
        }
    }
}
