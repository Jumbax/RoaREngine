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
        public AudioMixerGroup audioMixerGroup = null;
        public PriorityLevel priority = PriorityLevel.Standard;

        [Header("Properties")]
        public bool loop = false;
        public bool mute = false;
        [Range(0f, 1f)] public float volume = 1f;
        public float fadeInvolume = 0f;
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
        [Range(0f, 5f)] public float dopplerLevel = 1f;

        [Header("Ignores")]
        public bool bypasseffects = false;
        public bool bypasslistenereffects = false;
        public bool bypassreverbzones = false;
        public bool ignorelistenervolume = false;
        public bool ignorelistenerpause = false;

        public void ApplyTo(AudioSource audioSource)
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
        }
    }
}
