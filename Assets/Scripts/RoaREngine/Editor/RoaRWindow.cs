using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRWindow : MyEditorWindow
    {
        private string containerName;
        private List<AudioClip> clips = new List<AudioClip>(3);
        private int count = 3;

        [MenuItem("RoaREngine/RoaRWindow")]
        private static void DisplayWindow()
        {
            RoaRWindow w = GetWindow<RoaRWindow>("RoaRWindow");
            w.Show();
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                //GUILayout.Label("ContainerName", GUILayout.Width(150));
                containerName = EditorGUILayout.TextField("ContainerName", containerName);
                //clip = EditorGUILayout.ObjectField("AudioClips", clip, typeof(AudioClip), false) as AudioClip;
                for (int i = 0; i < clips.Count; i++)
                {
                    //clips.Add(EditorGUILayout.ObjectField("AudioClip", clips[i], typeof(AudioClip), false) as AudioClip);
                    clips[i] = EditorGUILayout.ObjectField("AudioClip", clips[i], typeof(AudioClip), false) as AudioClip;
            }
                if (GUILayout.Button("Add Clip"))
                {
                    AddClipField();
                }
                if (GUILayout.Button("Create Container"))
                {
                    CreateContainer();
                }
            }
        }

        private void CreateContainer()
        {
            RoaRContainer container = CreateInstance<RoaRContainer>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, ".asset"));
            AssetDatabase.CreateAsset(container, path);
            
            container.Name = containerName;
            container.roarClipBank = CreateClipBank();
            
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = container;
        }

        private RoaRClipsBankSO CreateClipBank()
        {
            RoaRClipsBankSO bank = CreateInstance<RoaRClipsBankSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, "BANK.asset"));
            AssetDatabase.CreateAsset(bank, path);

            bank.audioClipsGroups.audioClips = new AudioClip[1];
            bank.audioClipsGroups.audioClips[0] = clips[0];
            
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = bank;

            return bank;
        }
    
        private void AddClipField()
        {
            //clips.Add(default(AudioClip));
            //clip = EditorGUILayout.ObjectField("AudioClip", clip, typeof(AudioClip), false) as AudioClip;
        }
    }
}
