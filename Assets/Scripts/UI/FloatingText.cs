using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace LineUpHeros
{
    public class FloatingText : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private float _moveSpeed;
        private float _alphaSpeed;
        private float _destroyTime;
        private string _string;
        private TextMeshProUGUI _text;
        private Color _color;

        private float _startTime;

        private IMemoryPool _pool;

        void Update()
        {
            transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        
            _color.a = Mathf.Lerp(_color.a, 0, Time.deltaTime * _alphaSpeed);
            _text.color = _color;
            if (Time.time - _startTime > _destroyTime)
            {
                _pool.Despawn(this);
            }
        }

        public void SetText(string text, Vector3 position, int rgb = 0xFF0000, float moveSpeed = 100.0f, float alphaSpeed = 5.0f, float destroyTime = 1.0f)
        {
            _moveSpeed = moveSpeed;
            _alphaSpeed = alphaSpeed;
            _destroyTime = destroyTime;
            
            _text.text = text;
            _text.color = new Color(rgb >> 16,(rgb & 0x00FF00) >> 4,rgb & 0x0000FF);
            _color = _text.color;
            
            transform.position = Camera.main.WorldToScreenPoint(position);
        }

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool p1)
        {
            _text = GetComponent<TextMeshProUGUI>();
            // SetText("damage!");
            _startTime = Time.time;
            
            _pool = p1;
        }
        public class Factory : PlaceholderFactory<FloatingText>
        {
                
        }
    }
}

