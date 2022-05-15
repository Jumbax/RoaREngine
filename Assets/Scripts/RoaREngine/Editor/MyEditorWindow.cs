using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyEditorWindow : EditorWindow
{
    private string a = "Test";
    private string b = "Del Tasto";
    private string c = "Tastato";
    private string d = "Nell'intestino";
    private float e = 10.0f;
    private int f = 100;
    private float min = 1, max = 50;
    private GameObject go = null;

    [MenuItem("Tools/My Editor Window")]
    private static void ShowMyWindow()
    {
        MyEditorWindow w = GetWindow<MyEditorWindow>("My Ed Win");
        w.Show();
    }

    private void OnGUI()
    {
        //GUI.Box(new Rect(5, 20, 500, 120), "Test");
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label(a);
            EditorGUILayout.SelectableLabel(b);

            using (new GUILayout.VerticalScope())
            {
                c = GUILayout.TextField(c);
                d = GUILayout.TextArea(d);
            }
        }

        e = EditorGUILayout.FloatField("float", e);
        using (new GUILayout.HorizontalScope())
        { 
            f = EditorGUILayout.IntField("int", f, GUILayout.Width(50));
            e = Mathf.RoundToInt(GUILayout.HorizontalSlider(e, 0, 500));
        }

        using (new GUILayout.HorizontalScope()) 
        {
            min = EditorGUILayout.FloatField(min, GUILayout.Width(50));
            EditorGUILayout.MinMaxSlider(ref min, ref max, -50, 50);
            max = EditorGUILayout.FloatField(max, GUILayout.Width(50));
        }

        go = EditorGUILayout.ObjectField("Give me an object", go, typeof(GameObject), true) as GameObject;
    }
}
