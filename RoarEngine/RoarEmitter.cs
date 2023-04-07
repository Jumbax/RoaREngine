using System.Collections;
using UnityEngine;

namespace RoaREngine
{
    public class RoarEmitter : MonoBehaviour
    {
        #region var
        private Transform initialParent;
        private AudioSource audioSource;
        private RoarContainerSO container;
        private bool paused;
        private float delay = 0f;
        private int containerNameHash;
        #endregion

        #region private functions
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            initialParent = transform.parent;
        }

        private void GenerateRandomPosition(float minRandomXYZ, float maxRandomXYZ)
        {
            float posX = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posY = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posZ = Random.Range(minRandomXYZ, maxRandomXYZ);
            transform.position = new Vector3(posX, posY, posZ);
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

        private IEnumerator PlayCoroutine()
        {
            if (container.roarConfiguration.delay > 0)
            {
                if (Time.time < container.roarConfiguration.delay)
                {
                    container.roarConfiguration.delay -= Time.time;
                }
                while (delay < container.roarConfiguration.delay)
                {
                    delay += Time.deltaTime;
                    yield return null;
                }
                delay = 0f;
            }
            if (container.roarConfiguration.playFadeTime > 0)
            {
                Fade(container.roarConfiguration.playFadeTime, container.roarConfiguration.fadeInVolume);
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
            if (container.roarConfiguration.measureEvent)
            {
                StartCoroutine(MeasureEventCoroutine());
            }
            if (container.roarConfiguration.timedEvent)
            {
                StartCoroutine(TimedEventCoroutine());
            }
            if (container.roarConfiguration.onGoing)
            {
                StartCoroutine(OnGoingCoroutine());
            }
            if (container.roarConfiguration.playEvent)
            {
                container.OnPlayEvent?.Invoke();
            }
            if ((!audioSource.loop && !container.roarConfiguration.onGoing) || container.roarConfiguration.finishedEvent)
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

        private IEnumerator FadeCoroutine(float fadeTime, float volume, bool resume = false, bool stop = false, bool paused = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            if (!resume && !stop && !paused)
            {

                if (!audioSource.loop && !container.roarConfiguration.onGoing)
                {
                    StartCoroutine(AudioClipFinishPlayingCoroutine());
                }
                if (container.roarConfiguration.measureEvent)
                {
                    StartCoroutine(SyncMeasureEvent());
                }
                if (container.roarConfiguration.timedEvent)
                {
                    StartCoroutine(TimedEventCoroutine());
                }
                if (container.roarConfiguration.onGoing)
                {
                    StartCoroutine(OnGoingCoroutine());
                }
            }

            if (resume)
            {
                audioSource.UnPause();
                if (!audioSource.loop && !container.roarConfiguration.onGoing)
                {
                    StartCoroutine(AudioClipFinishPlayingCoroutine());
                }
                if (container.roarConfiguration.measureEvent)
                {
                    StartCoroutine(SyncMeasureEvent());
                }
                if (container.roarConfiguration.timedEvent)
                {
                    StartCoroutine(TimedEventCoroutine());
                }
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
                Dispose();
            }

            if (paused)
            {
                audioSource.Pause();
            }
        }

        private IEnumerator AudioClipFinishPlayingCoroutine()
        {
            float clipLengthRemaining = audioSource.clip.length - audioSource.time;
            yield return new WaitForSeconds(clipLengthRemaining);
            if (container.roarConfiguration.finishedEvent && !container.roarConfiguration.onGoing)
            {
                container.OnFinishedEvent?.Invoke();
            }
            if (!audioSource.loop && !container.roarConfiguration.onGoing)
            {
                Dispose();
            }
        }

        private IEnumerator OnGoingCoroutine()
        {
            if (container.roarConfiguration.onGoing)
            {
                float clipLengthRemaining = audioSource.clip.length - audioSource.time;
                yield return new WaitForSeconds(clipLengthRemaining);
                if (container.roarConfiguration.finishedEvent)
                {
                    container.OnFinishedEvent?.Invoke();
                }
                if (container.roarConfiguration.minTime != 0 || container.roarConfiguration.maxTime != 0)
                {
                    float randomTime = Random.Range(container.roarConfiguration.minTime, container.roarConfiguration.maxTime);
                    yield return new WaitForSeconds(randomTime);
                }
                audioSource.clip = container.Clip;
                audioSource.Play();
                if (container.roarConfiguration.playEvent)
                {
                    container.OnPlayEvent?.Invoke();
                }
                StartCoroutine(OnGoingCoroutine());
            }
        }

        private IEnumerator MeasureEventCoroutine()
        {
            if(container.roarConfiguration.measureEvent)
            {
                float seconds = (float)RoarTrackInfo.GetTrackBarLength(container.roarConfiguration.bpm, container.roarConfiguration.beats, container.roarConfiguration.measure);
                float beatNumber = seconds * container.roarConfiguration.everyNBeat;
                yield return new WaitForSeconds(beatNumber);
                container.MeasureEvent?.Invoke();
                StartCoroutine(MeasureEventCoroutine());
            }
        }

        private IEnumerator TimedEventCoroutine()
        {
            if(container.roarConfiguration.timedEvent)
            {
                float timedEventTime = container.roarConfiguration.timedEventTime;
                yield return new WaitForSeconds(timedEventTime);
                container.TimedEvent?.Invoke();
                if (container.roarConfiguration.repeatTimedEvent)
                {
                    StartCoroutine(TimedEventCoroutine());
                }
            }
        }

        private IEnumerator SyncMeasureEvent()
        {
            yield return new WaitForSeconds((float)RoarTrackInfo.GetTimeBeforeNextBar(audioSource, container.roarConfiguration.bpm, container.roarConfiguration.beats, container.roarConfiguration.measure));
            StartCoroutine(MeasureEventCoroutine());
        }

        private void Dispose()
        {
            ResetParent();
            audioSource.Stop();
            audioSource.clip = default;
            containerNameHash = default;
            audioSource.gameObject.SetActive(false);
        }
        #endregion

        #region public functions
        public void Play()
        {
            if (audioSource.clip == null)
            {
                return;
            }
            StartCoroutine(PlayCoroutine());
        }

        public void Pause()
        {
            if (!paused)
            {
                paused = true;
                if (container.roarConfiguration.pauseEvent)
                {
                    container.OnPauseEvent?.Invoke();
                }
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
        }

        public void Resume()
        {
            if (paused)
            {
                paused = false;
                if (container.roarConfiguration.resumeEvent)
                {
                    container.OnResumeEvent?.Invoke();
                }
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
                    if (container.roarConfiguration.measureEvent)
                    {
                        StartCoroutine(SyncMeasureEvent());
                    }
                    if (container.roarConfiguration.timedEvent)
                    {
                        StartCoroutine(TimedEventCoroutine());
                    }
                    if (container.roarConfiguration.onGoing)
                    {
                        StartCoroutine(OnGoingCoroutine());
                    }
                }
                else
                {
                    Fade(container.roarConfiguration.resumeFadeTime, container.roarConfiguration.fadeInVolume, true);
                }
            }
        }

        public void Stop()
        {
            if (container.roarConfiguration.stopEvent)
            {
                container.OnStopEvent?.Invoke();
            }
            if (container.roarConfiguration.stopFadeTime <= 0)
            {
                Dispose();
            }
            else
            {
                Fade(container.roarConfiguration.stopFadeTime, 0f, false, true);
            }
        }

        public bool IsPlaying() => audioSource.isPlaying;

        public bool IsInPause() => paused;

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
            transform.position = Vector3.zero;
            transform.position = parent.position;
        }

        public void ResetParent() => transform.parent = initialParent;

        public void Fade(float fadeTime, float volume, bool resume = false, bool stop = false, bool paused = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCoroutine(fadeTime, volume, resume, stop, paused));
        }

        public bool CheckForContainerName(string containerName)
        {
            int containerNameHash = containerName.GetHashCode();
            return this.containerNameHash == containerNameHash;
        }

        public AudioSource GetAudioSource() => audioSource;

        public RoarContainerSO GetContainer() => container;

        public void SetContainer(RoarContainerSO container)
        {
            this.container = container;
            audioSource.clip = this.container.Clip;
            this.container.SetConfiguration(audioSource);
            containerNameHash = this.container.Name.GetHashCode();
        }

        public void AddEffect(EffectType type)
        {
            switch (type)
            {
                case EffectType.Chorus:
                    gameObject.AddComponent<AudioChorusFilter>();
                    container.roarConfiguration.chorusFilter = true;
                    break;
                case EffectType.Distortion:
                    gameObject.AddComponent<AudioDistortionFilter>();
                    container.roarConfiguration.distortionFilter = true;
                    break;
                case EffectType.Echo:
                    gameObject.AddComponent<AudioEchoFilter>();
                    container.roarConfiguration.echoFilter = true;
                    break;
                case EffectType.HP:
                    gameObject.AddComponent<AudioHighPassFilter>();
                    container.roarConfiguration.hpFilter = true;
                    break;
                case EffectType.LP:
                    gameObject.AddComponent<AudioLowPassFilter>();
                    container.roarConfiguration.lpFilter = true;
                    break;
                case EffectType.ReverbFilter:
                    gameObject.AddComponent<AudioReverbFilter>();
                    container.roarConfiguration.reverbFilter = true;
                    break;
                case EffectType.ReverbZone:
                    gameObject.AddComponent<AudioReverbZone>();
                    container.roarConfiguration.reverbZone = true;
                    break;
            }
        }
        #endregion
    }
}
