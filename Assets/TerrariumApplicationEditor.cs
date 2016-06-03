using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets
{
    [CustomEditor(typeof(TerrariumApplication))]
    public class TerrariumApplicationEditor : Editor
    {
        void OnInspectorGUI()
        {
            TerrariumApplication script = (TerrariumApplication)target;
            DrawDefaultInspector();
        }
    }
}
