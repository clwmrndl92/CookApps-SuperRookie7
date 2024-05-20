using System;
using UnityEngine;

namespace LineUpHeros
{
    public class AnimationEvent : MonoBehaviour
    {
        private Unit _unit;
        private void Awake()
        {
            _unit = transform.parent.gameObject.GetComponent<Unit>();
        }
        // Attack 애니메이션에서 호출
        public void AnimationEventAttack()
        {
            _unit.AnimEventAttack();
        }
        // SpecialAttack 애니메이션에서 호출
        public void AnimationEventSpecialAttack()
        {
            _unit.AnimEventSpecialAttack();
        }
    }
}