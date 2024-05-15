using UnityEngine;

namespace LineUpHeros
{
    public partial class Unit
    {
        private Animator _animator;

        protected void InitAnim()
        {
            _animator = GetComponent<Animator>();
        }

        public void ChangeAnimationState(string newState)
        {
            _animator.Play(newState);
        }
    }

    public static partial class EnumState
    {
    }
}