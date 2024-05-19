using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LineUpHeros
{
    // 캐릭터가 배치되어있는 슬롯
    public class CharacterSlots : MonoBehaviour
    {
        // 슬롯 위치 리스트
        public List<Transform> slotList;
        // 슬롯 - 캐릭터 정보
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
            // 캐릭터 배치 정보
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
        
        // 0번 슬롯의 캐릭터(탱커) 위치로 전체 슬롯 위치 맞추기, 0번 캐릭터 기준 정렬
        private void SyncWithFirstSlot()
        {
            Transform unitTransform = GetSlotUnit(0);
            transform.position = transform.position.X(unitTransform.position.x);
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