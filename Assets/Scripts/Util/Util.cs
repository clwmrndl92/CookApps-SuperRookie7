using System.Collections.Generic;
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
            return  new Vector3(vec.x, vec.y, z);
        }
    }
}