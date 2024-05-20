using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace LineUpHeros
{
    public class CoolTimeImage : MonoBehaviour
    { 
         public TextMeshProUGUI text_CoolTime;
         public Image image_fill;
         
         public float cooltime = 10;
         private float currentTime => Time.time - _startTime;
         private float _startTime;
         
         private bool _isEnded = true;
     
         void Start()
         {
             Init();
             gameObject.SetActive(false);
         }
     
         void Update()
         {
             CheckCoolTime();
         }

         private void OnEnable()
         {
             ResetCoolTime();
         }

         private void Init()
         {
             image_fill.type = Image.Type.Filled;
             image_fill.fillMethod = Image.FillMethod.Radial360;
             image_fill.fillOrigin = (int)Image.Origin360.Top;
             image_fill.fillClockwise = false;
         }
        
     
         private void CheckCoolTime()
         {
             if (currentTime < cooltime)
             {
                 SetFillAmount(cooltime - currentTime);
             }
             else if (!_isEnded)
             {
                 EndCoolTime();
                 gameObject.SetActive(false);
             }
         }
     
         private void EndCoolTime()
         {
             SetFillAmount(0);
             _isEnded = true;
             text_CoolTime.gameObject.SetActive(false);
         }
     
         private void ResetCoolTime()
         {
             text_CoolTime.gameObject.SetActive(true);
             _startTime = Time.time;
             SetFillAmount(cooltime);
             _isEnded = false;
         }
         private void SetFillAmount(float value)
         {
             image_fill.fillAmount = value/cooltime;
             text_CoolTime.text = value.ToString("0.0");
         }
        
    }
}
