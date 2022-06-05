using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace RoaREngine
{
    public class RoaRWindow : EditorWindow
    {
        Vector2 scrollPos = Vector2.zero;

        private List<RoaRContainer> containers = new List<RoaRContainer>();
        private List<string> containersName = new List<string>();
        private int index = 0;
        private int oldIndex = 0;

        private string containerName;
        private int clipsNumber = 1;
        private List<AudioClip> clips = new List<AudioClip>();

        private int clipIndex = 0;
        private Transform parent = null;
        private AudioMixerGroup audioMixerGroup = null;
        private PriorityLevel priority = PriorityLevel.Standard;
        private AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        private float startTime = 0f;
        private bool randomStartTime = false;
        private bool loop = false;
        private bool mute = false;
        private float volume = 1f;
        private float fadeInTime = 0f;
        private float fadeOutTime = 0f;
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
        private float dopplerLevel = 0f;
        private bool bypasseffects = false;
        private bool bypasslistenereffects = false;
        private bool bypassreverbzones = false;
        private bool ignorelistenervolume = false;
        private bool ignorelistenerpause = false;
        private bool onGoing = false;
        private float minTime = 0f;
        private float maxTime = 0f;
        private float minRandomXYZ = 0f;
        private float maxRandomXYZ = 0f;
        private bool measureEvent = false;
        private int bpm = 120;
        private int tempo = 4;
        private int everyNBar = 1;

        private void Awake()
        {
            clips.Clear();
            AudioClip clip = null;
            clips.Add(clip);
            containers.Clear();
            containersName.Clear();
            containersName.Add("New Container");
            foreach (var asset in AssetDatabase.FindAssets("t:RoaRContainer CONTAINER"))
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                containers.Add((RoaRContainer)AssetDatabase.LoadMainAssetAtPath(path));
                containersName.Add(AssetDatabase.LoadMainAssetAtPath(path).name);
            }
        }

        [MenuItem("RoaREngine/RoaRWindow")]
        private static void DisplayWindow()
        {
            RoaRWindow w = GetWindow<RoaRWindow>("RoaRWindow");
            w.Show();
        }


        private void OnGUI()
        {
            index = EditorGUILayout.Popup(index, containersName.ToArray());


            //texture = AssetPreview.GetAssetPreview(clips[0]);
            //GUILayout.Label(texture);
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            using (new GUILayout.VerticalScope())
            {
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
                if (GUILayout.Button("Save"))
                {
                    SaveContainerSettings(containers[index-1]);
                }
                if (GUILayout.Button("Reload Containers"))
                {
                    ReloadContainers();
                }
                GUI.enabled = wasGUIEnabled;
            }
            GUILayout.EndScrollView();
        }

        private void AudioClipConfiguration()
        {
            if (index != oldIndex)
            {
                if (index == 0)
                {
                    Default();
                    oldIndex = index;
                }
                else
                {
                    GetSettingsFromContainer();
                    oldIndex = index;
                }
            }

            #region settings
            containerName = EditorGUILayout.TextField("ContainerName", containerName);
            GUI.enabled = false;
            clipsNumber = EditorGUILayout.IntField("ClipsNumber", clipsNumber);
            GUI.enabled = true;
            for (int i = 0; i < clipsNumber; i++)
            {
                clips[i] = EditorGUILayout.ObjectField("AudioClips", clips[i], typeof(AudioClip), false) as AudioClip;
            }
            parent = EditorGUILayout.ObjectField("Parent", parent, typeof(Transform), true) as Transform;
            audioMixerGroup = EditorGUILayout.ObjectField("AudioMixerGroup", audioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
            priority = (PriorityLevel)EditorGUILayout.EnumPopup("Priority", priority);
            sequenceMode = (AudioSequenceMode)EditorGUILayout.EnumPopup("SequenceMode", sequenceMode);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("StartTime", GUILayout.Width(145));
                startTime = EditorGUILayout.FloatField(startTime, GUILayout.Width(25));
            }
            randomStartTime = EditorGUILayout.Toggle("RandomStartTime", randomStartTime);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Index", GUILayout.Width(145));
                clipIndex = EditorGUILayout.IntField(clipIndex, GUILayout.Width(25));
            }
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
                fadeInTime = EditorGUILayout.FloatField(fadeInTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("FadeOutVolume", GUILayout.Width(145));
                fadeOutTime = EditorGUILayout.FloatField(fadeOutTime, GUILayout.Width(25));
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
            bypasseffects = EditorGUILayout.Toggle("BypassEffects", bypasseffects);
            bypasslistenereffects = EditorGUILayout.Toggle("BypassListenerEffects", bypasslistenereffects);
            bypassreverbzones = EditorGUILayout.Toggle("BypassRevebZones", bypassreverbzones);
            ignorelistenervolume = EditorGUILayout.Toggle("IgnoreListenerVolume", ignorelistenervolume);
            ignorelistenerpause = EditorGUILayout.Toggle("IgnoreListenerPause", ignorelistenerpause);
            onGoing = EditorGUILayout.Toggle("OnGoing", onGoing);
            GUI.enabled = onGoing;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MinTime", GUILayout.Width(145));
                minTime = EditorGUILayout.FloatField(minTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MaxTime", GUILayout.Width(145));
                maxTime = EditorGUILayout.FloatField(maxTime, GUILayout.Width(25));
            }
            GUI.enabled = true;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MinRandomXYZ", GUILayout.Width(145));
                minRandomXYZ = EditorGUILayout.FloatField(minRandomXYZ, GUILayout.Width(35));
                minRandomXYZ = GUILayout.HorizontalSlider(minRandomXYZ, -500f, 500f);
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("MaxRandomXYZ", GUILayout.Width(145));
                maxRandomXYZ = EditorGUILayout.FloatField(maxRandomXYZ, GUILayout.Width(35));
                maxRandomXYZ = GUILayout.HorizontalSlider(maxRandomXYZ, -500f, 500f);
            }
            measureEvent = EditorGUILayout.Toggle("MeasureEvent", measureEvent);
            GUI.enabled = measureEvent;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("BPM", GUILayout.Width(145));
                bpm = EditorGUILayout.IntField(bpm, GUILayout.Width(35));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Tempo", GUILayout.Width(145));
                tempo = EditorGUILayout.IntField(tempo, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("EveryNBar", GUILayout.Width(145));
                everyNBar = EditorGUILayout.IntField(everyNBar, GUILayout.Width(25));
            }
            GUI.enabled = true;
            #endregion settings
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
            bank.SetClipIndex(clipIndex);

            config.parent = parent;
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
            config.fadeInTime = fadeInTime;
            config.fadeOutTime = fadeOutTime;
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
            config.onGoing = onGoing;
            config.minTime = minTime;
            config.maxTime = maxTime;
            config.minRandomXYZ = minRandomXYZ;
            config.maxRandomXYZ = maxRandomXYZ;
            config.measureEvent = measureEvent;
            config.bpm = bpm;
            config.tempo = tempo;
            config.everyNBar = everyNBar;
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
            clipsNumber++;
            AudioClip clip = null;
            clips.Add(clip);
        }

        private void RemoveClipField()
        {
            if (clipsNumber > 1)
            {
                clipsNumber--;
                clips.RemoveAt(clipsNumber);
            }
        }

        private void ReloadContainers()
        {
            containers.Clear();
            containersName.Clear();
            containersName.Add("New Container");
            foreach (var asset in AssetDatabase.FindAssets("t:RoaRContainer CONTAINER"))
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                containers.Add((RoaRContainer)AssetDatabase.LoadMainAssetAtPath(path));
                containersName.Add(AssetDatabase.LoadMainAssetAtPath(path).name);
            }
        }

        private void Default()
        {
            for (int i = 0; i < clips.Count; i++)
            {
                if (clipsNumber == 1)
                {
                    break;
                }
                clipsNumber--;
                clips.RemoveAt(clipsNumber);
            }

            parent = null;
            containerName = "";
            clipsNumber = 1;
            clips[0] = null;
            clipIndex = 0;
            audioMixerGroup = null;
            priority = PriorityLevel.Standard;
            sequenceMode = AudioSequenceMode.Sequential;
            startTime = 0f;
            randomStartTime = false;
            loop = false;
            mute = false;
            volume = 1f;
            fadeInTime = 0f;
            fadeOutTime = 0f;
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
            onGoing = false;
            minTime = 0f;
            maxTime = 0f;
            minRandomXYZ = 0f;
            maxRandomXYZ = 0f;
            measureEvent = false;
            bpm = 120;
            tempo = 4;
            everyNBar = 1;
        }

        private void SaveContainerSettings(RoaRContainer container)
        {
            if (containerName == "" || index == 0)
            {
                //TODO ERROR MESSAGE "A CONTAINER MUST HAVE A NAME"
                return;
            }
            container.Name = containerName;
            container.roarClipBank.audioClipsGroups.audioClips = clips.ToArray();
            ApplySettings(container.roarClipBank, container.roarConfiguration);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
        }
        
        private void GetSettingsFromContainer()
        {
            RoaRContainer container = containers[index - 1];
            RoaRClipsBankSO containerBank = container.roarClipBank;
            RoaRConfigurationSO containerConfiguration = container.roarConfiguration;
            containerName = container.Name;
            clipsNumber = containerBank.audioClipsGroups.audioClips.Length;
            for (int i = 0; i < clipsNumber; i++)
            {
                if (clips.Count < containerBank.audioClipsGroups.audioClips.Length)
                {
                    AudioClip clip = null;
                    clips.Add(clip);
                }
                clips[i] = containerBank.audioClipsGroups.audioClips[i];
            }
            parent = containerConfiguration.parent;
            audioMixerGroup = containerConfiguration.audioMixerGroup;
            priority = containerConfiguration.priority;
            sequenceMode = containerBank.audioClipsGroups.sequenceMode;
            startTime = containerConfiguration.startTime;
            randomStartTime = containerConfiguration.randomStartTime;
            clipIndex = containerBank.audioClipsGroups.Index;
            loop = containerConfiguration.loop;
            mute = containerConfiguration.mute;
            volume = containerConfiguration.volume;
            fadeInTime = containerConfiguration.fadeInTime;
            fadeOutTime = containerConfiguration.fadeOutTime;
            randomMinVolume = containerConfiguration.randomMinvolume;
            randomMaxVolume = containerConfiguration.randomMaxvolume;
            pitch = containerConfiguration.pitch;
            randomMinPitch = containerConfiguration.randomMinPitch;
            randomMaxPitch = containerConfiguration.randomMaxPitch;
            panStereo = containerConfiguration.panStereo;
            reverbZoneMix = containerConfiguration.reverbZoneMix;
            spatialBlend = containerConfiguration.spatialBlend;
            rolloffMode = containerConfiguration.rolloffMode;
            minDistance = containerConfiguration.minDistance;
            maxDistance = containerConfiguration.maxDistance;
            spread = containerConfiguration.spread;
            dopplerLevel = containerConfiguration.dopplerLevel;
            bypasseffects = containerConfiguration.bypasseffects;
            bypasslistenereffects = containerConfiguration.bypasslistenereffects;
            bypassreverbzones = containerConfiguration.bypassreverbzones;
            ignorelistenervolume = containerConfiguration.ignorelistenervolume;
            ignorelistenerpause = containerConfiguration.ignorelistenerpause;
            onGoing = containerConfiguration.onGoing;
            minTime = containerConfiguration.minTime;
            maxTime = containerConfiguration.maxTime;
            minRandomXYZ = containerConfiguration.minRandomXYZ;
            maxRandomXYZ = containerConfiguration.maxRandomXYZ;
            measureEvent = containerConfiguration.measureEvent;
            bpm = containerConfiguration.bpm;
            tempo = containerConfiguration.tempo;
            everyNBar = containerConfiguration.everyNBar;
        }
    }
}
