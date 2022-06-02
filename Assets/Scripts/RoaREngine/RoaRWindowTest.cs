using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

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

        //public float scrollPos = 0.5F;
        // This will use the following style names to determine the size / placement of the buttons
        // MyScrollbarleftbutton    - Name of style used for the left button.
        // MyScrollbarrightbutton - Name of style used for the right button.
        // MyScrollbarthumb         - Name of style used for the draggable thumb.
        Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            //scrollPos = GUILayout.HorizontalScrollbar(scrollPos, 1, 0, 100);
            scrollPos = GUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(100), GUILayout.Height(100));
            //GUILayout.BeginArea(new Rect(0, 0, 300, 300));    //Does not display correctly if this is not commented out!

            GUILayout.Button("I am a button", GUILayout.MinWidth(150), GUILayout.MinHeight(150));

            //GUILayout.EndArea();
            GUILayout.EndScrollView();
        }

    }
}
