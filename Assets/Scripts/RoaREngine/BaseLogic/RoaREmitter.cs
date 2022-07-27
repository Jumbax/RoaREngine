using System.Collections;
using UnityEngine;

namespace RoaREngine
{
    public class RoaREmitter : MonoBehaviour
    {
        private Transform initialParent;
        private AudioSource audioSource;
        private RoaRContainer container;
        private bool paused;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            initialParent = transform.parent;
        }

        public void SetContainer(RoaRContainer container)
        {
            this.container = container;
            audioSource.clip = this.container.Clip;
            this.container.SetConfiguration(audioSource);
        }

        public bool CheckForContainerName(string containerName)
        {
            return container.Name == containerName;
        }

        public void Play(float fadeTime, float volume, float fadeInVolume,bool randomStartTime, float startTime, Transform parent, float minRandomXYZ, float maxRandomXYZ, float delay)
        {
            StartCoroutine(PlayCoroutine(fadeTime, volume, fadeInVolume,randomStartTime, startTime, parent, minRandomXYZ, maxRandomXYZ, delay));
        }

        public IEnumerator PlayCoroutine(float fadeTime, float volume, float fadeInVolume, bool randomStartTime, float startTime, Transform parent, float minRandomXYZ, float maxRandomXYZ, float delay)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            if (audioSource.clip == null)
            {
                yield return null;
            }
            if (volume <= 0f)
            {
                volume = container.roarConfiguration.volume;
            }
            if (fadeInVolume <= 0f)
            {
                fadeInVolume = container.roarConfiguration.fadeInVolume;
            }
            if (fadeTime <= 0)
            {
                fadeTime = container.roarConfiguration.fadeInTime;
            }
            if (fadeTime > 0)
            {
                Fade(fadeTime, fadeInVolume);
            }
            if (!randomStartTime)
            {
                randomStartTime = container.roarConfiguration.randomStartTime;
            }
            if (randomStartTime)
            {
                audioSource.time = Random.Range(0f, audioSource.clip.length);
            }
            if (startTime <= 0)
            {
                startTime = container.roarConfiguration.startTime;
            }
            if (startTime > 0 && !randomStartTime)
            {
                startTime = Mathf.Clamp(startTime, 0f, audioSource.clip.length - 0.01f);
                audioSource.time = startTime;
            }
            if (parent == null)
            {
                parent = container.roarConfiguration.parent;
            }
            if (parent != null)
            {
                SetParent(parent);
            }
            if (minRandomXYZ == 0 || maxRandomXYZ == 0)
            {
                minRandomXYZ = container.roarConfiguration.minRandomXYZ;
                maxRandomXYZ = container.roarConfiguration.maxRandomXYZ;
            }
            if (minRandomXYZ != 0 || maxRandomXYZ != 0)
            {
                GenerateRandomPosition(minRandomXYZ, maxRandomXYZ);
            }
            if (container.roarConfiguration.measureEvent)
            {
                StartCoroutine(MeasureEventCoroutine());
            }
            if (container.roarConfiguration.markerEvent)
            {
                StartCoroutine(MarkerEventCoroutine());
            }
            if (container.roarConfiguration.onGoing)
            {
                StartCoroutine(PlayOnGoing());
            }
            if (!audioSource.loop && !container.roarConfiguration.onGoing)
            {
                StartCoroutine(AudioClipFinishPlayingCoroutine());
            }
            AddEffect();
            audioSource.Play();
        }

        public void Stop(float fadeTime)
        {
            if (fadeTime <= 0)
            {
                fadeTime = container.roarConfiguration.fadeOutTime;
            }
            if (fadeTime <= 0)
            {
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
                ResetParent();
            }
            else
            {
                Fade(fadeTime, 0f, false, true);
            }
        }

        public bool IsPlaying() => audioSource.isPlaying;

        public bool IsInPause() => paused;

        public void Pause(float fadeTime)
        {
            if (fadeTime <= 0)
            {
                StopAllCoroutines();
                paused = true;
                audioSource.Pause();
            }
            else
            {
                Fade(fadeTime, 0f, false, false, true);
            }
        }

        public void Resume(float fadeTime, float finalVolume)
        {
            if (fadeTime <= 0)
            {
                paused = false;
                audioSource.UnPause();
                if (!audioSource.loop || !container.roarConfiguration.onGoing)
                {
                    StartCoroutine(AudioClipFinishPlayingCoroutine());
                }
                if (container.roarConfiguration.measureEvent)
                {
                    StartCoroutine(SyncMeasureEvent());
                }
                if (container.roarConfiguration.onGoing)
                {
                    StartCoroutine(PlayOnGoing());
                }
            }
            else
            {
                if (finalVolume <= 0)
                {
                    finalVolume = container.roarConfiguration.volume;
                }
                Fade(fadeTime, container.roarConfiguration.volume, true);
            }
        }

        public AudioSource GetAudioSource() => audioSource;

        public RoaRContainer GetContainer() => container;

        public void Fade(float fadeTime, float volume, bool resume = false, bool stop = false, bool pause = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCoroutine(fadeTime, volume, resume, stop, pause));
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
            transform.position = parent.position;
            transform.position = Vector3.zero;
        }

        public void ResetParent() => transform.parent = initialParent;

        public void GenerateRandomPosition(float minRandomXYZ, float maxRandomXYZ)
        {
            float posX = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posY = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posZ = Random.Range(minRandomXYZ, maxRandomXYZ);
            transform.position = new Vector3(posX, posY, posZ);
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

        private IEnumerator FadeCoroutine(float fadeTime, float volume, bool resume = false, bool stop = false, bool pause = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            if (resume)
            {
                paused = false;
                audioSource.UnPause();
                if (!audioSource.loop || !container.roarConfiguration.onGoing)
                {
                    StartCoroutine(AudioClipFinishPlayingCoroutine());
                }
                if (container.roarConfiguration.measureEvent)
                {
                    StartCoroutine(SyncMeasureEvent());
                }
                if (container.roarConfiguration.onGoing)
                {
                    StartCoroutine(PlayOnGoing());
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
                audioSource.gameObject.SetActive(false);
                ResetParent();
            }

            if (pause)
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
            audioSource.gameObject.SetActive(false);
            ResetParent();
        }

        private IEnumerator PlayOnGoing()
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
                StartCoroutine(PlayOnGoing());
            }
        }

        private IEnumerator MeasureEventCoroutine()
        {
            float seconds = (float)TrackInfo.GetTrackBarLength(container.roarConfiguration.bpm, container.roarConfiguration.tempo);
            float barNumber = seconds * container.roarConfiguration.everyNBar;
            yield return new WaitForSeconds(barNumber);
            container.MeasureEvent?.Invoke();
            StartCoroutine(MeasureEventCoroutine());
        }

        private IEnumerator MarkerEventCoroutine()
        {
            float eventTime = container.roarConfiguration.markerEventTime;
            yield return new WaitForSeconds(eventTime);
            container.MarkerEvent?.Invoke();
            if (container.roarConfiguration.repeat)
            {
                StartCoroutine(MarkerEventCoroutine());
            }
        }

        private IEnumerator SyncMeasureEvent()
        {
            yield return new WaitForSeconds((float)TrackInfo.GetTimeBeforeNextBar(audioSource, container.roarConfiguration.bpm, container.roarConfiguration.tempo));
            StartCoroutine(MeasureEventCoroutine());
        }
    }
}
