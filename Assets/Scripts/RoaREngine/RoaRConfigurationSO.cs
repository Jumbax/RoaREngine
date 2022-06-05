using System;
using UnityEngine;
using UnityEngine.Audio;


namespace RoaREngine
{
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
        [Range(0f, 1f)] public float finalVolume = 1f;
        public float fadeInTime = 0f;
        public float fadeOutTime = 0f;
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

        public bool onGoing = false;
        public float minTime = 0f;
        public float maxTime = 0f;

        public float minRandomXYZ = 0f;
        public float maxRandomXYZ = 0f;

        public bool measureEvent = false;
        public int bpm = 120;
        public int tempo = 4;
        public int everyNBar = 1;
        public void ApplyTo(AudioSource audioSource, GameObject roarEmitter)
        {
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


            //TEST
            //if (randomStartTime)
            //{
            //    startTime = UnityEngine.Random.Range(0f, audioSource.clip.length);
            //    audioSource.time = startTime;
            //}
            //if (startTime > 0 && !randomStartTime)
            //{
            //    startTime = Mathf.Clamp(startTime, 0f, audioSource.clip.length - 0.01f);
            //    audioSource.time = startTime;
            //}


            //if (fadeInTime > 0)
            //{
            //    roarEmitter.GetComponent<RoaREmitter>().Fade(fadeInTime, finalVolume);
            //}
            //if (parent != null)
            //{
            //    roarEmitter.GetComponent<RoaREmitter>().SetParent(parent);
            //}
            //if (minRandomXYZ != 0 || maxRandomXYZ != 0)
            //{
            //    roarEmitter.GetComponent<RoaREmitter>().GenerateRandomPosition(minRandomXYZ, maxRandomXYZ);
            //}
        }
    }
}
