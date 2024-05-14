using UnityEngine;
using System;
using System.Collections.Generic;

namespace LineUpHeros
{
    public abstract class Character : Unit, IDamagable
    {
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumState.Character.IDLE, new CharIdleState(this), new CharacterFSMGlobalParameter());
            _stateMachine.AddState(EnumState.Character.MOVE, new CharMoveState(this));
            _stateMachine.AddState(EnumState.Character.ATK, new CharAtkState(this));
            _stateMachine.AddState(EnumState.Character.SPECIAL_ATK, new CharSpecialAtkState(this));
            _stateMachine.AddState(EnumState.Character.DEAD, new CharDeadState(this));
            _stateMachine.AddState(EnumState.Character.HURT, new CharHurtState(this));
            _stateMachine.AddState(EnumState.Character.VICTORY, new CharVictoryState(this));
            _stateMachine.ChangeState(EnumState.Character.IDLE);
        }
        
        #region Status
        public bool canAttack { get; private set; }
        public bool canSkill { get; private set; }
        protected class CharacterStatus : Status
        {
            // 스킬 관련 스탯 추가
            public int skillRange => (int) GetFinalStat(_baseSkillRange, _addSkillRange, _addPerSkillRange);
            public float skillCool => (int) GetFinalStat(_baseSkillCool, _addSkillCool, _addPerSkillCool);
            
            // Base Stat, 영구 성장치 적용
            private int _baseSkillRange;
            private float _baseSkillCool;
            
            // Additional Stat, 아이템, 버프 등 일시적 성장치
            private int _addSkillRange;
            private float _addSkillCool;
            
            // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
            private int _addPerSkillRange;
            private float _addPerSkillCool;

            public CharacterStatus(Settings settings) : base(settings)
            {
                _baseSkillRange = settings.baseSkillRange;
                _baseSkillCool = settings.baseSkillCool;
            }

        }
        
        // Scriptable Object Installer 세팅 값
        [Serializable]
        public class Settings : StatSettings
        {
            // 스킬 관련 변수 추가
            public int baseSkillRange;
            public float baseSkillCool;
        }
        #endregion
        
        #region IDamagable
        public override void TakeHeal(int healAmount)
        {
            _status.tmpHp += healAmount;
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp -= damage;
        }
        public override void TakeStun(float stunTime)
        {
        }

        // return true : 공격 성공함, return false : 공격대상 없음
        public virtual bool Attack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;
            
            atkRangeTargetList[0].TakeDamage(status.atk);
            return true;
        }
        
        // return true : 스킬 사용함, return false : 스킬 사용 안함
        public virtual bool SpecialAttack(List<IDamagable> atkRangeTargetList)
        {
            return false;
        }
        #endregion

        #region Util

        public List<IDamagable> DetectMonsters(float radius)
        { 
            return Util.GetDetectDamagableList(position, radius, LayerMasks.Monster);
        }

        #endregion

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (status != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(position, status.atkRange);
            }
            #endif
        }
    }
    public static partial class EnumState
    {
        public static class Character
        {
            public const string IDLE = "idle";
            public const string MOVE = "walk";
            public const string ATK = "attack";
            public const string SPECIAL_ATK = "casting";
            public const string DEAD = "die";
            public const string HURT = "hurt";
            public const string VICTORY = "victory";
        }
    }
    
    public class CharacterFSMGlobalParameter : FSMGlobalParameter
    {
        public List<IDamagable> detectTargetList;
        public List<IDamagable> attackTargetList;
    }
}