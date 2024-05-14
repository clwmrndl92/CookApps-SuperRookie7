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
            StatSettings statSettings = new StatSettings();
            statSettings.baseHp = _settings.baseHp;
            statSettings.baseAtk = _settings.baseAtk;
            statSettings.baseAtkRange = _settings.baseAtkRange;
            statSettings.baseAtkCool = 1 / _settings.baseAtkPerSec;
            _status = new MonsterStatus(statSettings);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.maxHp);
        }
    }
}