using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRWindowContainerList : EditorWindow
    {
        private string search;
        private List<string> words = new List<string>();

        [MenuItem("RoaREngine/RoaRWindowContainerList")]
        private static void DisplayWindow()
        {
            RoaRWindowContainerList w = GetWindow<RoaRWindowContainerList>("RoaRWindowContainerList");
            w.Show();
        }

        private void Awake()
        {
            words.Clear();
            //foreach (var item in AssetDatabase.(search))
            //{
            //    words.Add(item);
            //}
        }

        private void OnGUI()
        {
            search = EditorGUILayout.TextField("Search", search);
            for (int i = 0; i < words.Count; i++)
            {
                if (string.IsNullOrEmpty(search) || words[i].Contains(search))
                {
                    GUILayout.Button(words[i]);
                }
            }


            //searchText = GUILayout.TextField(searchText);
            //for (int i = 0; i < words.Count; i++)
            //{
            //    if (string.IsNullOrEmpty(searchText) || words[i].Contains(searchText))
            //    {
            //        GUILayout.Button(words[i]);
            //    }
            //}

        }
    }
}
