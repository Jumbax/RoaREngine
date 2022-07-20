using System.Collections;
using UnityEngine;

namespace RoaREngine
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class RoaREmitterEditor : MonoBehaviour
    {
        private AudioSource audioSource;
        private RoaRContainer container;
        private bool paused;
        public float fadeTime;
        public float finalVolume;
        public bool randomStartTime;
        public float startTime;
        public Transform parent;
        public float minRandomXYZ, maxRandomXYZ;

        private void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void SetContainer(RoaRContainer otherContainer)
        {
            container = otherContainer;
            audioSource.clip = container.Clip;
            container.SetConfiguration(audioSource);
        }

        public void Play()
        {
            if (container.roarConfiguration.fadeInTime > 0)
            {
                Fade(container.roarConfiguration.fadeInTime, finalVolume);
            }
            if (randomStartTime)
            {
                audioSource.time = Random.Range(0f, audioSource.clip.length);
            }
            if (startTime > 0 && !randomStartTime)
            {
                startTime = Mathf.Clamp(startTime, 0f, audioSource.clip.length - 0.01f);
                audioSource.time = startTime;
            }
            if (parent != null)
            {
                SetParent(parent);
            }
            if (minRandomXYZ != 0 || maxRandomXYZ != 0)
            {
                GenerateRandomPosition(minRandomXYZ, maxRandomXYZ);
            }
            //if (container.roarConfiguration.measureEvent)
            //{
            //    MeasureEvent();
            //}
            //if (container.roarConfiguration.onGoing)
            //{
            //    StartCoroutine(PlayOnGoing());
            //}
            //AddEffect();
            audioSource.Play();
        }

        public void Stop()
        {
            if (fadeTime <= 0)
            {
                fadeTime = container.roarConfiguration.fadeOutTime;
            }
            if (fadeTime <= 0)
            {
                audioSource.Stop();

                DestroyImmediate(gameObject);
            }
            else
            {
                Fade(fadeTime, 0f, false, true);
            }
        }

        public void Pause()
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

        public void Resume()
        {
            if (fadeTime <= 0)
            {
                paused = false;
                audioSource.UnPause();
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
                    finalVolume = container.roarConfiguration.fadeInFinalVolume;
                }
                Fade(fadeTime, container.roarConfiguration.fadeInFinalVolume, true);
            }
        }
        
        public AudioSource GetAudioSource() => audioSource;
        
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

        public void GenerateRandomPosition(float minRandomXYZ, float maxRandomXYZ)
        {
            float posX = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posY = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posZ = Random.Range(minRandomXYZ, maxRandomXYZ);
            transform.position = new Vector3(posX, posY, posZ);
        }

        private void MeasureEvent() => StartCoroutine(MeasureEventCoroutine());

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
                DestroyImmediate(gameObject);
            }

            if (pause)
            {
                StopAllCoroutines();
                paused = true;
                audioSource.Pause();
            }
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

        private IEnumerator SyncMeasureEvent()
        {
            yield return new WaitForSeconds((float)TrackInfo.GetTimeBeforeNextBar(audioSource, container.roarConfiguration.bpm, container.roarConfiguration.tempo));
            StartCoroutine(MeasureEventCoroutine());
        }

        private void Update()
        {
            UnityEditor.EditorApplication.delayCall += UnityEditor.EditorApplication.QueuePlayerLoopUpdate;
            if (!Application.isPlaying)
            {
                container.roarConfiguration.ApplyTo(audioSource);
            }
        }
    }
#endif
}
