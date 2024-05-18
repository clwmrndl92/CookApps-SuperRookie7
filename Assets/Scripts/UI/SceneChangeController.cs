using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    // 페이드인 페이드아웃 전환 효과
    public class SceneChangeController : MonoBehaviour
    {
        public Image _sceneChangeImage;

        public float _fadeInTime = 2f;
        public float _fadeOutTime = 0f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // 현재 씬 리로드 메소드
        public void SceneReload()
        {
            StartCoroutine(SceneReloadCorutine());
        }
        
        IEnumerator SceneReloadCorutine()
        {
            _sceneChangeImage.gameObject.SetActive(true);
            float startTime = Time.time;
            Color color = _sceneChangeImage.color;
            
            // fade out
            while (Time.time - startTime < _fadeOutTime)
            {
                color.a = Mathf.Lerp(0, 1, (Time.time - startTime) / _fadeOutTime);
                _sceneChangeImage.color = color;
                yield return null;
            }

            color.a = 1;
            _sceneChangeImage.color = color;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            // fade in
            startTime = Time.time;
            while (Time.time - startTime < _fadeInTime)
            {
                color.a = Mathf.Lerp(1, 0, (Time.time - startTime) / _fadeInTime);
                _sceneChangeImage.color = color;
                yield return null;
            }
            
            _sceneChangeImage.gameObject.SetActive(false);
        }

    }
    
}
