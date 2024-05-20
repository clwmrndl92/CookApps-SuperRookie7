using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
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

        private int speed = 1;

        private Image _image;
        private TextMeshProUGUI _text;

        private void Start()
        {
            _image = GetComponent<Image>();
            _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            GetComponent<Button>().OnClickAsObservable().Subscribe(_=>
            {
                speed = speed%3 + 1;
                _gameController.gameSpeed = speed;
                Time.timeScale = speed;
                _text.text = $"{speed}X\nSpeed";
                if (speed > 1) _image.color = Color.green; 
                else _image.color = Color.white;
            });
        }

    }
}
