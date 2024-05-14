using System;

namespace LineUpHeros
{
    public abstract class Monster : Unit
    {
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumState.Monster.IDLE, new MonIdleState(this));
            _stateMachine.AddState(EnumState.Monster.MOVE, new MonMoveState(this));
            _stateMachine.AddState(EnumState.Monster.ATK, new MonAtkState(this));
            _stateMachine.AddState(EnumState.Monster.DEAD, new MonDeadState(this));
        }

        #region Status
        protected class MonsterStatus : Status
        {
            public MonsterStatus(StatSettings settings) : base(settings)
            {
            }

        }
        
        // Scriptable Object Installer 세팅 값
        [Serializable]
        public class Settings
        {
            public int baseHp;
            public int baseAtk;
            public int baseAtkRange;
            public float baseAtkPerSec;
        }
        #endregion


        public override void TakeHeal(int healAmount)
        {
            // Monster는 힐을 받지 않는다! 나중엔 받을지도 모르지만
            // _status.tmpHp += healAmount;
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp -= damage;
        }
        public override void TakeStun(float stunTime)
        {
            
            
        }
        
    }
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