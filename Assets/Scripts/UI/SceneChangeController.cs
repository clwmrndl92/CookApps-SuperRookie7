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
    public class SceneChangeController : MonoBehaviour
    {
        private GameObject _sceneChangeObject;
        private Image _sceneChangeImage;

        private float _fadeInTime = 2f;
        private float _fadeOutTime = 0f;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            _sceneChangeObject = transform.GetChild(0).gameObject;
            _sceneChangeImage = _sceneChangeObject.GetComponent<Image>();
        }
        // 현재 씬 리로드 메소드
        public void SceneReload()
        {
            StartCoroutine(SceneReloadCorutine());
        }
        
        IEnumerator SceneReloadCorutine()
        {
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
        }

    }
    
}
