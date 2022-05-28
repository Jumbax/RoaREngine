using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRWindowTest : EditorWindow
    {
        [MenuItem("RoaREngine/RoaRWindowTest")]
        private static void DisplayWindow()
        {
            RoaRWindowTest w = GetWindow<RoaRWindowTest>("RoaRWindowTest");
            w.Show();
        }
        private bool collapsed = false;
        private bool clearOnPlay = false;
        private void OnGUI()
        {
            //texture = AssetPreview.GetAssetPreview(clips[0]);
            //GUILayout.Label(texture);

            EditorGUILayout.BeginHorizontal("Toolbar", GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Clear", "ToolbarButton", GUILayout.Width(45f)))
            {
                Debug.Log("You click Clear button");
            }
            // Create space between Clear and Collapse button.
            GUILayout.Space(5f);
            // Create toggles button.
            collapsed = GUILayout.Toggle(collapsed, "Collapse", "ToolbarButton");
            clearOnPlay = GUILayout.Toggle(clearOnPlay, "Clear on Play", "ToolbarButton");
            // Push content to be what they should be. (ex. width)
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
