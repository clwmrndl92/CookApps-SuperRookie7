using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    // 일반 공격 스테이트
    public class MonAtkState : MonsterState
    {
        private float _timer;
        private bool canAttack => !_isCool && !_isAttacking;
        private bool _isCool;
        private bool _isAttacking;
        private List<IDamagable> _attackTargetList;
        public MonAtkState(Monster monster) : base(monster)
        {
            _monster = monster;
        }

        public override void OnEnterState()
        {
            Debug.Log("monster atk state");
            _timer = 0;
            _isCool = false;
            _isAttacking = false;
            _attackTargetList = new List<IDamagable>();
        }

        public override void OnUpdateState()
        {
            if(_isAttacking == false && CheckChangeState()) return;
            if (_timer <= 0)
            {
                _isCool = false;
            }
            if (canAttack && _attackTargetList.Count != 0)
            {
                // todo: flip 나중에 unit으로 옮기기
                if (_attackTargetList[0].gameObjectIDamagable.transform.position.x < _monster.position.x)
                {
                    _monster.scale = _monster.scale.X((Mathf.Abs(_monster.scale.x) * -1));
                }
                else if(_attackTargetList[0].gameObjectIDamagable.transform.position.x > _monster.position.x)
                {
                    _monster.scale = _monster.scale.X(Mathf.Abs(_monster.scale.x));
                }
                _monster.ChangeAnimationState(EnumState.Monster.ATK);
                _timer = _monster.status.atkCool;
                _isAttacking = true;
            }
            _timer -= Time.deltaTime;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnExitState()
        {
        }

        public override bool CheckChangeState()
        {
            // attack 범위내에 캐릭터 있는지 체크, 없으면 Idle state로 전환
            List<IDamagable> attackList = _monster.DetectCharacters(_monster.status.atkRange);
            if (attackList.Count == 0)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.IDLE);
                return true;
            }
            if (Mathf.Abs(_monster.position.y - attackList[0].gameObjectIDamagable.transform.position.y) > 0.1f)
            {
                _monster.stateMachine.ChangeState(EnumState.Monster.MOVE);
                return true;
            }
            _attackTargetList = attackList;
            return false;
        }
        
        
        public void Attack()
        {
            _monster.Attack(_attackTargetList);
            _isAttacking = false;
        }
    }
}