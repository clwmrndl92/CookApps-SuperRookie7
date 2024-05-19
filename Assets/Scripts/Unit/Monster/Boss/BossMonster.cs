using System;
using System.Collections.Generic;
using LineUpHeros;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public abstract class BossMonster : Monster
    {
        public bool canSkill
        {
            get
            {
                // 스킬 범위내 타겟 체크
                List<IDamagable> monsters = DetectCharacters(status.skillRange);
                if (monsters.Count == 0) return false;
                // 스킬 쿨타임 체크
                BossMonSpecialAtkState skillState = (BossMonSpecialAtkState)_stateMachine.GetState(EnumState.BossMonster.SKILL);
                return skillState.isCool == false;
            }
        }

        public BossMonsterStatus status => (BossMonsterStatus)_status;
       
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(new FsmMonsterGlobalVariables());
            _stateMachine.AddState(EnumState.BossMonster.IDLE, new BossMonIdleState(this));
            _stateMachine.AddState(EnumState.BossMonster.SKILL, new BossMonSpecialAtkState(this));
            _stateMachine.AddState(EnumState.BossMonster.MOVE, new MonMoveState(this));
            _stateMachine.AddState(EnumState.BossMonster.ATK, new MonAtkState(this));
            _stateMachine.AddState(EnumState.BossMonster.DEAD, new BossMonDeadState(this));
            _stateMachine.AddState(EnumState.BossMonster.STUN, new MonStunState(this));
            _stateMachine.ChangeState(EnumState.BossMonster.IDLE);
        }
        
        public override void AnimEventSpecialAttack()
        {
            BaseState specialAttackState = _stateMachine.GetState(EnumState.BossMonster.SKILL);
            if (_stateMachine.currentState == specialAttackState)
            {
                ((BossMonSpecialAtkState)specialAttackState).SpecialAttack();
            }
        }

        // return true : 공격 성공함, return false : 공격대상 없음
        public virtual bool SpecialAttack()
        {
            return false;
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }

        public class Factory : PlaceholderFactory<BossMonster>
        {

        }
    }

    #region Status

    public class BossMonsterStatus : MonsterStatus
    {
        public string name;
        
        public float skillCoolTime;
        public float skillRange;
        public float skillDamageMultiplier;
        
        public BossMonsterStatus(BossMonsterInfo info, MonsterGlobalSetting globalSetting) : base(info, globalSetting)
        {
            name = info.name;
            skillCoolTime = info.skillCoolTime;
            skillRange = info.skillRange;
            skillDamageMultiplier = info.skillDamageMultiplier;
        }
    }
    #endregion

    public static partial class EnumState
    {
        public static class BossMonster
        {
            public const string IDLE = "Idle";
            public const string MOVE = "Run";
            public const string ATK = "Attack";
            public const string DEAD = "Death";
            public const string STUN = "Stun";
            public const string SKILL = "Skill";
        }
    }
}
