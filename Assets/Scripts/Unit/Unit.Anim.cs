using UnityEngine;

namespace LineUpHeros
{
    public partial class Unit
    {
        private GameObject _spriteModel;
        private Animator _animator;

        protected void InitAnim()
        {
            _spriteModel = transform.Find("SpriteModel").gameObject;
            _animator = _spriteModel.GetComponent<Animator>();
        }

        public void ChangeAnimationState(string newState)
        {
            // 이미 실행중인 스테이트면 리턴
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(newState)) return;
            _animator.Play(newState);
        }
        
        // Attack Animation에서 호출하는 함수
        public virtual void AnimEventAttack()
        {
            // TakeDamage 실행, 이펙트, 소리 등
        }
        
        #region util
        // todo : 안쓸것 같으면 삭제
        protected float GetClipLength(string stateName)
        {
            RuntimeAnimatorController controller = _animator.runtimeAnimatorController;

            if (controller != null)
            {
                for(int i = 0; i<controller.animationClips.Length; i++)
                {
                    if(controller.animationClips[i].name == stateName)
                    {
                        return controller.animationClips[i].length;
                    }
                }
            }
            return 0;
        }
        // direction : 1, 오른쪽 / direction : -1, 왼쪽 / default 현재 방향의 반대방향으로 플립
        public void Flip(int direction = 0)
        {
            scale = direction switch
            {
                1 => scale.X(Mathf.Abs(scale.x)),
                -1 => scale.X(Mathf.Abs(scale.x) * -1),
                _ => scale.X(scale.x * -1)
            };
        }
        public void FlipToTarget(GameObject target)
        {
            Vector3 targetPosition = target.transform.position;
            if (targetPosition.x < position.x)
            {
                Flip(-1);
            }
            else if(targetPosition.x > position.x)
            {
                Flip(1);
            }
        }
        #endregion
    }

    public static partial class EnumState
    {
    }
    
}