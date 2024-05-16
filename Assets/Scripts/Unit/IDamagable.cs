using System;
using UnityEngine;

namespace LineUpHeros
{
    public interface IDamagable
    {
        // 프로퍼티
        GameObject gameObjectIDamagable { get; }
        public bool isDead { get; set; }
        Status status { get; set; }
        // 메소드
        void TakeHeal(int healAmount);
        void TakeDamage(int damage);
        void TakeStun(float stunTime);
    }
    
    public class Status
    {
        private int _tmpHp;
        public int tmpHp
        {
            get => _tmpHp;
            set => _tmpHp = Mathf.Max(0, Mathf.Min(value, maxHp)); // 0 < tmpHp < maxHP
        }

        // Final Stat Property
        public int maxHp => (int) GetFinalStat(_baseHp, _addHp, _addPerHp);
        public int atk => (int) GetFinalStat(_baseAtk, _addAtk, _addPerAtk);
        public int atkRange => (int) GetFinalStat(_baseAtkRange, _addAtkRange, _addPerAtkRange);
        public float atkCool => (int) GetFinalStat(_baseAtkCool, _addAtkCool, _addPerAtkCool);
        
        // Base Stat, 영구 성장치 적용
        private int _baseHp;
        private int _baseAtk;
        private int _baseAtkRange;
        private float _baseAtkCool;
        
        // Additional Stat, 아이템, 버프 등 일시적 성장치
        private int _addHp;
        private int _addAtk;
        private int _addAtkRange;
        private float _addAtkCool;
        
        // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
        private int _addPerHp;
        private int _addPerAtk;
        private int _addPerAtkRange;
        private float _addPerAtkCool;

        public Status(StatSettings settings)
        {
            _baseHp = settings.baseHp;
            _baseAtk = settings.baseAtk;
            _baseAtkRange = settings.baseAtkRange;
            _baseAtkCool = settings.baseAtkCool;
            tmpHp = maxHp;
        }
        protected float GetFinalStat(float baseStat, float addStat, float addPerStat)
        {
            // Final Stat 계산식
            return baseStat + addStat + ((baseStat + addStat) * (addPerStat / 100));
        }
    }
    
    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class StatSettings
    {
        public int baseHp;
        public int baseAtk;
        public int baseAtkRange;
        public float baseAtkCool;
    }

}