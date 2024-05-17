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
            Container.BindFactory<FloatingText, FloatingText.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                                                                    .FromComponentInNewPrefab(_settings.FloatingTextPrefab)
                                                                    .UnderTransform(_canvas.transform));
        }


        [Serializable]
        public class Settings
        {
            public GameObject FloatingTextPrefab;
        }
        
        
        class FloatingTextPool : MonoPoolableMemoryPool<IMemoryPool, FloatingText>
        {
        }
    }
}