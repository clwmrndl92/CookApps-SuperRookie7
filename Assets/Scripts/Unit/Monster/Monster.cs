﻿using System;
using System.Collections.Generic;
using LineUpHeros;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public abstract class Monster : Unit
    {
        public bool isStun { get; set; }
        public MonsterStatus status => (MonsterStatus)_status;
        [Inject]
        private FloatingText.Factory _floatTextFactory;
        private Vector3 _floatingTextOffset = new Vector3(0,1f,0);
        
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(new FsmMonsterGlobalVariables());
            _stateMachine.AddState(EnumState.Monster.IDLE, new MonIdleState(this));
            _stateMachine.AddState(EnumState.Monster.MOVE, new MonMoveState(this));
            _stateMachine.AddState(EnumState.Monster.ATK, new MonAtkState(this));
            _stateMachine.AddState(EnumState.Monster.DEAD, new MonDeadState(this));
            _stateMachine.AddState(EnumState.Monster.STUN, new MonStunState(this));
            _stateMachine.ChangeState(EnumState.Monster.IDLE);
        }

        #region IDamagable
        public override void TakeHeal(int healAmount)
        {
            // Monster는 힐을 받지 않는다! 나중엔 받을지도 모르지만
            // _status.tmpHp += healAmount;
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp.Value -= damage;
            
            var floatText = _floatTextFactory.Create();
            Vector3 textPos = position + _floatingTextOffset;
            floatText.SetText(damage.ToString(),textPos, 0xFF0000);
            
            if (_status.tmpHp.Value <= 0)
            {
                Die();
            }
            // Debug.Log(gameObject.name + " Take Damage " + damage + " HP : " + _status.tmpHp);
        }
        public override void TakeStun(float stunTime)
        {
            ((MonStunState)_stateMachine.GetState(EnumState.Monster.STUN)).stunTime = stunTime;
            _stateMachine.ChangeState(EnumState.Monster.STUN);
            
            var floatText = _floatTextFactory.Create();
            Vector3 textPos = position + _floatingTextOffset;
            floatText.SetText("Stun",textPos, 0xFFFF00);
        }
        
        #endregion

        #region publicMethod
        
        public override void AnimEventAttack()
        {
            BaseState attackState = _stateMachine.GetState(EnumState.Monster.ATK);
            if (_stateMachine.currentState == attackState)
            {
                ((MonAtkState) attackState).Attack();
            }
        }

        // return true : 공격 성공함, return false : 공격대상 없음
        public virtual bool Attack(List<IDamagable> atkRangeTargetList)
        {
            if (atkRangeTargetList.Count == 0) return false;
            
            atkRangeTargetList[0].TakeDamage(status.atk);
            return true;
        }
        
        public virtual void Die()
        {
            isDead.Value = true;
        }
        #endregion
        
        #region Util

        public List<IDamagable> DetectCharacters(float radius)
        {
            List<IDamagable> aliveCharacterList = new List<IDamagable>();
            foreach (var character in Util.GetDetectDamagableList(position, radius, LayerMasks.Character))
            {
                if (character.isDead.Value) continue;
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

        public void ChangeSpriteColor(Color color)
        {
            _spriteModel.GetComponent<SpriteRenderer>().color = color;
        }
        
        #endregion
    }
    
    #region Status

    public class MonsterStatus : Status
    {
        private MonsterGlobalSetting _globalSetting;
        public float detectRange => _globalSetting.detectRange;
        // todo: 나중에 글로벌스탯으로 만들든 몬스터스탯으로 만들든
        public float moveVelocity = 1f;
        public MonsterStatus(MonsterSetting setting, MonsterGlobalSetting globalSetting) : base(setting)
        {
            _globalSetting = globalSetting;
            setting.baseAtkCool = 1 / setting.baseAtkPerSec;
        }
    }
    #endregion


    #region Setting
    // Scriptable Object Installer 세팅 값
    [Serializable]
    public class MonsterSetting : StatSettings
    {
        public float baseAtkPerSec;
    }

    [Serializable]
    public class MonsterGlobalSetting
    {
        public float detectRange;
    }

    #endregion
    public static partial class EnumState
    {
        public static class Monster
        {
            public const string IDLE = "Idle";
            public const string MOVE = "Run";
            public const string ATK = "Attack";
            public const string DEAD = "Death";
            public const string STUN = "Stun";
        }
    }

    public class FsmMonsterGlobalVariables : FSMGlobalVariables
    {
        public List<IDamagable> detectTargetList;
        public const float EPSILON = 0.1f;
    }
}
