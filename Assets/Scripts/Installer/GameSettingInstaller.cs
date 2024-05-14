using UnityEngine;
using Zenject;
using System;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "ScriptableInstallers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        public GameInstaller.Settings gameInstallerSettings;
        public CharacterSettings characterSettings;

        [Serializable]
        public class CharacterSettings
        {
            public Character.Settings TankerSettings;
            public Character.Settings ShortRangeDealerSettings;
            public Character.Settings LongRangeDealerSettings;
            public Character.Settings HealerSettings;
        }


        public override void InstallBindings()
        {
            Container.BindInstance(gameInstallerSettings);
        }
    }
}