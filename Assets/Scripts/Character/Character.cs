using UnityEngine;
using System;

namespace LineUpHeros
{
    public abstract partial class Character : MonoBehaviour
    {
        protected StateMachine _stateMachine;
        protected Status _status;

        #region MonoBehaviour Method

        private void Awake()
        {
            InitAnim();
            InitStateMachine();
            InitStatus();
        }

        #endregion
        
        // InitStatus()에서 _status에 새 Status 인스턴스 할당 필요
        protected abstract void InitStatus();
        // protected abstract void Skill();
        
        // StateMachine
        private void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumState.Idle, new IdleState(this));
            _stateMachine.AddState(EnumState.Move, new MoveState(this));
            _stateMachine.AddState(EnumState.Atk, new AtkState(this));
            _stateMachine.AddState(EnumState.SpecialAtk, new SpecialAtkState(this));
            _stateMachine.AddState(EnumState.Dead, new DeadState(this));
            _stateMachine.AddState(EnumState.Hurt, new HurtState(this));
            _stateMachine.AddState(EnumState.Victory, new VictoryState(this));
        }

        #region Status
        protected class Status
        {
            // Final Stat Property
            public int hp => (int) GetFinalStat(_baseHp, _addHp, _addPerHp);
            public int atk => (int) GetFinalStat(_baseAtk, _addAtk, _addPerAtk);
            public int atkRange => (int) GetFinalStat(_baseAtkRange, _addAtkRange, _addPerAtkRange);
            public float atkCool => (int) GetFinalStat(_baseAtkCool, _addAtkCool, _addPerAtkCool);
            public int skillRange => (int) GetFinalStat(_baseSkillRange, _addSkillRange, _addPerSkillRange);
            public float skillCool => (int) GetFinalStat(_baseSkillCool, _addSkillCool, _addPerSkillCool);
            
            // Base Stat, 영구 성장치 적용
            private int _baseHp;
            private int _baseAtk;
            private int _baseAtkRange;
            private float _baseAtkCool;
            private int _baseSkillRange;
            private float _baseSkillCool;
            
            // Additional Stat, 아이템, 버프 등 일시적 성장치
            private int _addHp;
            private int _addAtk;
            private int _addAtkRange;
            private float _addAtkCool;
            private int _addSkillRange;
            private float _addSkillCool;
            
            // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
            private int _addPerHp;
            private int _addPerAtk;
            private int _addPerAtkRange;
            private float _addPerAtkCool;
            private int _addPerSkillRange;
            private float _addPerSkillCool;

            public Status(Settings settings)
            {
                _baseHp = settings.baseHp;
                _baseAtk = settings.baseAtk;
                _baseAtkRange = settings.baseAtkRange;
                _baseAtkCool = settings.baseAtkCool;
                _baseSkillRange = settings.baseSkillRange;
                _baseSkillCool = settings.baseSkillCool;
            }

            private float GetFinalStat(float baseStat, float addStat, float addPerStat)
            {
                // Final Stat 계산식
                return baseStat + addStat + ((baseStat + addStat) * (addPerStat / 100));
            }
        }
        #endregion
        
        // Scriptable Object Installer 세팅 값
        [Serializable]
        public class Settings
        {
            public int baseHp;
            public int baseAtk;
            public int baseAtkRange;
            public float baseAtkCool;
            public int baseSkillRange;
            public float baseSkillCool;
        }
    }
}