using System.Collections;
using UnityEngine;

namespace RoaREngine
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class RoarEmitterEditor : RoarEmitter
    {
        private AudioSource audioSource;
        private RoarContainerSO container;
        private float delay = 0f;

        #region private
        
        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        private IEnumerator PlayCoroutine()
        {
            if (container.roarConfiguration.delay > 0)
            {
                while (delay < container.roarConfiguration.delay)
                {
                    delay += Time.deltaTime;
                    yield return null;
                }
                delay = 0f;
            }
            if (container.roarConfiguration.randomStartTime)
            {
                audioSource.time = Random.Range(0f, audioSource.clip.length);
            }
            if (container.roarConfiguration.startTime > 0 && !container.roarConfiguration.randomStartTime)
            {
                container.roarConfiguration.startTime = Mathf.Clamp(container.roarConfiguration.startTime, 0f, audioSource.clip.length);
                audioSource.time = container.roarConfiguration.startTime;
            }
            if (container.roarConfiguration.minRandomXYZ != 0 || container.roarConfiguration.maxRandomXYZ != 0)
            {
                GenerateRandomPosition(container.roarConfiguration.minRandomXYZ, container.roarConfiguration.maxRandomXYZ);
            }
            if (container.roarConfiguration.onGoing)
            {
                StartCoroutine(OnGoingCoroutine());
            }
            if (!audioSource.loop && !container.roarConfiguration.onGoing)
            {
                StartCoroutine(AudioClipFinishPlayingCoroutine());
            }
            AddEffect();
            audioSource.Play();
        }
        
        private IEnumerator ResumeCoroutine()
        {
            while (delay < container.roarConfiguration.delay)
            {
                delay += Time.deltaTime;
                yield return null;
            }
            Fade(container.roarConfiguration.playFadeTime, container.roarConfiguration.fadeInVolume, true);
            delay = 0f;
            audioSource.Play();
        }

        private new void Fade(float fadeTime, float volume, bool resume = false, bool stop = false, bool paused = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCoroutine(fadeTime, volume, resume, stop, paused));
        }

        private void AddEffect()
        {
            if (container.roarConfiguration.chorusFilter)
            {
                AudioChorusFilter chorusFilter = gameObject.AddComponent<AudioChorusFilter>();
                chorusFilter.dryMix = container.roarConfiguration.chorusDryMix;
                chorusFilter.wetMix1 = container.roarConfiguration.chorusWetMix1;
                chorusFilter.wetMix2 = container.roarConfiguration.chorusWetMix2;
                chorusFilter.wetMix3 = container.roarConfiguration.chorusWetMix3;
                chorusFilter.delay = container.roarConfiguration.chorusDelay;
                chorusFilter.rate = container.roarConfiguration.chorusRate;
                chorusFilter.depth = container.roarConfiguration.chorusDepth;
            }
            if (container.roarConfiguration.distortionFilter)
            {
                AudioDistortionFilter distortionFilter = gameObject.AddComponent<AudioDistortionFilter>();
                distortionFilter.distortionLevel = container.roarConfiguration.distortionLevel;
            }
            if (container.roarConfiguration.echoFilter)
            {
                AudioEchoFilter echoFilter = gameObject.AddComponent<AudioEchoFilter>();
                echoFilter.delay = container.roarConfiguration.echoDelay;
                echoFilter.decayRatio = container.roarConfiguration.echoDecayRatio;
                echoFilter.dryMix = container.roarConfiguration.echoDryMix;
                echoFilter.wetMix = container.roarConfiguration.echoWetMix;
            }
            if (container.roarConfiguration.hpFilter)
            {
                AudioHighPassFilter hpFilter = gameObject.AddComponent<AudioHighPassFilter>();
                hpFilter.cutoffFrequency = container.roarConfiguration.highPassCutoffFrequency;
                hpFilter.highpassResonanceQ = container.roarConfiguration.highPassResonanceQ;
            }
            if (container.roarConfiguration.lpFilter)
            {
                AudioLowPassFilter lpFilter = gameObject.AddComponent<AudioLowPassFilter>();
                lpFilter.cutoffFrequency = container.roarConfiguration.lowPassCutoffFrequency;
                lpFilter.lowpassResonanceQ = container.roarConfiguration.lowPassResonanceQ;
            }
            if (container.roarConfiguration.reverbFilter)
            {
                AudioReverbFilter reverbFilter = gameObject.AddComponent<AudioReverbFilter>();
                reverbFilter.dryLevel = container.roarConfiguration.reverbFilterDryLevel;
                reverbFilter.room = container.roarConfiguration.reverbFilterRoom;
                reverbFilter.roomHF = container.roarConfiguration.reverbFilterRoomHF;
                reverbFilter.roomLF = container.roarConfiguration.reverbFilterRoomLF;
                reverbFilter.decayTime = container.roarConfiguration.reverbFilterDecayTime;
                reverbFilter.decayHFRatio = container.roarConfiguration.reverbFilterDecayHFRatio;
                reverbFilter.reflectionsLevel = container.roarConfiguration.reverbFilterReflectionsLevel;
                reverbFilter.reflectionsDelay = container.roarConfiguration.reverbFilterReflectionsDelay;
                reverbFilter.reverbLevel = container.roarConfiguration.reverbFilterReverbLevel;
                reverbFilter.reverbDelay = container.roarConfiguration.reverbFilterReverbDelay;
                reverbFilter.hfReference = container.roarConfiguration.reverbFilterHFReference;
                reverbFilter.lfReference = container.roarConfiguration.reverbFilterLFReference;
                reverbFilter.diffusion = container.roarConfiguration.reverbFilterDiffusion;
                reverbFilter.density = container.roarConfiguration.reverbFilterDensity;
            }
            if (container.roarConfiguration.reverbZone)
            {
                AudioReverbZone reverbZone = gameObject.AddComponent<AudioReverbZone>();
                reverbZone.minDistance = container.roarConfiguration.reverbZoneMinDistance;
                reverbZone.maxDistance = container.roarConfiguration.reverbZoneMaxDistance;
                reverbZone.room = container.roarConfiguration.reverbZoneRoom;
                reverbZone.roomHF = container.roarConfiguration.reverbZoneRoomHF;
                reverbZone.roomLF = container.roarConfiguration.reverbZoneRoomLF;
                reverbZone.decayTime = container.roarConfiguration.reverbZoneDecayTime;
                reverbZone.decayHFRatio = container.roarConfiguration.reverbZoneDecayHFRatio;
                reverbZone.reflections = container.roarConfiguration.reverbZoneReflections;
                reverbZone.reflectionsDelay = container.roarConfiguration.reverbZoneReflectionsDelay;
                reverbZone.reverb = container.roarConfiguration.reverbZoneReverb;
                reverbZone.reverbDelay = container.roarConfiguration.reverbZoneReverbDelay;
                reverbZone.HFReference = container.roarConfiguration.reverbZoneHFReference;
                reverbZone.LFReference = container.roarConfiguration.reverbZoneLFReference;
                reverbZone.diffusion = container.roarConfiguration.reverbZoneDiffusion;
                reverbZone.density = container.roarConfiguration.reverbZoneDensity;
            }
        }

        private IEnumerator FadeCoroutine(float fadeTime, float volume, bool resume = false, bool stop = false, bool paused = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            if (resume)
            {
                paused = false;
                audioSource.UnPause();
                if (container.roarConfiguration.onGoing)
                {
                    StartCoroutine(OnGoingCoroutine());
                }
            }

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, volume, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = volume;

            if (stop)
            {
                audioSource.Stop();
                DestroyImmediate(gameObject);
            }

            if (paused)
            {
                StopAllCoroutines();
                paused = true;
                audioSource.Pause();
            }
        }

        private IEnumerator AudioClipFinishPlayingCoroutine()
        {
            float clipLengthRemaining = audioSource.clip.length - audioSource.time;
            yield return new WaitForSeconds(clipLengthRemaining);
            audioSource.Stop();
            DestroyImmediate(gameObject);
        }

        private IEnumerator OnGoingCoroutine()
        {
            if (container.roarConfiguration.onGoing)
            {
                float clipLengthRemaining = audioSource.clip.length - audioSource.time;
                if (container.roarConfiguration.minTime != 0 || container.roarConfiguration.maxTime != 0)
                {
                    float seconds = Random.Range(container.roarConfiguration.minTime, container.roarConfiguration.maxTime);
                    yield return new WaitForSeconds(clipLengthRemaining + seconds);
                    audioSource.clip = container.Clip;
                    audioSource.Play();
                }
                else
                {
                    yield return new WaitForSeconds(clipLengthRemaining);
                    audioSource.clip = container.Clip;
                    audioSource.Play();
                }
                StartCoroutine(OnGoingCoroutine());
            }
        }

        private void Update()
        {
            UnityEditor.EditorApplication.delayCall += UnityEditor.EditorApplication.QueuePlayerLoopUpdate;
            if (!Application.isPlaying)
            {
                container.roarConfiguration.ApplyTo(audioSource);
            }
        }

        #endregion

        #region public

        public new void Play()
        {
            if (audioSource.clip == null)
            {
                return;
            }
            StartCoroutine(PlayCoroutine());
        }

        public new void SetContainer(RoarContainerSO otherContainer)
        {
            container = otherContainer;
            audioSource.clip = container.Clip;
            container.SetConfiguration(audioSource);
        }

        public new void Pause()
        {
            if (container.roarConfiguration.pauseFadeTime <= 0)
            {
                StopAllCoroutines();
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
            else
            {
                Fade(container.roarConfiguration.pauseFadeTime, 0f, false, false, true);
            }
        }

        public new void Resume()
        {
            if (container.roarConfiguration.resumeFadeTime <= 0)
            {
                audioSource.UnPause();
                if (!audioSource.isPlaying)
                {
                    if (delay != 0 && delay == container.roarConfiguration.delay)
                    {
                        audioSource.Play();
                    }
                    else
                    {
                        StartCoroutine(ResumeCoroutine());
                        return;
                    }
                }
                if (!audioSource.loop && !container.roarConfiguration.onGoing)
                {
                    StartCoroutine(AudioClipFinishPlayingCoroutine());
                }
                if (container.roarConfiguration.onGoing)
                {
                    StartCoroutine(OnGoingCoroutine());
                }
            }
            else
            {
                Fade(container.roarConfiguration.resumeFadeTime, container.roarConfiguration.volume, true);
            }
        }

        public new void Stop()
        {
            if (container.roarConfiguration.stopFadeTime <= 0)
            {
                audioSource.Stop();
                DestroyImmediate(gameObject);
            }
            else
            {
                Fade(container.roarConfiguration.stopFadeTime, 0f, false, true);
            }
        }

        public new void SetParent(Transform parent)
        {
            transform.parent = parent;
            transform.position = parent.position;
            transform.position = Vector3.zero;
        }

        public void GenerateRandomPosition(float minRandomXYZ, float maxRandomXYZ)
        {
            float posX = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posY = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posZ = Random.Range(minRandomXYZ, maxRandomXYZ);
            transform.position = new Vector3(posX, posY, posZ);
        }
        
        #endregion
    }
#endif
}
