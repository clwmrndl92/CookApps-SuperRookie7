using System;

namespace LineUpHeros
{
    public abstract class Monster : Unit
    {
        
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumAnimState.Monster.IDLE, new MonIdleState(this));
            _stateMachine.AddState(EnumAnimState.Monster.MOVE, new MonMoveState(this));
            _stateMachine.AddState(EnumAnimState.Monster.ATK, new MonAtkState(this));
            _stateMachine.AddState(EnumAnimState.Monster.DEAD, new MonDeadState(this));
        }

        #region Status
        protected class MonsterStatus : UnitStatus
        {
            public MonsterStatus(UnitSettings settings) : base(settings)
            {
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
            public float baseAtkPerSec;
        }
        
    }
    public static partial class EnumAnimState
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