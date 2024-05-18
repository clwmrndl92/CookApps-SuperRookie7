using UnityEngine;
using UnityEditor;

namespace LineUpHeros
{
    [CustomEditor(typeof(Character), true)]
    public class CubeGenerateButton : Editor
    {
        public override void OnInspectorGUI()
        {
            //   CubeGenerator generator = (CubeGenerator)target; 
            Character character = (Character)target;
            if (GUILayout.Button("Gain 1 Exp"))
            {
                character.status.GainExp(1);
            }
        }
    }
}

