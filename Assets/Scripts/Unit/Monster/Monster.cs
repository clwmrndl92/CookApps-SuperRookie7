using System;
using System.Collections.Generic;
using LineUpHeros;
using UnityEngine;

namespace LineUpHeros
{
    public abstract class Monster : Unit
    {
        public MonsterStatus status => (MonsterStatus)_status;
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumState.Monster.IDLE, new MonIdleState(this));
            _stateMachine.AddState(EnumState.Monster.MOVE, new MonMoveState(this));
            _stateMachine.AddState(EnumState.Monster.ATK, new MonAtkState(this));
            _stateMachine.AddState(EnumState.Monster.DEAD, new MonDeadState(this));
            _stateMachine.ChangeState(EnumState.Monster.IDLE);
        }


        #region Util

        public List<IDamagable> DetectCharacters(float radius)
        { 
            return Util.GetDetectDamagableList(position, radius, LayerMasks.Character);
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

        #region IDamagable
        public override void TakeHeal(int healAmount)
        {
            // Monster는 힐을 받지 않는다! 나중엔 받을지도 모르지만
            // _status.tmpHp += healAmount;
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp -= damage;
            Debug.Log(gameObject.name + " Take Damage " + damage + " HP : " + _status.tmpHp);
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
        }
}
}
