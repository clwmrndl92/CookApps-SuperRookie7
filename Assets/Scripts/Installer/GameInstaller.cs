using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] 
        private Settings _settings = null;

        public override void InstallBindings()
        {
        }


        [Serializable]
        public class Settings
        {
            public GameObject KnightPrefab;
        }
    }
}