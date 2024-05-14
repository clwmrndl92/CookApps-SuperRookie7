using UnityEngine;

namespace LineUpHeros
{
    public static class LayerMasks
    {
        public static readonly int Character = LayerMask.GetMask("Character");
        public static readonly int Monster = LayerMask.GetMask("Monster");
    }
}