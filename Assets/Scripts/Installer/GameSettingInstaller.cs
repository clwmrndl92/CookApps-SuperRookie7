using UnityEngine;
using Zenject;
using System;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "ScriptableInstallers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        public GameInstaller.Settings gameInstallerSettings;
        public AllCharacterSettings characterSettings;
        public Monster.Settings monsterSettings;

        [Serializable]
        public class AllCharacterSettings
        {
            public Character.Settings TankerSettings;
            public Character.Settings ShortRangeDealerSettings;
            public Character.Settings LongRangeDealerSettings;
            public Character.Settings HealerSettings;
        }


        public override void InstallBindings()
        {
            // Game
            Container.BindInstance(gameInstallerSettings);
            // Character
            Container.BindInstance(characterSettings.TankerSettings).WithId("Tanker");
            Container.BindInstance(characterSettings.ShortRangeDealerSettings).WithId("ShortRangeDealer");
            Container.BindInstance(characterSettings.LongRangeDealerSettings).WithId("LongRangeDealer");
            Container.BindInstance(characterSettings.HealerSettings).WithId("Healer");
            // Monster
            Container.BindInstance(monsterSettings).WithId("Goblin");
        }
    }
}