using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class InputHandler : ITickable
    {
        
        private InputState _inputState;

        [Inject]
        public void Constructor(InputState inputState)
        {
            _inputState = inputState;
        }

        public void Tick()
        {
            _inputState.IsClick = Input.GetMouseButtonDown(0) || Input.touchCount > 0; // 안드로이드 터치
            
            // 종료버튼
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    
    public class InputState
    {
        public bool IsClick
        {
            get;
            set;
        }

    }
}