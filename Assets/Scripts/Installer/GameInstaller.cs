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
            InstallSignals();
            InstallFactory();
            InstallPlayer();
        }

        private void InstallController()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MonsterSpawnController>().AsSingle();
            
            // input 관련
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
            Container.Bind<InputState>().AsSingle();
        }

        void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<GameEvent.StageStartSignal>();
            Container.DeclareSignal<GameEvent.MonsterDieSignal>();
            Container.DeclareSignal<GameEvent.BossDieSignal>();
        }
        private void InstallFactory()
        {
            Transform floatingTextTransform = _canvas.transform.Find("FloatingText");
            Container.BindFactory<FloatingText, FloatingText.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(5)
                    .FromComponentInNewPrefab(_settings.FloatingTextPrefab)
                    .UnderTransform(floatingTextTransform));
            
            Container.BindFactory<ArrowProjectile, ArrowProjectile.Factory>()
                .FromMonoPoolableMemoryPool(poolBinder => poolBinder.WithInitialSize(2)
                    .FromComponentInNewPrefab(_settings.ArrowPrefab)
                    .UnderTransformGroup("Arrows"));
        }

        private void InstallPlayer()
        {
            Container.Bind<PlayerInfoController>().AsSingle();
        }
        
        [Serializable]
        public class Settings
        {
            public GameObject FloatingTextPrefab;
            public GameObject ArrowPrefab;
        }
    }
}