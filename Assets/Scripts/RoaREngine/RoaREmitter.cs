using System.Collections;
using UnityEngine;

namespace RoaREngine
{
    public class RoaREmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private RoaRContainer container;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
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
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void Resume()
        {
            audioSource.UnPause();
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
            //StopAllCoroutines();
            StartCoroutine(AudioClipFinishPlayingCoroutine());
        }

        private IEnumerator FadeInCoroutine(float fadeTime, bool resume = false)
        {
            float time = 0f;
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            if (resume)
            {
                audioSource.UnPause();
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
                audioSource.Stop();
                audioSource.gameObject.SetActive(false);
            }
            if (pause)
            {
                audioSource.Pause();
            }
        }

        private IEnumerator AudioClipFinishPlayingCoroutine()
        {
            float clipLengthRemaining = audioSource.clip.length - audioSource.time;
            yield return new WaitForSeconds(clipLengthRemaining);
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        }

        public void UpdateEmitter()
        {
            if (container.roarConfiguration.ongGoing)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
