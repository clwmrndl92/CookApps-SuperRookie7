using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineUpHeros
{
    public class HealerEffect : MonoBehaviour
    {
        [SerializeField]
        float effectCreationTime = 0.2f; // 번개 날아가는 시간
        [SerializeField]
        float effectStopTime = 0.2f; // 번개 지팡이랑 적이랑 이어져있는 시간
        [SerializeField]
        float effectDisappearTime = 0.1f; // 번개 사라지는 시간
        
        // effect sprite 사이즈 관련 변수
        private float _length;
        private float _maxScale;
        
        private Vector3 _startPositon; // 힐러 위치
        private Vector3 _startPositonOffset = new Vector3(0.5f, 1f, 0);
        private Vector3 _endPositon; // 타겟 위치
        private Vector3 _endPositonOffset = new Vector3(0, 0.8f, 0);

        private Quaternion _rotation;
        
        public void SetEffect(Vector3 startPosition, Vector3 targetPosition)
        {
            // 스프라이트 사이즈 가져오기 (x 크기)
            transform.localScale = new Vector3(1,1.5f,1);
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
            
            _startPositon = startPosition + _startPositonOffset;
            _endPositon = targetPosition + _endPositonOffset;
            transform.position = _startPositon;
            
            _rotation = Quaternion.FromToRotation(Vector3.right, _endPositon - _startPositon);
            transform.rotation = _rotation;
            
            // 번개 이펙트 최대 크기 (비율, 적까지의 거리)
            _maxScale = Vector3.Distance(_startPositon, _endPositon) / _length;
        }

        public void StartEffect()
        {
            gameObject.SetActive(true);
            StartCoroutine(EffectCorutine());
        }

        IEnumerator EffectCorutine()
        {
            float startTime = Time.time;
            Vector3 diff = _endPositon - _startPositon;
            
            // 날아가는 시간, 적에 닿을때까지 길어짐
            while (Time.time - startTime < effectCreationTime)
            {
                Vector3 lerpPosition = _startPositon + Vector3.Lerp(Vector3.zero, diff / 2, (Time.time - startTime) / effectCreationTime);
                transform.position = lerpPosition;
                float scaleX = Mathf.Lerp(0, _maxScale, (Time.time - startTime) / effectCreationTime);
                transform.localScale = transform.localScale.X(scaleX);
                yield return null;
            }

            // 멈춰있는 시간
            transform.position = _startPositon + diff / 2;
            transform.localScale = transform.localScale.X(_maxScale);
            yield return new WaitForSeconds(effectStopTime);
            
            // 사라지는 시간, 적 쪽으로 짧아짐
            startTime = Time.time;
            while (Time.time - startTime < effectDisappearTime)
            {
                Vector3 lerpPosition = _endPositon - Vector3.Lerp(diff / 2, Vector3.zero, (Time.time - startTime) / effectDisappearTime);
                transform.position = lerpPosition;
                float scaleX = Mathf.Lerp(_maxScale, 0, (Time.time - startTime) / effectDisappearTime);
                transform.localScale = transform.localScale.X(scaleX);
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
