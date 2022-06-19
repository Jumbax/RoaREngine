using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Audio;

namespace RoaREngine
{
    public class RoaRWindowTest : EditorWindow
    {
        
        private void Awake()
        {
            
        }

        [MenuItem("RoaREngine/RoaRWindow")]
        private static void DisplayWindow()
        {
            RoaRWindow w = GetWindow<RoaRWindow>("RoaRWindow");
            w.Show();
        }

        private void OnGUI()
        {
            
        }

        private void PlaySound()
        {

        }
    }
}
