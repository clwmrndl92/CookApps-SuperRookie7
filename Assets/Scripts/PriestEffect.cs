using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    public class PriestEffect : MonoBehaviour
    {
        private float _length;
        private float _maxScale;
        private Vector3 _startPositon;
        private Vector3 _endPositon;
        // Start is called before the first frame update
        void Start()
        {
            _startPositon = transform.position;
            _endPositon = _startPositon.X(_startPositon.x + 4);
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
            _maxScale = Vector3.Distance(_startPositon, _endPositon) / _length;
            StartCoroutine(EffectCorutine());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        
        IEnumerator EffectCorutine()
        {
            
            float startTime = Time.time;
            Vector3 diff = _endPositon - _startPositon;
            float effectCreationTime = 0.2f;
            float effectDisappearTime = 0.3f;
            
            while (Time.time - startTime < effectCreationTime)
            {
                Vector3 lerpPosition = _startPositon + Vector3.Lerp(Vector3.zero, diff / 2, (Time.time - startTime) / effectCreationTime);
                transform.position = lerpPosition;
                float scaleX = Mathf.Lerp(0, _maxScale, (Time.time - startTime) / effectCreationTime);
                transform.localScale = transform.localScale.X(scaleX);
                yield return null;
            }

            
            startTime = Time.time;
            while (Time.time - startTime < effectDisappearTime)
            {
                Vector3 lerpPosition = _endPositon - Vector3.Lerp(diff / 2, Vector3.zero, (Time.time - startTime) / effectDisappearTime);
                transform.position = lerpPosition;
                float scaleX = Mathf.Lerp(_maxScale, 0, (Time.time - startTime) / effectDisappearTime);
                transform.localScale = transform.localScale.X(scaleX);
                yield return null;
            }
        }
    }
}
