using System;
using UnityEngine;
using UnityEngine.Audio;


namespace RoaREngine
{
    public enum EffectType
    {
        Chorus,
        Distortion,
        Echo,
        HP,
        LP,
        ReverbFilter,
        ReverbZone,
    }

    public enum PriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256
    }

    [CreateAssetMenu(fileName = "RoaRConfiguration", menuName = "RoaREngine/RoaRConfiguration")]
    public class RoaRConfigurationSO : ScriptableObject
    {
        public Transform parent = null;

        public AudioMixerGroup audioMixerGroup = null;
        public PriorityLevel priority = PriorityLevel.Standard;

        [Header("Properties")]
        public bool loop = false;
        public bool mute = false;
        public float startTime = 0f;
        public bool randomStartTime = false;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0f, 1f)] public float fadeInVolume = 1f;
        public float playFadeTime = 0f;
        public float pauseFadeTime = 0f;
        public float resumeFadeTime = 0f;
        public float stopFadeTime = 0f;
        public float delay = 0f;
        [Range(0f, 1f)] public float randomMinvolume = 0f;
        [Range(0f, 1f)] public float randomMaxvolume = 0f;
        [Range(-3f, 3f)] public float pitch = 1f;
        [Range(-3f, 3f)] public float randomMinPitch = 0f;
        [Range(-3f, 3f)] public float randomMaxPitch = 0f;
        [Range(0f, 1f)] public float panStereo = 0f;
        [Range(0f, 1f)] public float reverbZoneMix = 1f;

        [Header("Spatialisation")]
        [Range(0f, 1f)] public float spatialBlend = 0f;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [Range(0.01f, 5f)] public float minDistance = 0.1f;
        [Range(5f, 100f)] public float maxDistance = 50f;
        [Range(0, 360)] public int spread = 0;
        [Range(0f, 5f)] public float dopplerLevel = 0f;

        [Header("Ignores")]
        public bool bypasseffects = false;
        public bool bypasslistenereffects = false;
        public bool bypassreverbzones = false;
        public bool ignorelistenervolume = false;
        public bool ignorelistenerpause = false;

        [Header("On Going")]
        public bool onGoing = false;
        public float minTime = 0f;
        public float maxTime = 0f;

        [Header("Random Position")]
        public float minRandomXYZ = 0f;
        public float maxRandomXYZ = 0f;
        
        [Header("Controls Events")]
        public bool playEvent = false;
        public bool pauseEvent = false;
        public bool stopEvent = false;
        public bool resumeEvent = false;
        public bool finishedEvent = false;

        [Header("Measure Event")]
        public bool measureEvent = false;
        public int bpm = 120;
        public int tempo = 4;
        public int everyNBar = 1;

        [Header("Marker Event")]
        public bool markerEvent = false;
        public bool repeat = false;
        public float markerEventTime = 0f;


        [Header("Effects")]
        [Header("Chorus Filter")]
        public bool chorusFilter = false;
        public float chorusDryMix = 0.5f;
        public float chorusWetMix1 = 0.5f;
        public float chorusWetMix2 = 0.5f;
        public float chorusWetMix3 = 0.5f;
        public float chorusDelay = 40f;
        public float chorusRate = 0.8f;
        public float chorusDepth = 0.03f;
        [Header("Distortion Filter")]
        public bool distortionFilter = false;
        public float distortionLevel = 0.5f;
        [Header("Echo Filter")]
        public bool echoFilter = false;
        public int echoDelay = 500;
        public float echoDecayRatio = 0.5f;
        public float echoDryMix = 1f;
        public float echoWetMix = 1f;
        [Header("High Pass Filter")]
        public bool hpFilter = false;
        public int highPassCutoffFrequency = 5000;
        public float highPassResonanceQ = 1;
        [Header("Low Pass Filter")]
        public bool lpFilter = false;
        public int lowPassCutoffFrequency = 5000;
        public float lowPassResonanceQ = 1;
        [Header("Reverb Filter")]
        public bool reverbFilter = false;
        public int reverbFilterDryLevel = 0;
        public int reverbFilterRoom = 0;
        public int reverbFilterRoomHF = 0;
        public int reverbFilterRoomLF = 0;
        public float reverbFilterDecayTime = 1f;
        public float reverbFilterDecayHFRatio = 0.5f;
        public int reverbFilterReflectionsLevel = -10000;
        public float reverbFilterReflectionsDelay = 0f;
        public int reverbFilterReverbLevel = 0;
        public float reverbFilterReverbDelay = 0.04f;
        public int reverbFilterHFReference = 5000;
        public int reverbFilterLFReference = 250;
        public float reverbFilterDiffusion = 100f;
        public float reverbFilterDensity = 100f;
        [Header("Reverb Zone")]
        public bool reverbZone = false;
        public float reverbZoneMinDistance = 10f;
        public float reverbZoneMaxDistance = 15f;
        public int reverbZoneRoom = -1000;
        public int reverbZoneRoomHF = -100;
        public int reverbZoneRoomLF = 0;
        public float reverbZoneDecayTime = 1.49f;
        public float reverbZoneDecayHFRatio = 0.83f;
        public int reverbZoneReflections = -2602;
        public float reverbZoneReflectionsDelay = 0.007f;
        public int reverbZoneReverb = 200;
        public float reverbZoneReverbDelay = 0.011f;
        public int reverbZoneHFReference = 5000;
        public int reverbZoneLFReference = 250;
        public float reverbZoneDiffusion = 100f;
        public float reverbZoneDensity = 100f;
        [Header("Cross Fade Input")]
        public float fadeInParamValueStart = 0f;
        public float fadeInParamValueEnd = 0f;
        public float fadeOutParamValueStart = 0f;
        public float fadeOutParamValueEnd = 0f;

        public void ApplyTo(AudioSource audioSource, RoaREmitter emitter)
        {
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = this.audioMixerGroup;
            audioSource.loop = this.loop;
            audioSource.mute = this.mute;
            audioSource.bypassEffects = this.bypasseffects;
            audioSource.bypassListenerEffects = this.bypasslistenereffects;
            audioSource.bypassReverbZones = this.bypassreverbzones;
            audioSource.priority = (int)this.priority;
            if (randomMinvolume != 0 || randomMaxvolume != 0)
            {
                audioSource.volume = UnityEngine.Random.Range(randomMinvolume, randomMaxvolume);
            }
            else
            {
                audioSource.volume = this.volume;
            }
            if (randomMinPitch != 0 || randomMaxPitch != 0)
            {
                audioSource.pitch = UnityEngine.Random.Range(randomMinPitch, randomMaxPitch);
            }
            else
            {
                audioSource.pitch = this.pitch;
            }
            audioSource.panStereo = this.panStereo;
            audioSource.spatialBlend = this.spatialBlend;
            audioSource.reverbZoneMix = this.reverbZoneMix;
            audioSource.dopplerLevel = this.dopplerLevel;
            audioSource.spread = this.spread;
            audioSource.rolloffMode = this.rolloffMode;
            audioSource.minDistance = this.minDistance;
            audioSource.maxDistance = this.maxDistance;
            audioSource.ignoreListenerVolume = this.ignorelistenervolume;
            audioSource.ignoreListenerPause = this.ignorelistenerpause;

            //emitter.GetContainer().roarConfiguration.fadeInVolume = this.fadeInVolume;
            //emitter.GetContainer().roarConfiguration.fadeInTime = this.fadeInTime;
            //emitter.GetContainer().roarConfiguration.randomStartTime = this.randomStartTime;
            //emitter.GetContainer().roarConfiguration.startTime = this.startTime;
            //emitter.GetContainer().roarConfiguration.parent = this.parent;
            //emitter.GetContainer().roarConfiguration.minRandomXYZ = this.minRandomXYZ;
            //emitter.GetContainer().roarConfiguration.maxRandomXYZ = this.maxRandomXYZ;
        }
    }
}
