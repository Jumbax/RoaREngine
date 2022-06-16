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
        
        public void SetContainer(RoaRContainer otherContainer)
        {
            container = otherContainer;
            audioSource.clip = container.Clip;
            container.SetConfiguration(audioSource);  
        }

        public bool CheckForContainerName(string containerName)
        {
            return container.Name == containerName;
        }

        public void Play(float fadeTime, float finalVolume, bool randomStartTime, float startTime, Transform parent, float minRandomXYZ, float maxRandomXYZ)
        {
            if (finalVolume <= 0f)
            {
                finalVolume = container.roarConfiguration.finalVolume;
            }
            if (fadeTime <= 0)
            {
                fadeTime = container.roarConfiguration.fadeInTime;
            }
            if (fadeTime > 0)
            {
                Fade(fadeTime, finalVolume);
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
                MeasureEvent();
            }
            if (container.roarConfiguration.onGoing)
            {
                StartCoroutine(PlayOnGoing());
            }
            if (!audioSource.loop && !container.roarConfiguration.onGoing)
            {
                AudioClipFinishPlaying();
            }
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
                    AudioClipFinishPlaying();
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
                    finalVolume = container.roarConfiguration.finalVolume;
                }
                Fade(fadeTime, container.roarConfiguration.finalVolume, true);
            }
        }

        public AudioSource GetAudioSource() => audioSource;

        public RoaRContainer GetContainer() => container;

        public void Fade(float fadeTime, float volume, bool resume = false, bool stop = false, bool pause = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeCoroutine(fadeTime, volume, resume, stop, pause));
        }

        public void AudioClipFinishPlaying() => StartCoroutine(AudioClipFinishPlayingCoroutine());
        
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

        private void MeasureEvent() => StartCoroutine(MeasureEventCoroutine());
        
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
                    AudioClipFinishPlaying();
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

        private IEnumerator SyncMeasureEvent()
        {
            yield return new WaitForSeconds((float)TrackInfo.GetTimeBeforeNextBar(audioSource, container.roarConfiguration.bpm, container.roarConfiguration.tempo));
            StartCoroutine(MeasureEventCoroutine());
        }
    }
}
