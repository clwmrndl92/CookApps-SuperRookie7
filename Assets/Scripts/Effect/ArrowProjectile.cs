using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class ArrowProjectile : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private IDamagable _target;
        
        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 _direction;
        private Vector3 _startPositonOffset = new Vector3(0.5f, 1f, 0);
        private Vector3 _endPositonOffset = new Vector3(0, 0.8f, 0);
        [SerializeField]
        private float _speed = 10f;
        private int _damage;
        
        private readonly float EPS = 0.2f;
        
        private Unit _owner;

        private IMemoryPool _pool;
        private void Update()
        {
            if (Vector3.Distance(transform.position, _endPos) <= EPS)
            {
                _target?.TakeDamage(_damage);
                _pool.Despawn(this);
            }
            transform.position += _speed * Time.deltaTime * _direction;
        }

        public void FireProjectile(Unit owner, IDamagable target, int damage)
        {
            _owner = owner;
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