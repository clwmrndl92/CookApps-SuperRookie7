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
        private bool _isDead = false;
        public bool isDead => _isDead;
        
        public bool canSkill
        {
            get
            {
                // 스킬 범위내 몬스터 체크
                List<IDamagable> monsters = DetectMonsters(status.skillRange);
                if (monsters.Count == 0) return false;
                bool isInRange = Vector3.Distance(position, monsters[0].gameObjectIDamagable.transform.position) <=
                                 status.skillRange;
                // 스킬 쿨타임 체크
                CharSpecialAtkState skillState = (CharSpecialAtkState)_stateMachine.GetState(EnumState.Character.SPECIAL_ATK);
                
                return isInRange && skillState.isCool == false;
            }
        }
        
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine( new FSMCharacterGlobalVariables());
            _stateMachine.AddState(EnumState.Character.IDLE, new CharIdleState(this));
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
        // Animation Event에서 호출하는 함수
        public override void AnimEventAttack()
        {
            BaseState attackState = _stateMachine.GetState(EnumState.Character.ATK);
            if (_stateMachine.currentState == attackState)
            {
                ((CharAtkState) attackState).Attack();
            }
        }
        public override void AnimEventSpecialAttack()
        {
            BaseState specialAttackState = _stateMachine.GetState(EnumState.Character.SPECIAL_ATK);
            if (_stateMachine.currentState == specialAttackState)
            {
                ((CharSpecialAtkState) specialAttackState).SpecialAttack();
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
        public virtual void Revive()
        {
            _isDead = false;
        }
        #endregion

        #region Util
        // 범위내 살아있는 몬스터 리스트 찾아서 리턴 (거리순 정렬)
        public List<IDamagable> DetectMonsters(float radius)
        { 
            List<IDamagable> aliveMonsterList = new List<IDamagable>();
            foreach (var monster in Util.GetDetectDamagableList(position, radius, LayerMasks.Monster))
            {
                if (monster.isDead) continue;
                aliveMonsterList.Add(monster);
            }
            return aliveMonsterList;
        }

        private void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            if (status != null)
            { 
                // 공격범위 파랑
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(position, status.atkRange);
                // 감지범위 노랑
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

    [Serializable]
    public class CharacterGlobalSetting
    {
        // 몬스터 감지 범위
        public float detectRange;
        // 이동속도
        public float moveVelocity;
        // 부활시간
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
    
    // Character 스테이트머신 글로벌 변수
    public class FSMCharacterGlobalVariables : FSMGlobalVariables
    {
        public List<IDamagable> detectTargetList;
    }
}