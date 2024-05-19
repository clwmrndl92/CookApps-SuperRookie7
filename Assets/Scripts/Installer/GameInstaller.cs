using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] 
        private Settings _settings;
        [Inject] 
        private Canvas _canvas; // floating text 그릴 캔버스

        public override void InstallBindings()
        {
            InstallController();
            InstallFactory();
            InstallPlayer();
        }

        private void InstallController()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterController>().AsSingle();
            
            // input 관련
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
            Container.Bind<InputState>().AsSingle();
        }

        private void InstallFactory()
        {
            Container.BindFactory<FloatingText, FloatingText.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(_settings.FloatingTextPrefab)
                    .UnderTransform(_canvas.transform));
            
            
            Container.BindFactory<ArrowProjectile, ArrowProjectile.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(2)
                    .FromComponentInNewPrefab(_settings.ArrowPrefab)
                    .UnderTransformGroup("Arrows"));
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerInfo>().AsSingle();
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject FloatingTextPrefab;
            public GameObject ArrowPrefab;
        }
    }
}