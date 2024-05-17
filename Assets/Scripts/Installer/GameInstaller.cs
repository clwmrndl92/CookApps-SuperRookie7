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
        }

        private void InstallController()
        {
            Container.BindInterfacesAndSelfTo<MonsterController>().AsSingle();
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
        }

        [Serializable]
        public class Settings
        {
            public GameObject FloatingTextPrefab;
            public GameObject GoblinPrefab;
        }
        
        
        class FloatingTextPool : MonoPoolableMemoryPool<IMemoryPool, FloatingText>
        {
        }
        class MonsterPool : MonoPoolableMemoryPool<IMemoryPool, Monster>
        {
        }
    }
}