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
            UnitSettings unitSettings = new UnitSettings();
            unitSettings.baseHp = _settings.baseHp;
            unitSettings.baseAtk = _settings.baseAtk;
            unitSettings.baseAtkRange = _settings.baseAtkRange;
            unitSettings.baseAtkCool = 1 / _settings.baseAtkPerSec;
            _status = new MonsterStatus(unitSettings);
        }

        private void Start()
        {
            Debug.Log(gameObject.name);
            Debug.Log(_status.maxHp);
        }
    }
}