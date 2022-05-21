using System.Collections;
using System.Collections.Generic;
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

        public void Play(string musicID)
        {
            GameObject roarEmitter = roarPooler.Get();
            if (roarEmitter != null)
            {
                if (RoarContainerMap.MusicIDIsValid(musicID))
                {
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    roarEmitter.GetComponent<RoaREmitter>().Play();
                }
            }
        }

        public void PlayEsclusive(string musicID)
        {
            GameObject roarEmitter = roarPooler.Get();
            if (roarEmitter != null)
            {
                if (RoarContainerMap.MusicIDIsValid(musicID))
                {
                    GameObject oldEmitter = SearchActiveEmitterInPlay(musicID);
                    if (oldEmitter != null)
                    {
                        oldEmitter.GetComponent<RoaREmitter>().Stop();
                    }                    
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    roarEmitter.GetComponent<RoaREmitter>().Play();
                }
            }
        }
           
        public void PlayWithFade(string musicID, float fadeTime)
        {
            GameObject roarEmitter = roarPooler.Get();
            if (roarEmitter != null)
            {
                if (RoarContainerMap.MusicIDIsValid(musicID))
                {
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    roarEmitter.GetComponent<RoaREmitter>().Play();
                    StartCoroutine(FadeInCoroutine(roarEmitter, fadeTime));
                }
            }
        }

        public void Stop(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitterInPlay(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Stop();
                }
            }
        }

        public void Pause(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitterInPlay(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Pause();
                }
            }
        }

        public void Resume(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitter(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Resume();
                }
            }
        }

        private GameObject SearchActiveEmitterInPlay(string musicID)
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
                if (!roarEmitter.GetComponent<RoaREmitter>().IsPlaying() && roarEmitter.gameObject.activeInHierarchy == true)
                {
                    if (roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }

        private IEnumerator FadeInCoroutine(GameObject emitter, float fadeTime)
        {
            float time = 0f;
            AudioSource audioSource = emitter.GetComponent<RoaREmitter>().GetAudioSource();
            float startVolume = audioSource.volume;
            float duration = fadeTime;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 1f;
        }
    }
}
