using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 타겟 트랜스폼을 따라다니는 카메라
    public class FollowCamera : MonoBehaviour
    {
        public Transform followCharacter;
        public Vector2 offset;
        
        void Start()
        {
        
        }
        void LateUpdate()
        {
            transform.position = followCharacter.position.Z(transform.position.z) + (Vector3)offset;
        }
    }
}
