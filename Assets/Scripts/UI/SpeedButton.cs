using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LineUpHeros
{
    public class SpeedButton : MonoBehaviour
    {
        [Inject]
        GameController _gameController;

        private bool _is2XSpeed = false;

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
            Debug.Log("??");
        }

        public void OnButtonClick()
        {
            if (_is2XSpeed)
            {
                _is2XSpeed = false;
                _image.color = Color.white;
                _gameController.gameSpeed = 1;
                Time.timeScale = 1;
            }
            else
            {
                _is2XSpeed = true;
                _image.color = Color.green;
                _gameController.gameSpeed = 2;
                Time.timeScale = 2;
            }
        }
    }
}
