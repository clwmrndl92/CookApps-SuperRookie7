using UnityEngine;
using UnityEditor;
using Zenject;

namespace LineUpHeros
{
    [CustomEditor(typeof(DebugButtons), true)]
    public class StageClearButton : Editor
    {
        public override void OnInspectorGUI()
        {
            DebugButtons debug = (DebugButtons)target;
            
            if (GUILayout.Button("Next Stage"))
            {
                debug.NextStage();
            }
            
            if (GUILayout.Button("Boss"))
            {
                debug.Boss();
            }
        }
    }
}

