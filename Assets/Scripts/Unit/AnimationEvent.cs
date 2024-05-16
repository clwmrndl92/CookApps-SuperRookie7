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

        public void AnimationEventAttack()
        {
            _unit.AnimEventAttack();
        }
        public void AnimationEventSpecialAttack()
        {
            _unit.AnimEventSpecialAttack();
        }
    }
}