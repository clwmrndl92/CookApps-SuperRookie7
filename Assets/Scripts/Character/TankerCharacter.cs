using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class TankerCharacter : Character
    {
        [Inject(Id = "Tanker")]
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