using UniRx;
using UnityEngine;

namespace LineUpHeros
{
    public class CharacterHealthBar : MonoBehaviour
    {
        private Character _character;
        void Start()
        {
            _character = transform.parent.GetComponent<Character>();
            // 체력바 사이즈 조절
            float maxScale = transform.localScale.x;
            _character.status.tmpHp
                .Subscribe(value =>
                {
                    transform.localScale = transform.localScale.X(maxScale * Mathf.Clamp(((float)value / _character.status.maxHp), 0, 1));
                });
        }
    }
}
