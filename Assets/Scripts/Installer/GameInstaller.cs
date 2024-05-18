using System;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] 
        private Settings _settings;
        [Inject] 
        private Canvas _canvas;

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
            
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
            Container.Bind<InputState>().AsSingle();
        }

        private void InstallFactory()
        {
            Container.BindFactory<FloatingText, FloatingText.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(_settings.FloatingTextPrefab)
                    .UnderTransform(_canvas.transform));
            
            Container.BindFactory<Monster, Monster.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(_settings.GoblinPrefab)
                    .UnderTransformGroup("Monsters"));
            
            Container.BindFactory<ArrowProjectile, ArrowProjectile.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(2)
                    .FromComponentInNewPrefab(_settings.ArrowPrefab)
                    .UnderTransformGroup("Arrows"));
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerInfo>().AsSingle();
            // Container.Bind<IInitializable>().To<PlayerInfo>().AsSingle();
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject FloatingTextPrefab;
            public GameObject GoblinPrefab;
            public GameObject ArrowPrefab;
        }
    }
}