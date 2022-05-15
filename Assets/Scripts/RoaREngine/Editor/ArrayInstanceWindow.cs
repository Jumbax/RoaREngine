using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ArrayInstanceWindow : EditorWindow
{
    private enum InstancingMode : byte
    {
        GRID,
        RADIAL
    }
    private GameObject prefab = null;
    private InstancingMode mode = InstancingMode.GRID; 
    private int instancesCount = 5;
    private float extent = 10.0f;
    private const int MIN_INSTANCES_COUNT = 1;
    private const int MAX_INSTANCES_COUNT = 20;
    private const float MIN_EXTENT = 1.0f;
    private const float MAX_EXTENT = 100.0f;
    [MenuItem("Tools/Array Instancer")]
    private static void DisplayWindow()
    {
        ArrayInstanceWindow w = GetWindow<ArrayInstanceWindow>("ArrayInstanceWindow");
        w.Show();
    }

    private void OnGUI()
    {
        prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
        mode = (InstancingMode)EditorGUILayout.EnumPopup("Mode", mode);
        using (new GUILayout.HorizontalScope()) 
        {
            GUILayout.Label("Instances count", GUILayout.Width(150));
            instancesCount = Mathf.RoundToInt(GUILayout.HorizontalSlider(instancesCount, MIN_INSTANCES_COUNT, MAX_INSTANCES_COUNT));
            instancesCount = EditorGUILayout.IntField(instancesCount, GUILayout.Width(25));
            instancesCount = Mathf.Clamp(instancesCount, MIN_INSTANCES_COUNT, MAX_INSTANCES_COUNT);
        }
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label(mode == InstancingMode.GRID ? "Grid" : "Extent", GUILayout.Width(150));
            extent = GUILayout.HorizontalSlider(extent, MIN_EXTENT, MAX_EXTENT);
            extent = EditorGUILayout.FloatField(extent, GUILayout.Width(25));
            extent = Mathf.Clamp(extent, MIN_EXTENT, MAX_EXTENT);
        }

        bool wasGUIEnabled = GUI.enabled;
        GUI.enabled &= prefab != null;
        if (GUILayout.Button("Instantiate"))
            InstantiateAll();
        GUI.enabled = wasGUIEnabled;
    }

    private void InstantiateAll()
    {
        switch (mode)
        {
            case InstancingMode.GRID:
                InstantiateGrid();
                break;
            case InstancingMode.RADIAL:
                InstantiateRadial();
                break;
            default:
                break;
        }
    }

    private void InstantiateGrid()
    {
        GameObject instance = null;
        float step = extent / instancesCount;
        for (int i = 0; i < instancesCount; i++)
        {
            for (int j = 0; j < instancesCount; j++)
            {
                //instance = Instantiate(prefab);
                instance = PrefabUtility.InstantiatePrefab(instance) as GameObject;
                Undo.RegisterCreatedObjectUndo(instance,"array instance");
                instance.transform.position = new Vector3(i*step, 0.0f,j * step);
            }
        }
    }
    private void InstantiateRadial()
    {
        //TODO
    }
}
