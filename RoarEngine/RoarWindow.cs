using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Audio;

namespace RoaREngine
{
    public class RoarWindow : EditorWindow
    {
        #region var
        private string folderPath;

        Vector2 scrollPos = Vector2.zero;
        private RoarContainerSO containerEditor = null;
        RoarClipsBankSO bankEditor = null;
        RoarConfigurationSO configEditor = null;
        private List<RoarContainerSO> containers = new List<RoarContainerSO>();
        private List<string> containersName = new List<string>();
        private int containerIndex = 0;
        private int containerOldIndex = 0;
        private GameObject emitterEditor;

        private string containerName;
        public static bool canChangeContainer = true;
        private RoarConfigurationSO config = null;
        private RoarConfigurationSO oldConfig = null;
        private RoarClipsBankSO bank = null;
        private RoarClipsBankSO oldBank = null;
        private int clipsNumber = 0;
        private List<AudioClip> clips = new List<AudioClip>();
        private int clipIndex = 0;
        private AudioMixerGroup audioMixerGroup = null;
        private PriorityLevel priority = PriorityLevel.Standard;
        private AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        private float startTime = 0f;
        private bool esclusive = false;
        private bool randomStartTime = false;
        private bool loop = false;
        private bool mute = false;
        private float volume = 1f;
        private float fadeInVolume = 0f;
        private float playFadeTime = 0f;
        private float pauseFadeTime = 0f;
        private float resumeFadeTime = 0f;
        private float stopFadeTime = 0f;
        private float delay = 0f;
        private float randomMinVolume = 0f;
        private float randomMaxVolume = 0f;
        private float pitch = 1f;
        private float randomMinPitch = 0f;
        private float randomMaxPitch = 0f;
        private float stereoPan = 0f;
        private float reverbZoneMix = 1f;
        private bool is3D = false;
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
        private bool isOnGoing = false;
        private float minTime = 0f;
        private float maxTime = 0f;
        private float minRandomXYZ = 0f;
        private float maxRandomXYZ = 0f;
        private bool measureEvent = false;
        private bool hasMeasureEvent = false;
        private int bpm = 120;
        private int beats = 4;
        private int measure = 4;
        private int everyNBeat = 1;
        private bool timedEvent = false;
        private bool hasTimedEvent = false;
        private bool repeatTimedEvent;
        public float timedEventTime = 0f;
        public bool hasControlEvent = false;
        public bool playEvent = false;
        public bool pauseEvent = false;
        public bool resumeEvent = false;
        public bool stopEvent = false;
        public bool finishedEvent = false;
        private bool addEffect = false;
        private AnimBool chorus = new AnimBool(false);
        private float chorusDryMix = 0.5f;
        private float chorusWetMix1 = 0.5f;
        private float chorusWetMix2 = 0.5f;
        private float chorusWetMix3 = 0.5f;
        private float chorusDelay = 40f;
        private float chorusRate = 0.8f;
        private float chorusDepth = 0.03f;
        private AnimBool distortion = new AnimBool(false);
        private float distortionLevel = 0.5f;
        private AnimBool echo = new AnimBool(false);
        private int echoDelay = 500;
        private float echoDecayRatio = 0.5f;
        private float echoDryMix = 1f;
        private float echoWetMix = 1f;
        private AnimBool highPass = new AnimBool(false);
        private int highPassCutoffFrequency = 5000;
        private float highPassResonanceQ = 1;
        private AnimBool lowPass = new AnimBool(false);
        private int lowPassCutoffFrequency = 5000;
        private float lowPassResonanceQ = 1;
        private AnimBool reverbFilter = new AnimBool(false);
        private int reverbFilterDryLevel = 0;
        private int reverbFilterRoom = 0;
        private int reverbFilterRoomHF = 0;
        private int reverbFilterRoomLF = 0;
        private float reverbFilterDecayTime = 1f;
        private float reverbFilterDecayHFRatio = 0.5f;
        private int reverbFilterReflectionsLevel = -10000;
        private float reverbFilterReflectionsDelay = 0f;
        private int reverbFilterReverbLevel = 0;
        private float reverbFilterReverbDelay = 0.04f;
        private int reverbFilterHFReference = 5000;
        private int reverbFilterLFReference = 250;
        private float reverbFilterDiffusion = 100f;
        private float reverbFilterDensity = 100f;
        private AnimBool reverbZone = new AnimBool(false);
        private float reverbZoneMinDistance = 10f;
        private float reverbZoneMaxDistance = 15f;
        private int reverbZoneRoom = -1000;
        private int reverbZoneRoomHF = -100;
        private int reverbZoneRoomLF = 0;
        private float reverbZoneDecayTime = 1.49f;
        private float reverbZoneDecayHFRatio = 0.83f;
        private int reverbZoneReflections = -2602;
        private float reverbZoneReflectionsDelay = 0.007f;
        private int reverbZoneReverb = 200;
        private float reverbZoneReverbDelay = 0.011f;
        private int reverbZoneHFReference = 5000;
        private int reverbZoneLFReference = 250;
        private float reverbZoneDiffusion = 100f;
        private float reverbZoneDensity = 100f;
        private bool isInACrossFade = false;
        private float fadeInParamValueStart = 0f;
        private float fadeInParamValueEnd = 0f;
        private float fadeOutParamValueStart = 0f;
        private float fadeOutParamValueEnd = 0f;
        #endregion

        #region func
        private void Awake()
        {
            chorus.valueChanged.AddListener(Repaint);
            distortion.valueChanged.AddListener(Repaint);
            echo.valueChanged.AddListener(Repaint);
            highPass.valueChanged.AddListener(Repaint);
            lowPass.valueChanged.AddListener(Repaint);
            reverbFilter.valueChanged.AddListener(Repaint);
            reverbZone.valueChanged.AddListener(Repaint);
            clips.Clear();
            containers.Clear();
            containersName.Clear();
            containersName.Add("New Container");
            foreach (var asset in AssetDatabase.FindAssets("t:RoaRContainerSO CONTAINER"))
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                containers.Add((RoarContainerSO)AssetDatabase.LoadMainAssetAtPath(path));
                containersName.Add(AssetDatabase.LoadMainAssetAtPath(path).name);
            }
            if (!PlayerPrefs.HasKey("FolderPath"))
            {
                CreateFolderPath();
            }
            else
            {
                LoadFolderPath();
            }
        }

        [MenuItem("RoarEngine/RoarWindow")]
        private static void DisplayWindow()
        {
            RoarWindow w = GetWindow<RoarWindow>("RoarWindow");
            w.Show();
        }

        private void CreateFolderPath()
        {
            string path = EditorUtility.OpenFolderPanel("Choose a folder for RoaR Assets", "Assets", "");
            if (path.Length > 0)
            {
                folderPath = "Assets" + path.Substring(Application.dataPath.Length);
                PlayerPrefs.SetString("FolderPath", folderPath);
            }
        }

        private void LoadFolderPath()
        {
            folderPath = PlayerPrefs.GetString("FolderPath");
        }

        private void OnGUI()
        {
            if (containerIndex != containerOldIndex)
            {
                if (containerIndex == 0)
                {
                    containerName = "";
                    bank = null;
                    config = null;
                    DefaultContainer();
                    containerOldIndex = containerIndex;
                }
                else
                {
                    GetSettingsFromContainer();
                    containerOldIndex = containerIndex;
                }
            }
            if (bank != oldBank)
            {
                if (bank == null)
                {
                    DefaultBank();
                    oldBank = bank;
                }
                else
                {
                    GetSettingsFromBank();
                    oldBank = bank;
                }
            }
            if (config != oldConfig)
            {
                if (config == null)
                {
                    DefaultConfiguration();
                    oldConfig = config;
                }
                else
                {
                    GetSettingsFromConfiguration();
                    oldConfig = config;
                }
            }
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField("CONTAINER: " + containersName[containerIndex]);
                    GUI.enabled = canChangeContainer;
                    containerIndex = EditorGUILayout.Popup(containerIndex, containersName.ToArray());
                    GUI.enabled = true;
                    ContainerSettings();
                    EditorGUILayout.Space(25f);
                    string bankName = bank != null ? bank.name : "None";
                    EditorGUILayout.LabelField("BANK: " + bankName);
                    BankSettings();
                }
                EditorGUILayout.Space(50f);
                using (new GUILayout.VerticalScope())
                {
                    string configName = config != null ? config.name : "None";
                    EditorGUILayout.LabelField("CONFIGURATION: " + configName);
                    ConfigurationSettings();
                }
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("PLAY"))
            {
                PlayInEditor();
            }
            if (GUILayout.Button("STOP"))
            {
                StopInEditor();
            }
            if (GUILayout.Button("PAUSE"))
            {
                PauseInEditor();
            }
            if (GUILayout.Button("RESUME"))
            {
                ResumeInEditor();
            }
        }

        private void ContainerSettings()
        {
            containerName = EditorGUILayout.TextField("ContainerName", containerName);
            config = EditorGUILayout.ObjectField("Configuration", config, typeof(RoarConfigurationSO), false) as RoarConfigurationSO;
            bank = EditorGUILayout.ObjectField("Bank", bank, typeof(RoarClipsBankSO), false) as RoarClipsBankSO;
            if (GUILayout.Button("CREATE CONTAINER"))
            {
                CreateContainer();
            }
            if (GUILayout.Button("DEFAULT CONTAINER"))
            {
                DefaultContainer();
            }
            if (GUILayout.Button("SAVE CONTAINER"))
            {
                SaveContainerSettings();
            }
            if (GUILayout.Button("RELOAD CONTAINERS"))
            {
                ReloadContainers();
            }
            if (GUILayout.Button("CHANGE FOLDER PATH"))
            {
                CreateFolderPath();
            }
        }

        private void BankSettings()
        {
            sequenceMode = (AudioSequenceMode)EditorGUILayout.EnumPopup("SequenceMode", sequenceMode);
            GUI.enabled = false;
            clipsNumber = EditorGUILayout.IntField("ClipsNumber", clipsNumber);
            GUI.enabled = true;
            for (int i = 0; i < clipsNumber; i++)
            {
                clips[i] = EditorGUILayout.ObjectField("AudioClips", clips[i], typeof(AudioClip), false) as AudioClip;
            }
            GUI.enabled = sequenceMode == AudioSequenceMode.Choice;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Start From", GUILayout.Width(145));
                clipIndex = EditorGUILayout.IntField(clipIndex, GUILayout.Width(25));
                clipIndex = Mathf.Clamp(clipIndex, 0, clips.Count);
            }
            GUI.enabled = true;
            if (GUILayout.Button("ADD CLIP"))
            {
                AddClipField();
            }
            if (GUILayout.Button("REMOVE CLIP"))
            {
                RemoveClipField();
            }
        }

        private void ConfigurationSettings()
        {
            audioMixerGroup = EditorGUILayout.ObjectField("AudioMixerGroup", audioMixerGroup, typeof(AudioMixerGroup), false) as AudioMixerGroup;
            priority = (PriorityLevel)EditorGUILayout.EnumPopup("Priority", priority);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("StartTime", GUILayout.Width(145));
                startTime = EditorGUILayout.FloatField(startTime, GUILayout.Width(25));
            }
            esclusive = EditorGUILayout.Toggle("Esclusive", esclusive);
            randomStartTime = EditorGUILayout.Toggle("Random Start Time", randomStartTime);
            loop = EditorGUILayout.Toggle("Loop", loop);
            mute = EditorGUILayout.Toggle("Mute", mute);
            volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);
            fadeInVolume = EditorGUILayout.Slider("FadeIn Volume", fadeInVolume, 0f, 1f);
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Play FadeIn Time", GUILayout.Width(145));
                playFadeTime = EditorGUILayout.FloatField(playFadeTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Pause FadeOut Time", GUILayout.Width(145));
                pauseFadeTime = EditorGUILayout.FloatField(pauseFadeTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Resume FadeIn Time", GUILayout.Width(145));
                resumeFadeTime = EditorGUILayout.FloatField(resumeFadeTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Stop FadeOut Time", GUILayout.Width(145));
                stopFadeTime = EditorGUILayout.FloatField(stopFadeTime, GUILayout.Width(25));
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Delay", GUILayout.Width(145));
                delay = EditorGUILayout.FloatField(delay, GUILayout.Width(25));
            }
            randomMinVolume = EditorGUILayout.Slider("Random Min Volume", randomMinVolume, 0f, 1f);
            randomMaxVolume = EditorGUILayout.Slider("Random Max Volume", randomMaxVolume, 0f, 1f);
            pitch = EditorGUILayout.Slider("Pitch", pitch, -3f, 3f);
            randomMinPitch = EditorGUILayout.Slider("Random Min Pitch", randomMinPitch, -3f, 3f);
            randomMaxPitch = EditorGUILayout.Slider("Random Max Pitch", randomMaxPitch, -3f, 3f);
            stereoPan = EditorGUILayout.Slider("Stereo Pan", stereoPan, -1f, 1f);
            reverbZoneMix = EditorGUILayout.Slider("Reverb Zone Mix", reverbZoneMix, 0f, 1f);
            bypasseffects = EditorGUILayout.Toggle("BypassEffects", bypasseffects);
            bypasslistenereffects = EditorGUILayout.Toggle("BypassListenerEffects", bypasslistenereffects);
            bypassreverbzones = EditorGUILayout.Toggle("BypassRevebZones", bypassreverbzones);
            ignorelistenervolume = EditorGUILayout.Toggle("IgnoreListenerVolume", ignorelistenervolume);
            ignorelistenerpause = EditorGUILayout.Toggle("IgnoreListenerPause", ignorelistenerpause);

            is3D = EditorGUILayout.BeginFoldoutHeaderGroup(is3D, "3D");
            if (is3D)
            {
                spatialBlend = EditorGUILayout.Slider("Spatial Blend", spatialBlend, 0f, 1f);
                minDistance = EditorGUILayout.Slider("Min Distance", minDistance, 0.01f, 5f);
                rolloffMode = (AudioRolloffMode)EditorGUILayout.EnumPopup("Mode", rolloffMode);
                maxDistance = EditorGUILayout.Slider("Max Distance", maxDistance, 5f, 100f);
                spread = EditorGUILayout.IntSlider("Spread", spread, 0, 360);
                dopplerLevel = EditorGUILayout.Slider("Doopler Level", dopplerLevel, 0f, 5f);
                minRandomXYZ = EditorGUILayout.Slider("Min Random XYZ", minRandomXYZ, -500f, 500f);
                maxRandomXYZ = EditorGUILayout.Slider("Max Random XYZ", maxRandomXYZ, -500f, 500f);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            isOnGoing = EditorGUILayout.BeginFoldoutHeaderGroup(isOnGoing, "OnGoing");
            if (isOnGoing)
            {
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
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            hasMeasureEvent = EditorGUILayout.BeginFoldoutHeaderGroup(hasMeasureEvent, "MeasureEvent");
            if (hasMeasureEvent)
            {
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
                    beats = EditorGUILayout.IntField(beats, GUILayout.Width(25));
                    measure = EditorGUILayout.IntField(measure, GUILayout.Width(25));
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("EveryNBeat", GUILayout.Width(145));
                    everyNBeat = EditorGUILayout.IntField(everyNBeat, GUILayout.Width(25));
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            hasTimedEvent = EditorGUILayout.BeginFoldoutHeaderGroup(hasTimedEvent, "Timed Event");
            if (hasTimedEvent)
            {
                timedEvent = EditorGUILayout.Toggle("Timed Event", timedEvent);
                GUI.enabled = timedEvent;
                repeatTimedEvent = EditorGUILayout.Toggle("Repeat Timed Event", repeatTimedEvent);
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Timed Event Time", GUILayout.Width(145));
                    timedEventTime = EditorGUILayout.FloatField(timedEventTime, GUILayout.Width(25));
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            hasControlEvent = EditorGUILayout.BeginFoldoutHeaderGroup(hasControlEvent, "Control Events");
            if (hasControlEvent)
            {
                playEvent = EditorGUILayout.Toggle("Play Event", playEvent);
                pauseEvent = EditorGUILayout.Toggle("Pause Event", pauseEvent);
                resumeEvent = EditorGUILayout.Toggle("Resume Event", resumeEvent);
                stopEvent = EditorGUILayout.Toggle("Stop Event", stopEvent);
                finishedEvent = EditorGUILayout.Toggle("Finished Event", finishedEvent);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            isInACrossFade = EditorGUILayout.BeginFoldoutHeaderGroup(isInACrossFade, "Cross Fade");
            if (isInACrossFade)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("FadeIn Param Value Start", GUILayout.Width(145));
                    fadeInParamValueStart = EditorGUILayout.FloatField(fadeInParamValueStart, GUILayout.Width(25));
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("FadeIn Param Value End", GUILayout.Width(145));
                    fadeInParamValueEnd = EditorGUILayout.FloatField(fadeInParamValueEnd, GUILayout.Width(25));
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("FadeOut Param Value Start", GUILayout.Width(145));
                    fadeOutParamValueStart = EditorGUILayout.FloatField(fadeOutParamValueStart, GUILayout.Width(25));
                }
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("FadeOut Param Value Start", GUILayout.Width(145));
                    fadeOutParamValueStart = EditorGUILayout.FloatField(fadeOutParamValueStart, GUILayout.Width(25));
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            addEffect = EditorGUILayout.BeginFoldoutHeaderGroup(addEffect, "Add Effect");
            if (addEffect)
            {
                chorus.target = EditorGUILayout.Toggle("Chorus", chorus.target);
                if (EditorGUILayout.BeginFadeGroup(chorus.faded))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Dry Mix", GUILayout.Width(145));
                        chorusDryMix = EditorGUILayout.FloatField(chorusDryMix, GUILayout.Width(25));
                        chorusDryMix = Mathf.Clamp(chorusDryMix, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Wet Mix 1", GUILayout.Width(145));
                        chorusWetMix1 = EditorGUILayout.FloatField(chorusWetMix1, GUILayout.Width(25));
                        chorusWetMix1 = Mathf.Clamp(chorusWetMix1, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Wet Mix 2", GUILayout.Width(145));
                        chorusWetMix2 = EditorGUILayout.FloatField(chorusWetMix2, GUILayout.Width(25));
                        chorusWetMix2 = Mathf.Clamp(chorusWetMix2, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Wet Mix 3", GUILayout.Width(145));
                        chorusWetMix3 = EditorGUILayout.FloatField(chorusWetMix3, GUILayout.Width(25));
                        chorusWetMix3 = Mathf.Clamp(chorusWetMix3, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Delay", GUILayout.Width(145));
                        chorusDelay = EditorGUILayout.FloatField(chorusDelay, GUILayout.Width(25));
                        chorusDelay = Mathf.Clamp(chorusDelay, 0.1f, 100f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Rate", GUILayout.Width(145));
                        chorusRate = EditorGUILayout.FloatField(chorusRate, GUILayout.Width(25));
                        chorusRate = Mathf.Clamp(chorusRate, 0f, 20f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Depth", GUILayout.Width(145));
                        chorusDepth = EditorGUILayout.FloatField(chorusDepth, GUILayout.Width(25));
                        chorusDepth = Mathf.Clamp(chorusDepth, 0f, 1f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                distortion.target = EditorGUILayout.Toggle("Distortion", distortion.target);
                if (EditorGUILayout.BeginFadeGroup(distortion.faded))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Distortion Level", GUILayout.Width(145));
                        distortionLevel = EditorGUILayout.FloatField(distortionLevel, GUILayout.Width(25));
                        distortionLevel = Mathf.Clamp(distortionLevel, 0f, 1f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                echo.target = EditorGUILayout.Toggle("Echo", echo.target);
                if (EditorGUILayout.BeginFadeGroup(echo.faded))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Delay", GUILayout.Width(145));
                        echoDelay = EditorGUILayout.IntField(echoDelay, GUILayout.Width(25));
                        echoDelay = Mathf.Clamp(echoDelay, 10, 5000);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Decay Ratio", GUILayout.Width(145));
                        echoDecayRatio = EditorGUILayout.FloatField(echoDecayRatio, GUILayout.Width(25));
                        echoDecayRatio = Mathf.Clamp(echoDecayRatio, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Dry Mix", GUILayout.Width(145));
                        echoDryMix = EditorGUILayout.FloatField(echoDryMix, GUILayout.Width(25));
                        echoDryMix = Mathf.Clamp(echoDryMix, 0f, 1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Wet Mix", GUILayout.Width(145));
                        echoWetMix = EditorGUILayout.FloatField(echoWetMix, GUILayout.Width(25));
                        echoWetMix = Mathf.Clamp(echoWetMix, 0f, 1f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                highPass.target = EditorGUILayout.Toggle("HighPass", highPass.target);
                if (EditorGUILayout.BeginFadeGroup(highPass.faded))
                {
                    highPassCutoffFrequency = EditorGUILayout.IntSlider("Cutoff Frequency", highPassCutoffFrequency, 10, 22000);
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Highpass Resonance Q", GUILayout.Width(145));
                        highPassResonanceQ = EditorGUILayout.FloatField(highPassResonanceQ, GUILayout.Width(25));
                        highPassResonanceQ = Mathf.Clamp(highPassResonanceQ, 1f, 10f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                lowPass.target = EditorGUILayout.Toggle("LowPass", lowPass.target);
                if (EditorGUILayout.BeginFadeGroup(lowPass.faded))
                {
                    lowPassCutoffFrequency = EditorGUILayout.IntSlider("Cutoff Frequency", lowPassCutoffFrequency, 10, 22000);
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Highpass Resonance Q", GUILayout.Width(145));
                        lowPassResonanceQ = EditorGUILayout.FloatField(lowPassResonanceQ, GUILayout.Width(25));
                        lowPassResonanceQ = Mathf.Clamp(lowPassResonanceQ, 1f, 10f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                reverbFilter.target = EditorGUILayout.Toggle("ReverbFilter", reverbFilter.target);
                if (EditorGUILayout.BeginFadeGroup(reverbFilter.faded))
                {
                    reverbFilterDryLevel = EditorGUILayout.IntSlider("Dry Level", reverbFilterDryLevel, -10000, 0);
                    reverbFilterRoom = EditorGUILayout.IntSlider("Filter Room", reverbFilterRoom, -10000, 0);
                    reverbFilterRoomHF = EditorGUILayout.IntSlider("Room HF", reverbFilterRoomHF, -10000, 0);
                    reverbFilterRoomLF = EditorGUILayout.IntSlider("Room LF", reverbFilterRoomLF, -10000, 0);
                    reverbFilterDecayTime = EditorGUILayout.Slider("Decay Time", reverbFilterDecayTime, 0.1f, 20f);
                    reverbFilterDecayHFRatio = EditorGUILayout.Slider("Decay HF Ratio", reverbFilterDecayHFRatio, 0.1f, 2f);
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Reflections Level", GUILayout.Width(145));
                        reverbFilterReflectionsLevel = EditorGUILayout.IntField(reverbFilterReflectionsLevel, GUILayout.Width(25));
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Reflections Delay", GUILayout.Width(145));
                        reverbFilterReflectionsDelay = EditorGUILayout.FloatField(reverbFilterReflectionsDelay, GUILayout.Width(25));
                        reverbFilterReflectionsDelay = Mathf.Clamp(reverbFilterReflectionsDelay, 0f, 0.3f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Reverb Level", GUILayout.Width(145));
                        reverbFilterReverbLevel = EditorGUILayout.IntField(reverbFilterReverbLevel, GUILayout.Width(25));
                        reverbFilterReverbLevel = Mathf.Clamp(reverbFilterReverbLevel, -10000, 2000);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Reverb Delay", GUILayout.Width(145));
                        reverbFilterReverbDelay = EditorGUILayout.FloatField(reverbFilterReverbDelay, GUILayout.Width(25));
                        reverbFilterReverbDelay = Mathf.Clamp(reverbFilterReverbDelay, 0f, 0.1f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("HF Reference", GUILayout.Width(145));
                        reverbFilterHFReference = EditorGUILayout.IntField(reverbFilterHFReference, GUILayout.Width(25));
                        reverbFilterHFReference = Mathf.Clamp(reverbFilterHFReference, 1000, 20000);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("LF Reference", GUILayout.Width(145));
                        reverbFilterLFReference = EditorGUILayout.IntField(reverbFilterLFReference, GUILayout.Width(25));
                        reverbFilterLFReference = Mathf.Clamp(reverbFilterLFReference, 20, 1000);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Diffusion", GUILayout.Width(145));
                        reverbFilterDiffusion = EditorGUILayout.FloatField(reverbFilterDiffusion, GUILayout.Width(25));
                        reverbFilterDiffusion = Mathf.Clamp(reverbFilterDiffusion, 0f, 100f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Density", GUILayout.Width(145));
                        reverbFilterDensity = EditorGUILayout.FloatField(reverbFilterDensity, GUILayout.Width(25));
                        reverbFilterDensity = Mathf.Clamp(reverbFilterDensity, 0f, 100f);
                    }
                }
                EditorGUILayout.EndFadeGroup();
                reverbZone.target = EditorGUILayout.Toggle("ReverbZone", reverbZone.target);
                if (EditorGUILayout.BeginFadeGroup(reverbZone.faded))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("MinDistance", GUILayout.Width(145));
                        reverbZoneMinDistance = EditorGUILayout.FloatField(reverbZoneMinDistance, GUILayout.Width(25));
                        reverbZoneMinDistance = Mathf.Clamp(reverbZoneMinDistance, 0f, 1000000f);
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("MaxDistance", GUILayout.Width(145));
                        reverbZoneMaxDistance = EditorGUILayout.FloatField(reverbZoneMaxDistance, GUILayout.Width(25));
                        reverbZoneMaxDistance = Mathf.Clamp(reverbZoneMinDistance, reverbZoneMaxDistance, 1000000);
                    }
                    reverbZoneRoom = EditorGUILayout.IntSlider("Room", reverbZoneRoom, -10000, 0);
                    reverbZoneRoomHF = EditorGUILayout.IntSlider("Room HF", reverbZoneRoomHF, -10000, 0);
                    reverbZoneRoomLF = EditorGUILayout.IntSlider("Room LF", reverbZoneRoomLF, -10000, 0);
                    reverbZoneDecayTime = EditorGUILayout.Slider("Decay Time", reverbZoneDecayTime, 0.1f, 20f);
                    reverbZoneDecayHFRatio = EditorGUILayout.Slider("Decay HF Ratio", reverbZoneDecayHFRatio, 0.1f, 2f);
                    reverbZoneReflections = EditorGUILayout.IntSlider("Reflections", reverbZoneReflections, -10000, 1000);
                    reverbZoneReflectionsDelay = EditorGUILayout.Slider("Reflections Delay", reverbZoneReflectionsDelay, 0f, 0.3f);
                    reverbZoneReverb = EditorGUILayout.IntSlider("Reverb", reverbZoneReverb, -10000, 2000);
                    reverbZoneReverbDelay = EditorGUILayout.Slider("Reverb Delay", reverbZoneReverbDelay, 0f, 0.1f);
                    reverbZoneHFReference = EditorGUILayout.IntSlider("HF Reference", reverbZoneHFReference, 1000, 20000);
                    reverbZoneLFReference = EditorGUILayout.IntSlider("LF Reference", reverbZoneLFReference, 20, 1000);
                    reverbZoneDiffusion = EditorGUILayout.Slider("Diffusion", reverbZoneDiffusion, 0f, 100f);
                    reverbZoneDensity = EditorGUILayout.Slider("Density", reverbZoneDensity, 0f, 100f);
                }
                EditorGUILayout.EndFadeGroup();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void CreateContainer()
        {
            if (containers.Find(container => container.Name == containerName))
            {
                Debug.LogError("A Container with the same name already exists");
                return;
            }

            RoarContainerSO container = CreateInstance<RoarContainerSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat(folderPath, "/", containerName, "CONTAINER.asset"));
            AssetDatabase.CreateAsset(container, path);

            container.Name = containerName;
            if (bank == null || bank.name == "")
            {
                container.roarClipBank = CreateClipBank();
            }
            else
            {
                container.roarClipBank = bank;
            }
            if (config == null || config.name == "")
            {
                container.roarConfiguration = CreateConfiguration(container.roarClipBank);
            }
            else
            {
                container.roarConfiguration = config;
            }
            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = container;
            ReloadContainers();
        }

        private RoarClipsBankSO CreateClipBank()
        {
            RoarClipsBankSO bank = CreateInstance<RoarClipsBankSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat(folderPath, "/", containerName, "BANK.asset"));
            AssetDatabase.CreateAsset(bank, path);

            bank.audioClips = clips.ToArray();

            EditorUtility.SetDirty(bank);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = bank;

            return bank;
        }

        private RoarConfigurationSO CreateConfiguration(RoarClipsBankSO bank)
        {
            RoarConfigurationSO config = CreateInstance<RoarConfigurationSO>();

            string path = AssetDatabase.GenerateUniqueAssetPath(string.Concat(folderPath, "/", containerName, "CONFIG.asset"));
            AssetDatabase.CreateAsset(config, path);

            ApplySettings(bank, config);

            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = config;

            return config;
        }

        private void ApplySettings(RoarClipsBankSO bank, RoarConfigurationSO config)
        {
            bank.sequenceMode = sequenceMode;
            bank.ClipIndex = clipIndex;

            config.audioMixerGroup = audioMixerGroup;
            config.startTime = startTime;
            config.esclusive = esclusive;
            config.randomStartTime = randomStartTime;
            config.loop = loop;
            config.mute = mute;
            config.bypasseffects = bypasseffects;
            config.bypasslistenereffects = bypasslistenereffects;
            config.bypassreverbzones = bypassreverbzones;
            config.priority = priority;
            config.volume = volume;
            config.fadeInVolume = fadeInVolume;
            config.playFadeTime = playFadeTime;
            config.pauseFadeTime = pauseFadeTime;
            config.resumeFadeTime = resumeFadeTime;
            config.stopFadeTime = stopFadeTime;
            config.delay = delay;
            config.randomMinvolume = randomMinVolume;
            config.randomMaxvolume = randomMaxVolume;
            config.pitch = pitch;
            config.randomMinPitch = randomMaxPitch;
            config.randomMaxPitch = randomMaxPitch;
            config.stereoPan = stereoPan;
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
            config.fadeInParamValueStart = fadeInParamValueStart;
            config.fadeInParamValueEnd = fadeInParamValueEnd;
            config.fadeOutParamValueStart = fadeOutParamValueStart;
            config.fadeOutParamValueEnd = fadeOutParamValueEnd;
            config.bpm = bpm;
            config.beats = beats;
            config.measure = measure;
            config.everyNBeat = everyNBeat;
            config.timedEvent = timedEvent;
            config.repeatTimedEvent = repeatTimedEvent;
            config.timedEventTime = timedEventTime;
            config.playEvent = playEvent;
            config.pauseEvent = pauseEvent;
            config.resumeEvent = resumeEvent;
            config.stopEvent = stopEvent;
            config.finishedEvent = finishedEvent;
            config.chorusFilter = chorus.target;
            config.chorusDryMix = chorusDryMix;
            config.chorusWetMix1 = chorusWetMix1;
            config.chorusWetMix2 = chorusWetMix2;
            config.chorusWetMix3 = chorusWetMix3;
            config.chorusDelay = chorusDelay;
            config.chorusRate = chorusRate;
            config.chorusDepth = chorusDepth;
            config.distortionFilter = distortion.target;
            config.distortionLevel = distortionLevel;
            config.echoFilter = echo.target;
            config.echoDelay = echoDelay;
            config.echoDecayRatio = echoDecayRatio;
            config.echoDryMix = echoDryMix;
            config.echoWetMix = echoWetMix;
            config.hpFilter = highPass.target;
            config.highPassCutoffFrequency = highPassCutoffFrequency;
            config.highPassResonanceQ = highPassResonanceQ;
            config.lpFilter = lowPass.target;
            config.lowPassCutoffFrequency = lowPassCutoffFrequency;
            config.lowPassResonanceQ = lowPassResonanceQ;
            config.reverbFilter = reverbFilter.target;
            config.reverbFilterDryLevel = reverbFilterDryLevel;
            config.reverbFilterRoom = reverbFilterRoom;
            config.reverbFilterRoomHF = reverbFilterRoomHF;
            config.reverbFilterRoomLF = reverbFilterRoomLF;
            config.reverbFilterDecayTime = reverbFilterDecayTime;
            config.reverbFilterDecayHFRatio = reverbFilterDecayHFRatio;
            config.reverbFilterReflectionsLevel = reverbFilterReflectionsLevel;
            config.reverbFilterReflectionsDelay = reverbFilterReflectionsDelay;
            config.reverbFilterReverbLevel = reverbFilterReverbLevel;
            config.reverbFilterReverbDelay = reverbFilterReverbDelay;
            config.reverbFilterHFReference = reverbFilterHFReference;
            config.reverbFilterLFReference = reverbFilterLFReference;
            config.reverbFilterDiffusion = reverbFilterDiffusion;
            config.reverbFilterDensity = reverbFilterDensity;
            config.reverbZone = reverbZone.target;
            config.reverbZoneMinDistance = reverbZoneMinDistance;
            config.reverbZoneMaxDistance = reverbZoneMaxDistance;
            config.reverbZoneRoom = reverbZoneRoom;
            config.reverbZoneRoomHF = reverbZoneRoomHF;
            config.reverbZoneRoomLF = reverbZoneRoomLF;
            config.reverbZoneDecayTime = reverbZoneDecayTime;
            config.reverbZoneDecayHFRatio = reverbZoneDecayHFRatio;
            config.reverbZoneReflections = reverbZoneReflections;
            config.reverbZoneReflectionsDelay = reverbZoneReflectionsDelay;
            config.reverbZoneReverb = reverbZoneReverb;
            config.reverbZoneReverbDelay = reverbZoneReverbDelay;
            config.reverbZoneHFReference = reverbZoneHFReference;
            config.reverbZoneLFReference = reverbZoneLFReference;
            config.reverbZoneDiffusion = reverbZoneDiffusion;
            config.reverbZoneDensity = reverbZoneDensity;
        }

        private void AddClipField()
        {
            clipsNumber++;
            AudioClip clip = null;
            clips.Add(clip);
        }

        private void RemoveClipField()
        {
            if (clipsNumber > 0)
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
            foreach (var asset in AssetDatabase.FindAssets("t:RoaRContainerSO"))
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                containers.Add((RoarContainerSO)AssetDatabase.LoadMainAssetAtPath(path));
                containersName.Add(AssetDatabase.LoadMainAssetAtPath(path).name);
            }
            if (containerIndex == 0)
            {
                DefaultContainer();
                containerOldIndex = containerIndex;
            }
            else
            {
                GetSettingsFromContainer();
                containerOldIndex = containerIndex;
            }
        }

        private void DefaultContainer()
        {
            DefaultBank();
            DefaultConfiguration();
        }

        private void DefaultBank()
        {
            clips.Clear();
            clipsNumber = 0;
            clipIndex = 0;
        }

        private void DefaultConfiguration()
        {
            audioMixerGroup = null;
            priority = PriorityLevel.Standard;
            sequenceMode = AudioSequenceMode.Sequential;
            startTime = 0f;
            esclusive = false;
            randomStartTime = false;
            loop = false;
            mute = false;
            volume = 1f;
            fadeInVolume = 0f;
            playFadeTime = 0f;
            pauseFadeTime = 0f;
            resumeFadeTime = 0f;
            stopFadeTime = 0f;
            delay = 0f;
            randomMinVolume = 0f;
            randomMaxVolume = 0f;
            pitch = 1f;
            randomMinPitch = 0f;
            randomMaxPitch = 0f;
            stereoPan = 0f;
            reverbZoneMix = 1f;
            is3D = false;
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
            isOnGoing = false;
            minTime = 0f;
            maxTime = 0f;
            minRandomXYZ = 0f;
            maxRandomXYZ = 0f;
            measureEvent = false;
            hasMeasureEvent = false;
            hasTimedEvent = false;
            repeatTimedEvent = false;
            timedEventTime = 0f;
            playEvent = false;
            pauseEvent = false;
            resumeEvent = false;
            stopEvent = false;
            finishedEvent = false;
            bpm = 120;
            beats = 4;
            measure = 4;
            everyNBeat = 1;
            isInACrossFade = false;
            fadeInParamValueStart = 0f;
            fadeInParamValueEnd = 0f;
            fadeOutParamValueStart = 0f;
            fadeOutParamValueEnd = 0f;
            addEffect = false;
            chorus = new AnimBool(false);
            distortion = new AnimBool(false);
            echo = new AnimBool(false);
            highPass = new AnimBool(false);
            lowPass = new AnimBool(false);
            reverbFilter = new AnimBool(false);
            reverbZone = new AnimBool(false);
        }

        private void SaveContainerSettings()
        {
            if (containerName == "" || containerIndex == 0)
            {
                Debug.LogError("You need to create a container before you can save it");
                return;
            }
            RoarContainerSO container = containers[containerIndex - 1];
            container.Name = containerName;
            if (config != null)
            {
                container.roarConfiguration = config;
            }
            else
            {
                container.roarConfiguration = null;
            }
            if (bank != null)
            {
                container.roarClipBank = bank;
                container.roarClipBank.audioClips = clips.ToArray();
            }
            else
            {
                container.roarClipBank = null;
            }
            if (config != null && bank != null)
            {
                ApplySettings(container.roarClipBank, container.roarConfiguration);
            }
            EditorUtility.SetDirty(bank);
            EditorUtility.SetDirty(config);
            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
        }

        private void GetSettingsFromContainer()
        {
            RoarContainerSO container = containers[containerIndex - 1];
            if (container.roarClipBank != null)
            {
                bank = container.roarClipBank;
            }
            else
            {
                DefaultBank();
            }
            if (container.roarConfiguration != null)
            {
                config = container.roarConfiguration;
            }
            else
            {
                DefaultConfiguration();
            }
            containerName = container.Name;
            GetSettingsFromBank();
            GetSettingsFromConfiguration();
        }

        private void GetSettingsFromBank()
        {
            if (bank != null)
            {
                clipsNumber = bank.audioClips.Length;
                for (int i = 0; i < clipsNumber; i++)
                {
                    if (clips.Count < bank.audioClips.Length)
                    {
                        AudioClip clip = null;
                        clips.Add(clip);
                    }
                    clips[i] = bank.audioClips[i];
                }
                sequenceMode = bank.sequenceMode;
                clipIndex = bank.ClipIndex;
            }
        }

        private void GetSettingsFromConfiguration()
        {
            if (config != null)
            {
                audioMixerGroup = config.audioMixerGroup;
                priority = config.priority;
                startTime = config.startTime;
                esclusive = config.esclusive;
                randomStartTime = config.randomStartTime;
                loop = config.loop;
                mute = config.mute;
                volume = config.volume;
                fadeInVolume = config.fadeInVolume;
                playFadeTime = config.playFadeTime;
                pauseFadeTime = config.pauseFadeTime;
                resumeFadeTime = config.resumeFadeTime;
                stopFadeTime = config.stopFadeTime;
                delay = config.delay;
                randomMinVolume = config.randomMinvolume;
                randomMaxVolume = config.randomMaxvolume;
                pitch = config.pitch;
                randomMinPitch = config.randomMinPitch;
                randomMaxPitch = config.randomMaxPitch;
                stereoPan = config.stereoPan;
                reverbZoneMix = config.reverbZoneMix;
                spatialBlend = config.spatialBlend;
                rolloffMode = config.rolloffMode;
                minDistance = config.minDistance;
                maxDistance = config.maxDistance;
                spread = config.spread;
                dopplerLevel = config.dopplerLevel;
                bypasseffects = config.bypasseffects;
                bypasslistenereffects = config.bypasslistenereffects;
                bypassreverbzones = config.bypassreverbzones;
                ignorelistenervolume = config.ignorelistenervolume;
                ignorelistenerpause = config.ignorelistenerpause;
                onGoing = config.onGoing;
                minTime = config.minTime;
                maxTime = config.maxTime;
                minRandomXYZ = config.minRandomXYZ;
                maxRandomXYZ = config.maxRandomXYZ;
                measureEvent = config.measureEvent;
                bpm = config.bpm;
                beats = config.beats;
                measure = config.measure;
                everyNBeat = config.everyNBeat;
                timedEvent = config.timedEvent;
                repeatTimedEvent = config.repeatTimedEvent;
                timedEventTime = config.timedEventTime;
                playEvent = config.playEvent;
                pauseEvent = config.pauseEvent;
                resumeEvent = config.resumeEvent;
                stopEvent = config.stopEvent;
                finishedEvent = config.finishedEvent;
                fadeInParamValueStart = config.fadeInParamValueStart;
                fadeInParamValueEnd = config.fadeInParamValueEnd;
                fadeOutParamValueStart = config.fadeOutParamValueStart;
                fadeOutParamValueEnd = config.fadeOutParamValueEnd;
                chorus.target = config.chorusFilter;
                chorusDryMix = config.chorusDryMix;
                chorusWetMix1 = config.chorusWetMix1;
                chorusWetMix2 = config.chorusWetMix2;
                chorusWetMix3 = config.chorusWetMix3;
                chorusDelay = config.chorusDelay;
                chorusRate = config.chorusRate;
                chorusDepth = config.chorusDepth;
                chorus.target = config.chorusFilter;
                distortion.target = config.distortionFilter;
                distortionLevel = config.distortionLevel;
                echo.target = config.echoFilter;
                echoDelay = config.echoDelay;
                echoDecayRatio = config.echoDecayRatio;
                echoDryMix = config.echoDryMix;
                echoWetMix = config.echoWetMix;
                highPass.target = config.hpFilter;
                highPassCutoffFrequency = config.highPassCutoffFrequency;
                highPassResonanceQ = config.highPassResonanceQ;
                lowPass.target = config.lpFilter;
                lowPassCutoffFrequency = config.lowPassCutoffFrequency;
                lowPassResonanceQ = config.lowPassResonanceQ;
                reverbFilter.target = config.reverbFilter;
                reverbFilterDryLevel = config.reverbFilterDryLevel;
                reverbFilterRoom = config.reverbFilterRoom;
                reverbFilterRoomHF = config.reverbFilterRoomHF;
                reverbFilterRoomLF = config.reverbFilterRoomLF;
                reverbFilterDecayTime = config.reverbFilterDecayTime;
                reverbFilterDecayHFRatio = config.reverbFilterDecayHFRatio;
                reverbFilterReflectionsLevel = config.reverbFilterReflectionsLevel;
                reverbFilterReflectionsDelay = config.reverbFilterReflectionsDelay;
                reverbFilterReverbLevel = config.reverbFilterReverbLevel;
                reverbFilterReverbDelay = config.reverbFilterReverbDelay;
                reverbFilterHFReference = config.reverbFilterHFReference;
                reverbFilterLFReference = config.reverbFilterLFReference;
                reverbFilterDiffusion = config.reverbFilterDiffusion;
                reverbFilterDensity = config.reverbFilterDensity;
                reverbZone.target = config.reverbZone;
                reverbZoneMinDistance = config.reverbZoneMinDistance;
                reverbZoneMaxDistance = config.reverbZoneMaxDistance;
                reverbZoneRoom = config.reverbZoneRoom;
                reverbZoneRoomHF = config.reverbZoneRoomHF;
                reverbZoneRoomLF = config.reverbZoneRoomLF;
                reverbZoneDecayTime = config.reverbZoneDecayTime;
                reverbZoneDecayHFRatio = config.reverbZoneDecayHFRatio;
                reverbZoneReflections = config.reverbZoneReflections;
                reverbZoneReflectionsDelay = config.reverbZoneReflectionsDelay;
                reverbZoneReverb = config.reverbZoneReverb;
                reverbZoneReverbDelay = config.reverbZoneReverbDelay;
                reverbZoneHFReference = config.reverbZoneHFReference;
                reverbZoneLFReference = config.reverbZoneLFReference;
                reverbZoneDiffusion = config.reverbZoneDiffusion;
                reverbZoneDensity = config.reverbZoneDensity;
            }
        }
   
        private void UpdateEmitterEditorSettings()
        {
            if (emitterEditor != null)
            {
                ApplySettings(bankEditor, configEditor);
                if (configEditor.playFadeTime > 0 || configEditor.resumeFadeTime > 0 || configEditor.pauseFadeTime > 0 || configEditor.stopFadeTime > 0)
                {
                    volume = emitterEditor.GetComponent<AudioSource>().volume;
                }
            }
        }

        private void PlayInEditor()
        {
            if (clips.Count > 0)
            {
                canChangeContainer = false;
                emitterEditor = new GameObject();
                emitterEditor.name = "EmitterEditor";
                RoarEmitterEditor emitterComponent = emitterEditor.AddComponent<RoarEmitterEditor>();
                containerEditor = CreateInstance<RoarContainerSO>();
                bankEditor = CreateInstance<RoarClipsBankSO>();
                configEditor = CreateInstance<RoarConfigurationSO>();
                bankEditor.audioClips = clips.ToArray();
                ApplySettings(bankEditor, configEditor);
                containerEditor.roarClipBank = bankEditor;
                containerEditor.roarConfiguration = configEditor;
                emitterComponent.SetContainer(containerEditor);
                emitterComponent.Play();
            }
        }

        private void StopInEditor()
        {
            RoarEmitterEditor emitterEditor = FindObjectOfType<RoarEmitterEditor>();
            if (emitterEditor != null)
            {
                RoarEmitterEditor emitterComponent = emitterEditor.GetComponent<RoarEmitterEditor>();
                emitterComponent.Stop();
                canChangeContainer = true;
            }
        }

        private void PauseInEditor()
        {
            if (emitterEditor != null)
            {
                RoarEmitterEditor emitterComponent = emitterEditor.GetComponent<RoarEmitterEditor>();
                emitterComponent.Pause();
            }
        }

        private void ResumeInEditor()
        {
            if (emitterEditor != null)
            {
                RoarEmitterEditor emitterComponent = emitterEditor.GetComponent<RoarEmitterEditor>();
                emitterComponent.Resume();
            }
        }

        private void Update()
        {
            UpdateEmitterEditorSettings();
        }
        #endregion
    }
}
