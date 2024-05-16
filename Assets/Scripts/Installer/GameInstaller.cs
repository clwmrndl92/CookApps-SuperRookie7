using System;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] 
        private Settings _settings;

        public override void InstallBindings()
        {
            // Container.BindFactory<Transform, ExplosionFactory>()
            //     .FromComponentInNewPrefab(_settings.ExplosionPrefab);
        }


        [Serializable]
        public class Settings
        {
            public GameObject KnightPrefab;
        }
    }
}