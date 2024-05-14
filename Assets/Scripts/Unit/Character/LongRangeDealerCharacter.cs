using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class LongRangeDealerCharacter : Character
    {
        [Inject(Id = "LongRangeDealer")]
        private Settings _settings;

        protected override void InitStatus()
        {
            _status = new CahracterStatus(_settings);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.maxHp);
        }
    }
}