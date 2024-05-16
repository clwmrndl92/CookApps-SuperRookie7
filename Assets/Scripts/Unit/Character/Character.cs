using UnityEngine;
using System;
using System.Collections.Generic;
using LineUpHeros;
using Zenject;

namespace LineUpHeros
{
    public abstract class Character : Unit, IDamagable
    {
        
        public CharacterStatus status => (CharacterStatus)_status;
        public bool canAttack { get; private set; }
        public bool canSkill { get; private set; }
        private bool _isDead = false;
        public bool isDead { get => _isDead;  set => _isDead = value; }
        
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
        
        #region IDamagable
        public override void TakeHeal(int healAmount)
        {
            _status.tmpHp += healAmount;
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp -= damage; 
            // Debug.Log(gameObject.name + " Take Damage " + damage + " HP : " + _status.tmpHp);
            if (_status.tmpHp <= 0)
            {
                Die();
            }
        }
        public override void TakeStun(float stunTime)
        {
        }

        #endregion
        
        #region public Methods
        
        public override void AnimEventAttack()
        {
            BaseState attackState = _stateMachine.GetState(EnumState.Character.ATK);
            if (_stateMachine.currentState == attackState)
            {
                ((CharAtkState) attackState).Attack();
            }
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
        public virtual void Die()
        {
            _isDead = true;
        }
        #endregion

        #region Util

        public List<IDamagable> DetectMonsters(float radius)
        { 
            List<IDamagable> aliveCharacterList = new List<IDamagable>();
            foreach (var character in Util.GetDetectDamagableList(position, radius, LayerMasks.Monster))
            {
                if (character.isDead) continue;
                aliveCharacterList.Add(character);
            }
            return aliveCharacterList;
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (status != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(position, status.atkRange);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(position, status.detectRange);
            }
            #endif
        }

        #endregion
    }
            
    #region Status

    public class CharacterStatus : Status
    {
        private CharacterGlobalSetting _globalSetting;
        public float detectRange => _globalSetting.detectRange;
        public float moveVelocity => _globalSetting.moveVelocity;
        public float revivalTime => _globalSetting.revivalTime;

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

        public CharacterStatus(CharacterSetting settings, CharacterGlobalSetting globalSetting) : base(settings)
        {
            _baseSkillRange = settings.baseSkillRange;
            _baseSkillCool = settings.baseSkillCool;

            _globalSetting = globalSetting;
        }

    }
    #endregion
    
    #region setting
    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class CharacterSetting : StatSettings
    {
        // 스킬 관련 변수 추가
        public int baseSkillRange;
        public float baseSkillCool;
    }

    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class CharacterGlobalSetting
    {
        // 몬스터 감지 범위
        public float detectRange;
        public float moveVelocity;
        public float revivalTime;
    }
    #endregion
    
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
    
    // todo : 안쓸것 같으면 삭제하기
    public class CharacterFSMGlobalParameter : FSMGlobalParameter
    {
        public List<IDamagable> detectTargetList;
        public List<IDamagable> attackTargetList;
    }
}