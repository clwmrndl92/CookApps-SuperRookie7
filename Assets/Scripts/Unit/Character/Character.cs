using UnityEngine;
using System;
using System.Collections.Generic;
using LineUpHeros;
using UniRx;
using Zenject;

namespace LineUpHeros
{
    public abstract class Character : Unit, IDamagable
    {
        protected CharacterGlobalSetting _globalSettings;
        protected PlayerInfo _playerInfo;
        
        private CharacterSlots _characterSlots;
        
        private FloatingText.Factory _floatTextFactory;
        public CharacterStatus status => (CharacterStatus)_status;
        
        public static readonly Vector3 FLOATING_TEXT_OFFSET = new Vector3(0, 2f, 0);
            
        [Inject]
        private void Constructor(CharacterGlobalSetting globalSettings,FloatingText.Factory floatTextFactory, 
                                CharacterSlots characterSlot, PlayerInfo playerInfo)
        {
            _globalSettings = globalSettings;
            _floatTextFactory = floatTextFactory;
            _characterSlots = characterSlot;
            _playerInfo = playerInfo;
        }

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
            _stateMachine = new StateMachine(new FSMCharacterGlobalVariables());
            _stateMachine.AddState(EnumState.Character.IDLE, new CharIdleState(this));
            _stateMachine.AddState(EnumState.Character.MOVE, new CharMoveState(this));
            _stateMachine.AddState(EnumState.Character.ATK, new CharAtkState(this));
            _stateMachine.AddState(EnumState.Character.SPECIAL_ATK, new CharSpecialAtkState(this));
            _stateMachine.AddState(EnumState.Character.DEAD, new CharDeadState(this));
            _stateMachine.AddState(EnumState.Character.HURT, new CharHurtState(this));
            _stateMachine.AddState(EnumState.Character.VICTORY, new CharVictoryState(this));
            _stateMachine.AddState(EnumState.Character.GOTO_SLOT, new CharGotoSlotState(this));
            _stateMachine.ChangeState(EnumState.Character.IDLE);
        }

        private void LateUpdate()
        {
            ChangeOrderInLayer(_spriteModel.transform);
        }


        #region IDamagable
        public override void TakeHeal(int healAmount)
        {
            _status.tmpHp.Value += healAmount;

            var floatText = _floatTextFactory.Create();
            Vector3 textPos = position + FLOATING_TEXT_OFFSET;
            floatText.SetText(healAmount.ToString(), textPos, 0x00FF00);
        }

        public override void TakeDamage(int damage)
        {
            _status.tmpHp.Value -= damage;

            // var floatText = _floatTextFactory.Create();
            // Vector3 textPos = position + _floatingTextOffset;
            // floatText.SetText(damage.ToString(),textPos, 0xFF0000);

            if (_status.tmpHp.Value <= 0)
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
                ((CharAtkState)attackState).Attack();
            }
        }
        public override void AnimEventSpecialAttack()
        {
            BaseState specialAttackState = _stateMachine.GetState(EnumState.Character.SPECIAL_ATK);
            if (_stateMachine.currentState == specialAttackState)
            {
                ((CharSpecialAtkState)specialAttackState).SpecialAttack();
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
        public virtual bool SpecialAttack(List<IDamagable> atkRangeTargetList = null)
        {
            return false;
        }
        public virtual void Die()
        {
            isDead.Value = true;
        }
        public virtual void Revive()
        {
            status.tmpHp.Value = status.maxHp;
            isDead.Value = false;
        }
        #endregion

        #region Util
        // 범위내 살아있는 몬스터 리스트 찾아서 리턴 (거리순 정렬)
        public List<IDamagable> DetectMonsters(float radius)
        {
            List<IDamagable> aliveMonsterList = new List<IDamagable>();
            foreach (var monster in Util.GetDetectDamagableList(position, radius, LayerMasks.Monster))
            {
                if (monster.isDead.Value) continue;
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


        void ChangeOrderInLayer(Transform parent)
        {
            SpriteRenderer[] children = parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer renderer in children)
            {
                int y = (int)(position.y * 100);
                renderer.sortingOrder = 10000 - (y * 10) + renderer.sortingOrder % 10;
            }
        }

        public Transform GetSlot()
        {
            return _characterSlots.GetSlot(transform);
        }
        #endregion
    }

    #region Status
    public class CharacterStatus : Status
    {
        public ReactiveProperty<int> level;
        
        private CharacterGlobalSetting _globalSetting;
        public float detectRange => _globalSetting.detectRange;
        public float moveVelocity => _globalSetting.moveVelocity;
        public float revivalTime => _globalSetting.revivalTime;

        // Final Stat Property
        public int maxHp => (int)GetFinalStat(_baseHp, _addHp, _addPerHp, _addLevelHp);
        public int atk => (int)GetFinalStat(_baseAtk, _addAtk, _addPerAtk, _addLevelAtk);
        public int atkRange => (int)GetFinalStat(_baseAtkRange, _addAtkRange, _addPerAtkRange, _addLevelAtkRange);
        public float atkCool => (int)GetFinalStat(_baseAtkCool, _addAtkCool, _addPerAtkCool, _addLevelAtkCool);
        // 스킬 관련 스탯 추가
        public int skillRange => (int)GetFinalStat(_baseSkillRange, _addSkillRange, _addPerSkillRange, _addLevelSkillRange);
        public float skillCool => (int)GetFinalStat(_baseSkillCool, _addSkillCool, _addPerSkillCool, _addLevelSkillCool);

        // Base Stat, 영구 성장치 적용
        private int _baseSkillRange;
        private float _baseSkillCool;

        // Additional Stat, 아이템, 버프 등 일시적 성장치
        private int _addSkillRange;
        private float _addSkillCool;

        // Additional Percent Stat, 아이템, 버프 등 일시적 성장치 (퍼센트)
        private int _addPerSkillRange;
        private float _addPerSkillCool;

        // 레벨 관련 성장치
        private int _addLevelHp;
        private int _addLevelAtk;
        private int _addLevelAtkRange;
        private float _addLevelAtkCool;
        private int _addLevelSkillRange;
        private float _addLevelSkillCool;

        protected float GetFinalStat(float baseStat, float addStat, float addPerStat, float addLevelStat)
        {
            // Final Stat 계산식
            return baseStat + addStat + ((level.Value - 1) * addLevelStat) + ((baseStat + addStat) * (addPerStat / 100));
        }
        
        public CharacterStatus(CharacterSetting settings, CharacterGlobalSetting globalSetting, PlayerInfo playerInfo) : base(settings)
        {
            level = playerInfo.level;
            
            _baseSkillRange = settings.baseSkillRange;
            _baseSkillCool = settings.baseSkillCool;

            _globalSetting = globalSetting;
            
            /* TODO: installer에서 캐릭터별 성장치 설정 */
            _addLevelHp = _baseHp / 5;
            _addLevelAtk = _baseAtk / 5;
            _addLevelAtkRange = _baseAtkRange / 5;
            _addLevelAtkCool = _baseAtkCool / 5;
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
            public const string GOTO_SLOT = "return";
        }
    }

    // Character 스테이트머신 글로벌 변수
    public class FSMCharacterGlobalVariables : FSMGlobalVariables
    {
        public List<IDamagable> detectTargetList;
    }
}