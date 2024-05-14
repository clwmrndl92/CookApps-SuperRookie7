using UnityEngine;
using System;

namespace LineUpHeros
{
    public abstract partial class Unit : MonoBehaviour
    {
        protected StateMachine _stateMachine;
        protected UnitStatus _status;

        #region MonoBehaviour Method

        private void Awake()
        {
            Init();
        }

        #endregion
        
        #region Initialize
        private void Init()
        {
            // 지켜야할 순서 Anim -> StateMachine
            InitAnim();
            InitStateMachine();
            InitStatus();
        }
        // InitStateMachine()에서 _stateMachine 새 StateMachine 인스턴스 할당 필요
        protected abstract void InitStateMachine();
        // InitStatus()에서 _status에 새 Status 인스턴스 할당 필요
        protected abstract void InitStatus();
        #endregion

    }
    #region Status
    public class UnitStatus
    {
        private int _tmpHp;
        public int tmpHp
        {
            get => _tmpHp;
            set => _tmpHp = Mathf.Max(0, Mathf.Min(value, maxHp)); // 0 <= tmpHp <= maxHP
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

        public UnitStatus(UnitSettings settings)
        {
            _baseHp = settings.baseHp;
            _baseAtk = settings.baseAtk;
            _baseAtkRange = settings.baseAtkRange;
            _baseAtkCool = settings.baseAtkCool;
        }

        protected float GetFinalStat(float baseStat, float addStat, float addPerStat)
        {
            // Final Stat 계산식
            return baseStat + addStat + ((baseStat + addStat) * (addPerStat / 100));
        }
    }
    #endregion
    
    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class UnitSettings
    {
        public int baseHp;
        public int baseAtk;
        public int baseAtkRange;
        public float baseAtkCool;
    }
}