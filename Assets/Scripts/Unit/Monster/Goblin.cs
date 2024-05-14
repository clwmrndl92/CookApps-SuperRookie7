using UnityEngine;
using System;
using Zenject;

namespace LineUpHeros
{
    public class Goblin : Monster
    {
        [Inject(Id = "Goblin")]
        private Settings _settings;
        protected override void InitStatus()
        {
            _status = new MonsterStatus(_settings);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.maxHp);
        }
    }
}