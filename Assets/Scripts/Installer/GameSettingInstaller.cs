
using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "ScriptableInstallers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {
        public GameInstaller.Settings GameInstallerSettings;



        public override void InstallBindings()
        {
            Container.BindInstance(GameInstallerSettings);
        }
        
    }
}