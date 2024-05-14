using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class HealerCharacter : Character
    {
        [Inject(Id = "Healer")]
        private Settings _settings;

        protected override void InitStatus()
        {
            _status = new Status(_settings);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.hp);
        }
    }
}