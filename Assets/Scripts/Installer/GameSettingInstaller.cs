using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "ScriptableObject/Installers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        public GameInstaller.Settings gameInstallerSettings;
        public GameController.Settings gameSettings;
        public MonsterSettings monsterSettings;
        public CharacterSettings characterSettings;

        [Serializable]
        public class CharacterSettings
        {
            public CharacterGlobalSetting globalSettings;
            public TankerSetting tankerSettings;
            public CharacterSetting shortRangeDealerSettings;
            public CharacterSetting longRangeDealerSettings;
            public CharacterSetting healerSettings;
        }
        [Serializable]
        public class MonsterSettings
        {
            public MonsterGlobalSetting monsterGlobalSetting;
            public MonsterInfo goblinInfo;
            public BossMonsterInfo kingGoblinInfo;
            public MonsterInfo flyingEyeInfo;
            public BossMonsterInfo kingFlyingEyeInfo;
        }


        public override void InstallBindings()
        {
            // Game
            Container.BindInstance(gameInstallerSettings);
            Container.BindInstance(gameSettings);
            // Character
            Container.BindInstance(characterSettings.globalSettings).AsSingle();
            Container.BindInstance(characterSettings.tankerSettings).WithId("Tanker");
            Container.BindInstance(characterSettings.shortRangeDealerSettings).WithId("ShortRangeDealer");
            Container.BindInstance(characterSettings.longRangeDealerSettings).WithId("LongRangeDealer");
            Container.BindInstance(characterSettings.healerSettings).WithId("Healer");
            // Monster
            Container.BindInstance(monsterSettings.monsterGlobalSetting).AsSingle();
            Container.BindInstance(monsterSettings.goblinInfo).WithId("Goblin");
            Container.BindInstance(monsterSettings.kingGoblinInfo).WithId("KingGoblin");
            Container.BindInstance(monsterSettings.flyingEyeInfo).WithId("FlyingEye");
            Container.BindInstance(monsterSettings.kingFlyingEyeInfo).WithId("KingFlyingEye");
            InstallMonsterFactory();

        }
        
        private void InstallMonsterFactory()
        {
            Container.BindInterfacesAndSelfTo<MonsterFactory>().AsSingle();
            
            Container.BindFactory<Monster, Monster.Factory>()
                .WithId("GoblinFactory")
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(monsterSettings.goblinInfo.prefab)
                    .UnderTransformGroup("Monsters"));
            
            Container.BindFactory<Monster, Monster.Factory>()
                .WithId("FlyingEyeFactory")
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(monsterSettings.flyingEyeInfo.prefab)
                    .UnderTransformGroup("Monsters"));

            Container.BindFactory<BossMonster, BossMonster.Factory>()
                .WithId("KingGoblinFactory")
                .FromComponentInNewPrefab(monsterSettings.kingGoblinInfo.prefab)
                .UnderTransformGroup("Monsters");
            
            Container.BindFactory<BossMonster, BossMonster.Factory>()
                .WithId("KingFlyingEyeFactory")
                .FromComponentInNewPrefab(monsterSettings.kingFlyingEyeInfo.prefab)
                .UnderTransformGroup("Monsters");
        }

    }
}