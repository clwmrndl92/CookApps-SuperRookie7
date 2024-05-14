using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class ShortRangeDealerCharacter : Character
    {
        [Inject(Id = "ShortRangeDealer")]
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