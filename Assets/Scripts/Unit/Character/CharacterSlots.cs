using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    public class CharacterSlots : MonoBehaviour
    {
        public List<Transform> slotList;
        private Dictionary<Transform, int> slotCharacter;
        
        TankerCharacter _tanker;
        ShortRangeDealerCharacter _shortRangeDealer;
        LongRangeDealerCharacter _longRangeDealer;
        HealerCharacter _healer;
        
        [Inject]
        public void Construct(TankerCharacter tanker, ShortRangeDealerCharacter shortRangeDealer,
            LongRangeDealerCharacter longRangeDealer, HealerCharacter healer) 
        {
            _tanker = tanker;
            _shortRangeDealer = shortRangeDealer;
            _longRangeDealer = longRangeDealer;
            _healer = healer;
            
            slotCharacter = new Dictionary<Transform, int>
            {
                { _tanker.gameObjectIDamagable.transform, 0 },
                { _shortRangeDealer.gameObjectIDamagable.transform, 1 },
                { _longRangeDealer.gameObjectIDamagable.transform, 2 },
                { _healer.gameObjectIDamagable.transform, 3 }
            };
        }

        private void Update()
        {
            SyncWithFirstSlot();
        }

        private void SyncWithFirstSlot()
        {
            Transform firstSlot = GetSlot(0);
            Transform unitTransform = GetSlotUnit(0);
            Vector3 diff = unitTransform.position - firstSlot.position;
            transform.position += diff;
        }

        public Transform GetSlot(int slotNum)
        {
            return slotList[slotNum];
        }

        public Transform GetSlotUnit(int slotNum)
        {
            foreach (var item in slotCharacter)
            {
                if (item.Value == slotNum) return item.Key;
            }
            return null;
        }
        
        public Transform GetSlot(Transform unitTransform)
        {
            return slotList[slotCharacter[unitTransform]];
        }
    }
}