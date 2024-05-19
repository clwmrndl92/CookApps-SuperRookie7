using UnityEngine;

namespace LineUpHeros
{
    public class Parallax : MonoBehaviour
    {
        public float parallaxEffect;

        private float _length;
        private Vector3 _startPos;
        private float _startPosX;
        private Vector3 _leftPos;
        private Vector3 _rightPos;
        private GameObject _cam;

        float speed = 0.2f;
        Vector3 direction;

        void Start()
        {
            _startPos = transform.position;
            _startPosX = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
            _cam = Camera.main.gameObject;
        }

        void FixedUpdate()
        {
            float temp = (_cam.transform.position.x * (1 - parallaxEffect));
            float dist = (_cam.transform.position.x * parallaxEffect);
            transform.position = new Vector3(_startPosX + dist, transform.position.y, transform.position.z);
            if (temp > _startPosX + _length) _startPosX += _length;
            else if (temp < _startPosX - _length) _startPosX -= _length;
        }
    }
    
}