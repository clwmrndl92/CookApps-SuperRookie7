using UnityEngine;
using Zenject;
using System;
using UnityEngine.Serialization;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "ScriptableInstallers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        public GameInstaller.Settings gameInstallerSettings;
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
            public MonsterController.MonsterControllerSetting monsterControllerSetting;
            public MonsterGlobalSetting monsterGlobalSetting;
            public MonsterSetting goblinSetting;
        }


        public override void InstallBindings()
        {
            // Game
            Container.BindInstance(gameInstallerSettings);
            // Character
            Container.BindInstance(characterSettings.globalSettings).AsSingle();
            Container.BindInstance(characterSettings.tankerSettings).WithId("Tanker");
            Container.BindInstance(characterSettings.shortRangeDealerSettings).WithId("ShortRangeDealer");
            Container.BindInstance(characterSettings.longRangeDealerSettings).WithId("LongRangeDealer");
            Container.BindInstance(characterSettings.healerSettings).WithId("Healer");
            // Monster
            Container.BindInstance(monsterSettings.monsterControllerSetting).AsSingle();
            Container.BindInstance(monsterSettings.monsterGlobalSetting).AsSingle();
            Container.BindInstance(monsterSettings.goblinSetting).WithId("Goblin");
        }
    }
}