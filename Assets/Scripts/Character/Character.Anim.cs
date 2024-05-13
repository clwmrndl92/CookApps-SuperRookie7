using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LineUpHeros
{
    public partial class Character
    {
        protected Animator animator { get; private set; }
        
        private string _currentState;
        
        protected void InitAnim()
        {
            animator = GetComponent<Animator>();
            ChangeAnimationState(EnumAnimState.MOVE);
        }
        
        public void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;
        
            animator.Play(newState);
        
            _currentState = newState;
        }

    }
    
    public static class EnumAnimState
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
