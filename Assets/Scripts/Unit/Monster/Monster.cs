using System;
using System.Collections.Generic;
using LineUpHeros;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public abstract class Monster : Unit, IPoolable<IMemoryPool>
    {
        public MonsterStatus status => (MonsterStatus)_status;
        [Inject]
        private FloatingText.Factory _floatTextFactory;
        private Vector3 _floatingTextOffset = new Vector3(0, 1f, 0);

        private IMemoryPool _pool;

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

        private void LateUpdate()
        {
            ChangeOrderInLayer();
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
            // 특정 데미지 이상 색 변경
            // todo : 나중에 크리티컬 추가? 하드코딩 좀 어케 수정
            int color = damage >= 50 ? 0x00FFFF : 0xFFFFFF;
            floatText.SetText(damage.ToString(), textPos, color);

            if (_status.tmpHp.Value <= 0 && isDead.Value == false)
            {
                Die();
            }
        }
        public override void TakeStun(float stunTime)
        {
            ((MonStunState)_stateMachine.GetState(EnumState.Monster.STUN)).stunTime = stunTime;
            _stateMachine.ChangeState(EnumState.Monster.STUN);

            var floatText = _floatTextFactory.Create();
            Vector3 textPos = position + _floatingTextOffset;
            floatText.SetText("Stun", textPos, 0xFFFF00);
        }

        #endregion

        #region publicMethod

        public override void AnimEventAttack()
        {
            BaseState attackState = _stateMachine.GetState(EnumState.Monster.ATK);
            if (_stateMachine.currentState == attackState)
            {
                ((MonAtkState)attackState).Attack();
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
        public void DespawnMonster()
        {
            _pool.Despawn(this);
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
        
        // 스턴 이펙트 용
        public void ChangeSpriteColor(Color color)
        {
            _spriteModel.GetComponent<SpriteRenderer>().color = color;
        }

        void ChangeOrderInLayer()
        {
            SpriteRenderer renderer = _spriteModel.GetComponent<SpriteRenderer>();
            int y = (int)(position.y * 100);
            renderer.sortingOrder = 10000 - (y * 10) + renderer.sortingOrder % 10;
        }
        #endregion

        #region Factory

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool p1)
        {
            isDead.Value = false;
            Init();
            // todo : 포지션 정해주기, hp 초기화, state초기화, status초기화
            _pool = p1;
        }

        public class Factory : PlaceholderFactory<Monster>
        {

        }
        #endregion
    }

    #region Status

    public class MonsterStatus : Status
    {
        public float detectRange;
        public float moveVelocity;
        public MonsterStatus(MonsterInfo info, MonsterGlobalSetting globalSetting) : base(info.statusSetting)
        {
            detectRange = globalSetting.detectRange;
            moveVelocity = info.statusSetting.moveVelocity;
        }
    }
    #endregion


    #region Setting
    // Scriptable Object Installer 세팅 값

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

public enum EnumMonsterType
{
    Goblin,
    FlyingEye,
    Mushroom,
    Skeleton,
    KingGoblin,
    KingFlyingEye,
    KingMushroom,
    KingSkeleton,
}