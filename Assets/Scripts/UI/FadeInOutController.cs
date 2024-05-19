using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    // 페이드인 페이드아웃 전환 효과
    public class FadeInOutController : MonoBehaviour
    {
        [SerializeField]
        private Image _fadeInOutImage;

        [SerializeField]
        private float _fadeOutTime = 1f;
        [SerializeField]
        private float _stopTime = 1f;
        [SerializeField]
        private float _fadeInTime = 2f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        public void StartEffect(Action action)
        {
            StartCoroutine(FadeInOut(action));
        }
        
        public void StartEffect(Action action, float fadeOutTime, float stopTime, float fadeInTime )
        {
            _fadeOutTime= fadeOutTime; 
            _stopTime= stopTime;
            _fadeInTime= fadeInTime;
            StartCoroutine(FadeInOut(action));
        }
        // public void SceneReload()
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // }

        IEnumerator FadeInOut(Action action)
        {
            _fadeInOutImage.gameObject.SetActive(true);
            float startTime = Time.time;
            Color color = _fadeInOutImage.color;
            
            // fade out
            while (Time.time - startTime < _fadeOutTime)
            {
                color.a = Mathf.Lerp(0, 1, (Time.time - startTime) / _fadeOutTime);
                _fadeInOutImage.color = color;
                yield return null;
            }
            
            // stop time
            color.a = 1;
            _fadeInOutImage.color = color;
            yield return new WaitForSeconds(_stopTime);
            action();
            
            // fade in
            startTime = Time.time;
            while (Time.time - startTime < _fadeInTime)
            {
                color.a = Mathf.Lerp(1, 0, (Time.time - startTime) / _fadeInTime);
                _fadeInOutImage.color = color;
                yield return null;
            }
            
            _fadeInOutImage.gameObject.SetActive(false);
        }
        

    }
    
}
