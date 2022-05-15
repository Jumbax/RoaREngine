using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace RoaREngine
{
    public class RoaRWindow : EditorWindow
    {
        private string containerName;
        private int ClipsNumber = 1;
        private List<AudioClip> clips = new List<AudioClip>();

        private AudioMixerGroup audioMixerGroup = null;
        private PriorityLevel priority = PriorityLevel.Standard;
        private bool loop = false;
        private bool mute = false;
        private bool randomPitch;
        private float volume = 1f;
        private float pitch = 1f;
        private float panStereo = 0f;
        private float reverbZoneMix = 1f;
        private float spatialBlend = 0f;
        private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        private float minDistance = 0.1f;
        private float maxDistance = 50f;
        private int spread = 0;
        private float dopplerLevel = 1f;
        private bool bypasseffects = false;
        private bool bypasslistenereffects = false;
        private bool bypassreverbzones = false;
        private bool ignorelistenervolume = false;
        private bool ignorelistenerpause = false;

        [MenuItem("RoaREngine/RoaRWindow")]
        private static void DisplayWindow()
        {
            RoaRWindow w = GetWindow<RoaRWindow>("RoaRWindow");
            w.Show();
        }

        private void Awake()
        {
            AudioClip clip = null;
            clips.Add(clip);

        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                containerName = EditorGUILayout.TextField("ContainerName", containerName);

                //FIX THIS
                GUI.enabled = false;
                ClipsNumber = EditorGUILayout.IntField("ClipsNumber", ClipsNumber);
                GUI.enabled = true;

                for (int i = 0; i < ClipsNumber; i++)
                {
                    clips[i] = EditorGUILayout.ObjectField("AudioClips", clips[i], typeof(AudioClip), false) as AudioClip;
                }

                AudioClipConfiguration();

                bool wasGUIEnabled = GUI.enabled;
                if (GUILayout.Button("Add Clip"))
                {
                    AddClipField();
                }
                if (GUILayout.Button("Remove Clip"))
                {
                    RemoveClipField();
                }
                if (GUILayout.Button("Create Container"))
                {
                    CreateContainer();
                }
                GUI.enabled = wasGUIEnabled;
            }
        }

        private void AudioClipConfiguration()
        {
            audioMixerGroup = EditorGUILayout.ObjectField("AudioMixerGroup", audioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
            priority = (PriorityLevel)EditorGUILayout.EnumPopup("Priority", priority);
            loop = EditorGUILayout.Toggle("Loop", loop);
            mute = EditorGUILayout.Toggle("Mute", mute);
            randomPitch = EditorGUILayout.Toggle("randomPitch", randomPitch);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Volume", GUILayout.Width(145));
                volume = EditorGUILayout.FloatField(volume, GUILayout.Width(25));
                volume = GUILayout.HorizontalSlider(volume, 0f, 1f);
                volume = Mathf.Clamp(volume, 0f, 1f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Pitch", GUILayout.Width(145));
                pitch = EditorGUILayout.FloatField(pitch, GUILayout.Width(25));
                pitch = GUILayout.HorizontalSlider(pitch, -3f, 3f);
                pitch = Mathf.Clamp(pitch, -3f, 3f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("PanStereo", GUILayout.Width(145));
                panStereo = EditorGUILayout.FloatField(panStereo, GUILayout.Width(25));
                panStereo = GUILayout.HorizontalSlider(panStereo, 0f, 1f);
                panStereo = Mathf.Clamp(panStereo, 0f, 1f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("ReverbZoneMix", GUILayout.Width(145));
                reverbZoneMix = EditorGUILayout.FloatField(reverbZoneMix, GUILayout.Width(25));
                reverbZoneMix = GUILayout.HorizontalSlider(reverbZoneMix, 0f, 1f);
                reverbZoneMix = Mathf.Clamp(reverbZoneMix, 0f, 1f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("SpatialBlend", GUILayout.Width(145));
                spatialBlend = EditorGUILayout.FloatField(spatialBlend, GUILayout.Width(25));
                spatialBlend = GUILayout.HorizontalSlider(spatialBlend, 0f, 1f);
                spatialBlend = Mathf.Clamp(spatialBlend, 0f, 1f);
            }
            rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Mode", rolloffMode);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MinDistance", GUILayout.Width(145));
                minDistance = EditorGUILayout.FloatField(minDistance, GUILayout.Width(25));
                minDistance = GUILayout.HorizontalSlider(minDistance, 0.01f, 5f);
                minDistance = Mathf.Clamp(minDistance, 0.01f, 5f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MaxDistance", GUILayout.Width(145));
                maxDistance = EditorGUILayout.FloatField(maxDistance, GUILayout.Width(25));
                maxDistance = GUILayout.HorizontalSlider(maxDistance, 5f, 100f);
                maxDistance = Mathf.Clamp(maxDistance, 5f, 100f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Spread", GUILayout.Width(145));
                spread = EditorGUILayout.IntField(spread, GUILayout.Width(25));
                spread = Mathf.RoundToInt(GUILayout.HorizontalSlider(spread, 0, 360));
                spread = Mathf.Clamp(spread, 0, 360);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("DopplerLevel", GUILayout.Width(145));
                dopplerLevel = EditorGUILayout.FloatField(dopplerLevel, GUILayout.Width(25));
                dopplerLevel = GUILayout.HorizontalSlider(dopplerLevel, 0f, 5f);
                dopplerLevel = Mathf.Clamp(dopplerLevel, 0f, 5f);
            }
            bypasseffects = EditorGUILayout.Toggle("Bypasseffects", randomPitch);
            bypasslistenereffects = EditorGUILayout.Toggle("Bypasslistenereffects", randomPitch);
            bypassreverbzones = EditorGUILayout.Toggle("Bypassreverbzones", randomPitch);
            ignorelistenervolume = EditorGUILayout.Toggle("Ignorelistenervolume", randomPitch);
            ignorelistenerpause = EditorGUILayout.Toggle("Ignorelistenerpause", randomPitch);
    }

        private void CreateContainer()
        {
            RoaRContainer container = CreateInstance<RoaRContainer>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, ".asset"));
            AssetDatabase.CreateAsset(container, path);

            container.Name = containerName;
            container.roarClipBank = CreateClipBank();
            container.roarConfiguration = CreateConfiguration();

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = container;
        }

        private RoaRClipsBankSO CreateClipBank()
        {
            RoaRClipsBankSO bank = CreateInstance<RoaRClipsBankSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, "BANK.asset"));
            AssetDatabase.CreateAsset(bank, path);

            bank.audioClipsGroups.audioClips = clips.ToArray();

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = bank;

            return bank;
        }
        
        private void ApplyConfiguration(RoaRConfigurationSO config) 
        {
            config.audioMixerGroup = this.audioMixerGroup;
            config.loop = loop;
            config.mute = this.mute;
            config.bypasseffects = this.bypasseffects;
            config.bypasslistenereffects = this.bypasslistenereffects;
            config.bypassreverbzones = this.bypassreverbzones;
            config.priority = this.priority;
            config.volume = this.volume;
            config.pitch = this.pitch;
            config.panStereo = this.panStereo;
            config.spatialBlend = this.spatialBlend;
            config.reverbZoneMix = this.reverbZoneMix;
            config.dopplerLevel = this.dopplerLevel;
            config.spread = this.spread;
            config.rolloffMode = this.rolloffMode;
            config.minDistance = this.minDistance;
            config.maxDistance = this.maxDistance;
            config.ignorelistenervolume = this.ignorelistenervolume;
            config.ignorelistenerpause = this.ignorelistenerpause;
        }

        private RoaRConfigurationSO CreateConfiguration()
        {
            RoaRConfigurationSO config = CreateInstance<RoaRConfigurationSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, "CONFIG.asset"));
            AssetDatabase.CreateAsset(config, path);

            ApplyConfiguration(config);

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;

            return config;
        }
        private void AddClipField()
        {
            ClipsNumber++;
            AudioClip clip = null;
            clips.Add(clip);
        }

        private void RemoveClipField()
        {
            if (ClipsNumber > 1)
            {
                ClipsNumber--;
                clips.RemoveAt(ClipsNumber);
            }
        }
    }
}
