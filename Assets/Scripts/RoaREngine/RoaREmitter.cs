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

        public void Play()
        {
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public bool IsInPause()
        {
            return paused;
        }

        public void Pause()
        {
            StopAllCoroutines();
            paused = true;
            audioSource.Pause();
        }

        public void Resume()
        {
            paused = false;
            audioSource.UnPause();
            if (!container.roarConfiguration.ongGoing)
            {
                AudioClipFinishPlaying();
            }
        }

        public AudioSource GetAudioSource() => audioSource;

        public RoaRContainer GetContainer() => container;

        public void FadeIn(float fadeTime, bool resume = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCoroutine(fadeTime, resume));
        }

        public void FadeOut(float fadeTime, bool stop = false, bool pause = false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutCoroutine(fadeTime, stop, pause));
        }

        public void AudioClipFinishPlaying()
        {
            StartCoroutine(AudioClipFinishPlayingCoroutine());
        }

        private IEnumerator FadeInCoroutine(float fadeTime, bool resume = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            if (resume)
            {
                Resume();
            }

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 1f;
        }

        private IEnumerator FadeOutCoroutine(float fadeTime, bool stop = false, bool pause = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 0f;
            if (stop)
            {
                Stop();
            }
            if (pause)
            {
                Pause();
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

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
            transform.position = parent.position;
            transform.position = Vector3.zero;
        }

        public void ResetParent()
        {
            transform.parent = initialParent;
        }

        public void UpdateEmitter()
        {
            if (container.roarConfiguration.ongGoing)
            {
                if (!audioSource.isPlaying && !paused)
                {
                    if (container.roarConfiguration.minTime != 0 || container.roarConfiguration.maxTime != 0)
                    {
                        audioSource.clip = container.Clip;
                        float seconds = Random.Range(container.roarConfiguration.minTime, container.roarConfiguration.maxTime);
                        audioSource.PlayDelayed(seconds);
                    }
                    else
                    {
                        audioSource.clip = container.Clip;
                        audioSource.Play();
                    }
                }
            }
        }

        public void GenerateRandomPosition(float minRandomXYZ, float maxRandomXYZ)
        {
            float posX = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posY = Random.Range(minRandomXYZ, maxRandomXYZ);
            float posZ = Random.Range(minRandomXYZ, maxRandomXYZ);
            transform.position = new Vector3(posX, posY, posZ);
        }
    }
}
