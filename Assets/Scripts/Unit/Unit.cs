using UnityEngine;
using System;
using UniRx;

namespace LineUpHeros
{
    public abstract partial class Unit : MonoBehaviour, IDamagable
    {
        protected StateMachine _stateMachine;
        protected Status _status;
        
        public StateMachine stateMachine { get => _stateMachine; set=> _stateMachine = value; }
        public ReactiveProperty<bool> isDead { get; set; } = new ReactiveProperty<bool>();

        #region Componets
        public Vector3 position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Vector3 scale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }
        #endregion

        #region MonoBehaviour Method

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            stateMachine.UpdateState();
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdateState();
        }

        #endregion
        
        #region Initialize
        private void Init()
        {
            // 지켜야할 순서 Anim -> StateMachine
            InitAnim();
            InitStateMachine();
            InitStatus();
        }
        // InitStateMachine()에서 _stateMachine 새 StateMachine 인스턴스 할당 필요
        protected abstract void InitStateMachine();
        // InitStatus()에서 _status에 새 Status 인스턴스 할당 필요
        protected abstract void InitStatus();
        #endregion
        
        #region IDamagable
        public Status status { get => _status; set=> _status = value; }
        public GameObject gameObjectIDamagable { get => gameObject; }

        public abstract void TakeHeal(int healAmount);
        public abstract void TakeDamage(int damage);
        public abstract void TakeStun(float stunTime);

        #endregion
    }
    
}