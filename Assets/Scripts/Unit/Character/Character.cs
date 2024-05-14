using UnityEngine;
using System;

namespace LineUpHeros
{
    public abstract class Character : Unit
    {
        protected override void InitStateMachine()
        {
            _stateMachine = new StateMachine(EnumAnimState.Character.IDLE, new CharIdleState(this));
            _stateMachine.AddState(EnumAnimState.Character.MOVE, new CharMoveState(this));
            _stateMachine.AddState(EnumAnimState.Character.ATK, new CharAtkState(this));
            _stateMachine.AddState(EnumAnimState.Character.SPECIAL_ATK, new CharSpecialAtkState(this));
            _stateMachine.AddState(EnumAnimState.Character.DEAD, new CharDeadState(this));
            _stateMachine.AddState(EnumAnimState.Character.HURT, new CharHurtState(this));
            _stateMachine.AddState(EnumAnimState.Character.VICTORY, new CharVictoryState(this));
        }

        #region Status
        protected class CahracterStatus : UnitStatus
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

            public CahracterStatus(Settings settings) : base(settings)
            {
                _baseSkillRange = settings.baseSkillRange;
                _baseSkillCool = settings.baseSkillCool;
            }

        }
        #endregion
        
        // Scriptable Object Installer 세팅 값
        [Serializable]
        public class Settings : UnitSettings
        {
            // 스킬 관련 변수 추가
            public int baseSkillRange;
            public float baseSkillCool;
        }
        
    }
    public static partial class EnumAnimState
    {
        public static class Character
        {
            public const string IDLE = "idle";
            public const string MOVE = "walk";
            public const string ATK = "attak";
            public const string SPECIAL_ATK = "casting";
            public const string DEAD = "die";
            public const string HURT = "hurt";
            public const string VICTORY = "victory";
        }
    }
}