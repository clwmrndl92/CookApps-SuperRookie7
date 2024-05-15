using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineUpHeros
{
    public static class Util
    {
        // Vector3의 X,Y,Z 값을 바꿔 복제한 Vector3를 리턴하는 함수
        public static Vector3 X(this Vector3 vec, float x)
        {
            return new Vector3(x, vec.y, vec.z);
        }

        public static Vector3 Y(this Vector3 vec, float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }

        public static Vector3 Z(this Vector3 vec, float z)
        {
            return new Vector3(vec.x, vec.y, z);
        }
        
        // 밑에있는 GetDetectList 함수를 사용하려 했으나 인터페이스에 사용하기가 복잡스러워서 그냥 만듬 (Component[]로 리턴해서 변환해줘야함)
        public static List<IDamagable> GetDetectDamagableList(Vector3 centerPosition, float radius, LayerMask targetLayerMask)
        {
            Collider2D[] detectColliderList = new Collider2D[100];
            Physics2D.OverlapCircleNonAlloc(centerPosition, radius, detectColliderList, targetLayerMask);
            List<IDamagable> targetList = new List<IDamagable>();
            foreach(var detect in detectColliderList){
                if(detect == null) break;
                Component[] target = detect.GetComponents(typeof(IDamagable));
                if (target.Length != 0)
                {
                    targetList.Add((IDamagable)(target[0]));
                }
            }
            // 거리순 정렬
            targetList = targetList.OrderBy(item => Vector3.Distance(centerPosition, item.gameObjectIDamagable.transform.position)).ToList();

            return targetList;
        }
        // gameobject.ㅎetComponent로 불러올수 있는 컴포넌트만 됨, 인터페이스 안됨
        public static List<T> GetDetectList<T>(Vector3 centerPosition, float radius, LayerMask targetLayerMask)
        {
            Collider2D[] detectColliderList = new Collider2D[100];
            Physics2D.OverlapCircleNonAlloc(centerPosition, radius, detectColliderList, targetLayerMask);
            List<T> targetList = new List<T>();
            foreach(var detect in detectColliderList){
                if(detect == null) break;
                T target = detect.gameObject.GetComponent<T>();
                if (target != null)
                {
                    targetList.Add(target);
                }
            }

            return targetList;
        }
        
    }
}