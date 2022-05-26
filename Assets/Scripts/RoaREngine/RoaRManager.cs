using System.Collections;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRManager : MonoBehaviour
    {
        private RoaRPooler roarPooler;
        public RoaRContainerMap RoarContainerMap = default;

        private void Start()
        {
            roarPooler = GetComponent<RoaRPooler>();
            RoarContainerMap.SetNames();
        }

        public void Play(string musicID, float fadeTime = 0f, bool esclusive = false, float startTime = 0f, bool randomStartTime = false)
        {
            GameObject roarEmitter = roarPooler.Get();
            if (roarEmitter != null)
            {
                if (RoarContainerMap.MusicIDIsValid(musicID))
                {
                    if (esclusive)
                    {
                        GameObject oldEmitter = SearchEmitterInPlay(musicID);
                        if (oldEmitter != null)
                        {
                            oldEmitter.GetComponent<RoaREmitter>().Stop();
                            oldEmitter.gameObject.SetActive(false);
                        }
                    }
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    if (startTime <= 0)
                    {
                        startTime = roarEmitter.GetComponent<RoaREmitter>().GetContainer().roarConfiguration.startTime;
                    }
                    if (!randomStartTime)
                    {
                        randomStartTime = roarEmitter.GetComponent<RoaREmitter>().GetContainer().roarConfiguration.randomStartTime;
                    }
                    if (randomStartTime)
                    {
                        startTime = Random.Range(0f, roarEmitter.GetComponent<RoaREmitter>().GetAudioSource().clip.length);
                        roarEmitter.GetComponent<RoaREmitter>().GetAudioSource().time = startTime;
                    }
                    if (startTime > 0 && !randomStartTime)
                    {
                        startTime = Mathf.Clamp(startTime, 0f, roarEmitter.GetComponent<RoaREmitter>().GetAudioSource().clip.length - 0.01f);
                        roarEmitter.GetComponent<RoaREmitter>().GetAudioSource().time = startTime;
                    }
                    if (fadeTime <= 0)
                    {
                        fadeTime = roarEmitter.GetComponent<RoaREmitter>().GetContainer().roarConfiguration.fadeInvolume;
                    }
                    if (fadeTime > 0)
                    {
                        roarEmitter.GetComponent<RoaREmitter>().FadeIn(fadeTime);
                    }
                    roarEmitter.GetComponent<RoaREmitter>().Play();
                    if (!roarEmitter.GetComponent<RoaREmitter>().GetAudioSource().loop)
                    {
                        roarEmitter.GetComponent<RoaREmitter>().AudioClipFinishPlaying();
                    }

                }
            }
        }

        public void Stop(string musicID, float fadeTime = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitter(musicID);
                if (roarEmitter != null)
                {
                    if (fadeTime <= 0)
                    {
                        fadeTime = roarEmitter.GetComponent<RoaREmitter>().GetContainer().roarConfiguration.fadeOutvolume;
                    }
                    if (fadeTime <= 0)
                    {
                        roarEmitter.GetComponent<RoaREmitter>().Stop();
                        roarEmitter.gameObject.SetActive(false);
                    }
                    else
                    {
                        roarEmitter.GetComponent<RoaREmitter>().FadeOut(fadeTime, true);
                    }
                }
            }
        }

        public void Pause(string musicID, float fadeTime = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchEmitterInPlay(musicID);
                if (roarEmitter != null)
                {
                    if (fadeTime <= 0)
                    {
                        roarEmitter.GetComponent<RoaREmitter>().Pause();
                    }
                    else
                    {
                        roarEmitter.GetComponent<RoaREmitter>().FadeOut(fadeTime, false, true);
                    }
                }
            }
        }

        public void Resume(string musicID, float fadeTime = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitter(musicID);
                if (roarEmitter != null)
                {
                    if (fadeTime <= 0)
                    {
                        roarEmitter.GetComponent<RoaREmitter>().Resume();
                    }
                    else
                    {
                        roarEmitter.GetComponent<RoaREmitter>().FadeIn(fadeTime, true);
                    }
                }
            }
        }

        private GameObject SearchEmitterInPlay(string musicID)
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                if (roarEmitter.GetComponent<RoaREmitter>().IsPlaying())
                {
                    if (roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }

        private GameObject SearchActiveEmitter(string musicID)
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                if (roarEmitter.gameObject.activeInHierarchy == true)
                {
                    if (roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }

        private IEnumerator FadeInCoroutine(GameObject emitter, float fadeTime, bool resume = false)
        {
            float time = 0f;
            AudioSource audioSource = emitter.GetComponent<RoaREmitter>().GetAudioSource();
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

        private IEnumerator FadeOutCoroutine(GameObject emitter, float fadeTime, bool stop = false, bool pause = false)
        {
            float time = 0f;
            AudioSource audioSource = emitter.GetComponent<RoaREmitter>().GetAudioSource();
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
                emitter.gameObject.SetActive(false);
            }
            if (pause)
            {
                audioSource.Pause();
            }
        }

        private IEnumerator AudioClipFinishPlaying(string musicID, float lenght)
        {
            yield return new WaitForSeconds(lenght);
            Stop(musicID);
        }
    }
}
