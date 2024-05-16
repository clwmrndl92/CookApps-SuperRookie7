using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace LineUpHeros
{
    public partial class Unit
    {
        private GameObject _spriteModel;
        private Animator _animator;

        protected void InitAnim()
        {
            _spriteModel = GameObject.Find("SpriteModel").gameObject;
            _animator = _spriteModel.GetComponent<Animator>();
        }

        public void ChangeAnimationState(string newState)
        {
            _animator.Play(newState);
        }
        
        // Attack Animation에서 호출하는 함수
        protected virtual void AnimEventAttack()
        {
            // TakeDamage 실행, 이펙트, 소리 등
        }
        
    }

    public static partial class EnumState
    {
    }
    
}