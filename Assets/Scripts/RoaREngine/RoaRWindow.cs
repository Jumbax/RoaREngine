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
        private int Index;
        private AudioMixerGroup audioMixerGroup = null;
        private PriorityLevel priority = PriorityLevel.Standard;
        private AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        private float startTime = 0f;
        private bool randomStartTime = false;
        private bool loop = false;
        private bool mute = false;
        private float volume = 1f;
        private float fadeInVolume = 0f;
        private float fadeOutVolume = 0f;
        private float randomMinVolume = 0f;
        private float randomMaxVolume = 0f;
        private float pitch = 1f;
        private float randomMinPitch = 0f;
        private float randomMaxPitch = 0f;
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
            clips.Clear();
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
                if (GUILayout.Button("Default"))
                {
                    Default();
                }
                GUI.enabled = wasGUIEnabled;
            }
        }

        private void AudioClipConfiguration()
        {
            audioMixerGroup = EditorGUILayout.ObjectField("AudioMixerGroup", audioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
            priority = (PriorityLevel)EditorGUILayout.EnumPopup("Priority", priority);
            sequenceMode = (AudioSequenceMode)EditorGUILayout.EnumPopup("SequenceMode", sequenceMode);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("StartTime", GUILayout.Width(145));
                startTime = EditorGUILayout.FloatField(startTime, GUILayout.Width(25));
            }
            randomStartTime = EditorGUILayout.Toggle("RandomStartTime", randomStartTime);
            Index = EditorGUILayout.IntField("Index", 0);
            loop = EditorGUILayout.Toggle("Loop", loop);
            mute = EditorGUILayout.Toggle("Mute", mute);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Volume", GUILayout.Width(145));
                volume = EditorGUILayout.FloatField(volume, GUILayout.Width(25));
                volume = GUILayout.HorizontalSlider(volume, 0f, 1f);
                volume = Mathf.Clamp(volume, 0f, 1f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("FadeInVolume", GUILayout.Width(145));
                fadeInVolume = EditorGUILayout.FloatField(fadeInVolume, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("FadeOutVolume", GUILayout.Width(145));
                fadeOutVolume = EditorGUILayout.FloatField(fadeOutVolume, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("RandomMinVolume", GUILayout.Width(145));
                randomMinVolume = EditorGUILayout.FloatField(randomMinVolume, GUILayout.Width(25));
                randomMinVolume = GUILayout.HorizontalSlider(randomMinVolume, 0f, 1f);
                randomMinVolume = Mathf.Clamp(randomMinVolume, 0f, 1f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("RandomMaxVolume", GUILayout.Width(145));
                randomMaxVolume = EditorGUILayout.FloatField(randomMaxVolume, GUILayout.Width(25));
                randomMaxVolume = GUILayout.HorizontalSlider(randomMaxVolume, 0f, 1f);
                randomMaxVolume = Mathf.Clamp(randomMaxVolume, 0f, 1f);
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
                GUILayout.Label("RandomMinPitch", GUILayout.Width(145));
                randomMinPitch = EditorGUILayout.FloatField(randomMinPitch, GUILayout.Width(25));
                randomMinPitch = GUILayout.HorizontalSlider(randomMinPitch, -3f, 3f);
                randomMinPitch = Mathf.Clamp(randomMinPitch, -3f, 3f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("randomMaxPitch", GUILayout.Width(145));
                randomMaxPitch = EditorGUILayout.FloatField(randomMaxPitch, GUILayout.Width(25));
                randomMaxPitch = GUILayout.HorizontalSlider(randomMaxPitch, -3f, 3f);
                randomMaxPitch = Mathf.Clamp(randomMaxPitch, -3f, 3f);
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
            bypasseffects = EditorGUILayout.Toggle("Bypasseffects", bypasseffects);
            bypasslistenereffects = EditorGUILayout.Toggle("Bypasslistenereffects", bypasslistenereffects);
            bypassreverbzones = EditorGUILayout.Toggle("Bypassreverbzones", bypassreverbzones);
            ignorelistenervolume = EditorGUILayout.Toggle("Ignorelistenervolume", ignorelistenervolume);
            ignorelistenerpause = EditorGUILayout.Toggle("Ignorelistenerpause", ignorelistenerpause);
        }

        private void CreateContainer()
        {
            if (containerName == "")
            {
                //TODO ERROR MESSAGE "A CONTAINER MUST HAVE A NAME"
                return;
            }
            RoaRContainer container = CreateInstance<RoaRContainer>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, "CONTAINER.asset"));
            AssetDatabase.CreateAsset(container, path);

            container.Name = containerName;
            container.roarClipBank = CreateClipBank();
            container.roarConfiguration = CreateConfiguration(container.roarClipBank);

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

        private void ApplySettings(RoaRClipsBankSO bank, RoaRConfigurationSO config)
        {
            bank.audioClipsGroups.sequenceMode = sequenceMode;
            bank.SetClipIndex(Index);

            config.audioMixerGroup = audioMixerGroup;
            config.startTime = startTime;
            config.randomStartTime = randomStartTime;
            config.loop = loop;
            config.mute = mute;
            config.bypasseffects = bypasseffects;
            config.bypasslistenereffects = bypasslistenereffects;
            config.bypassreverbzones = bypassreverbzones;
            config.priority = priority;
            config.volume = volume;
            config.fadeInvolume = fadeInVolume;
            config.fadeOutvolume = fadeOutVolume;
            config.randomMinvolume = randomMinVolume;
            config.randomMaxvolume = randomMaxVolume;
            config.pitch = pitch;
            config.randomMinPitch = randomMaxPitch;
            config.randomMaxPitch = randomMaxPitch;
            config.panStereo = panStereo;
            config.spatialBlend = spatialBlend;
            config.reverbZoneMix = reverbZoneMix;
            config.dopplerLevel = dopplerLevel;
            config.spread = spread;
            config.rolloffMode = rolloffMode;
            config.minDistance = minDistance;
            config.maxDistance = maxDistance;
            config.ignorelistenervolume = ignorelistenervolume;
            config.ignorelistenerpause = ignorelistenerpause;
        }

        private RoaRConfigurationSO CreateConfiguration(RoaRClipsBankSO bank)
        {
            RoaRConfigurationSO config = CreateInstance<RoaRConfigurationSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat("Assets/Test/", containerName, "CONFIG.asset"));
            AssetDatabase.CreateAsset(config, path);

            ApplySettings(bank, config);

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

        private void Default()
        {
            for (int i = 0; i < clips.Count; i++)
            {
                if (ClipsNumber == 1)
                {
                    break;
                }
                ClipsNumber--;
                clips.RemoveAt(ClipsNumber);
            }

            containerName = "";
            ClipsNumber = 1;
            Index = 0;
            audioMixerGroup = null;
            priority = PriorityLevel.Standard;
            sequenceMode = AudioSequenceMode.Sequential;
            startTime = 0f;
            randomStartTime = false;
            loop = false;
            mute = false;
            volume = 1f;
            fadeInVolume = 0f;
            fadeOutVolume = 0f;
            randomMinVolume = 0f;
            randomMaxVolume = 0f;
            pitch = 1f;
            randomMinPitch = 0f;
            randomMaxPitch = 0f;
            panStereo = 0f;
            reverbZoneMix = 1f;
            spatialBlend = 0f;
            rolloffMode = AudioRolloffMode.Logarithmic;
            minDistance = 0.1f;
            maxDistance = 50f;
            spread = 0;
            dopplerLevel = 1f;
            bypasseffects = false;
            bypasslistenereffects = false;
            bypassreverbzones = false;
            ignorelistenervolume = false;
            ignorelistenerpause = false;
        }
    }
}
