using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    // 원딜 투사체, 직선 화살
    public class ArrowProjectile : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private IDamagable _target;
        
        private Vector3 _startPos; // 화살 쏜 위치
        private Vector3 _endPos; // 타겟 위치
        private Vector3 _direction;
        
        private readonly Vector3 _startPositonOffset = new Vector3(0.5f, 1f, 0);
        private readonly Vector3 _endPositonOffset = new Vector3(0, 0.8f, 0);
        
        [SerializeField]
        private float _speed = 10f;
        private int _damage;
        
        // 화살 도착 지점 허용 오차
        private readonly float EPS = 0.2f;

        private IMemoryPool _pool;
        private void Update()
        {
            // todo : 화살 너무 빨라서 뚫고 지나갈 수 있음 그에 관한 처리 필요
            if (Vector3.Distance(transform.position, _endPos) <= EPS)
            {
                _target?.TakeDamage(_damage);
                _pool.Despawn(this);
            }
            transform.position += _speed * Time.deltaTime * _direction;
        }

        public void FireProjectile(Unit owner, IDamagable target, int damage)
        {
            _target = target;
            
            _startPos = owner.position +_startPositonOffset;
            _endPos = target.gameObjectIDamagable.transform.position + _endPositonOffset;
            _direction = (_endPos - _startPos).normalized;
            transform.position = _startPos;
            
            _damage = damage;
    
            transform.rotation = Quaternion.FromToRotation(Vector3.up, _endPos - _startPos);
        }

        public void OnDespawned()
        {
            
        }

        public void OnSpawned(IMemoryPool p1)
        {
            _pool = p1;
        }
        
        public class Factory : PlaceholderFactory<ArrowProjectile>
        {
        }
    }
    
}