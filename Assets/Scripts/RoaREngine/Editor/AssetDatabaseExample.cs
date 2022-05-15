using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class AssetDatabaseExample
{
    [MenuItem("Assets/Create/My Material")]
    private static void DoCreateMyMaterial ()
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = Color.cyan;
        string path;
        /* Explicit path
         * 
         */
        /* Ask asset path
        AssetDatabase.CreateAsset(mat,"Assets/");
         * 
        string path = EditorUtility.SaveFilePanel("Save My Material", "", "", "mat");
        int assetIndex = path.IndexOf("Assets/");
        if (assetIndex < 0)
        {
            Debug.Log("Chosen path is not beneath assets");
            return;
        }
        if (assetIndex > 0)
        {
            path = path.Substring(assetIndex);
        }
          */
        /* Implicit path
        */
        path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Path.Combine(path, "New My Material.mat");
        /**/
        AssetDatabase.CreateAsset(mat, path);
        Material mat2 = new Material(Shader.Find("Standard"));
        mat2.color = Color.magenta;
        AssetDatabase.AddObjectToAsset(mat2, mat);
        //AssetDatabase.SaveAssets();
        AssetDatabase.SaveAssetIfDirty(mat);
    }
}
