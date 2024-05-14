using UnityEngine;

namespace LineUpHeros
{
    public partial class Unit
    {
        private string _currentState;
        private Animator _animator;

        protected void InitAnim()
        {
            _animator = GetComponent<Animator>();
        }

        public void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;

            _animator.Play(newState);

            _currentState = newState;
        }
    }

    public static partial class EnumAnimState
    {
    }
}