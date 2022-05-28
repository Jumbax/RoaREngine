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
                RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
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
                        startTime = emitterComponent.GetContainer().roarConfiguration.startTime;
                    }
                    if (!randomStartTime)
                    {
                        randomStartTime = emitterComponent.GetContainer().roarConfiguration.randomStartTime;
                    }
                    if (randomStartTime)
                    {
                        startTime = Random.Range(0f, emitterComponent.GetAudioSource().clip.length);
                        emitterComponent.GetAudioSource().time = startTime;
                    }
                    if (startTime > 0 && !randomStartTime)
                    {
                        startTime = Mathf.Clamp(startTime, 0f, emitterComponent.GetAudioSource().clip.length - 0.01f);
                        emitterComponent.GetAudioSource().time = startTime;
                    }
                    if (fadeTime <= 0)
                    {
                        fadeTime = emitterComponent.GetContainer().roarConfiguration.fadeInvolume;
                    }
                    if (fadeTime > 0)
                    {
                        emitterComponent.FadeIn(fadeTime);
                    }
                    emitterComponent.Play();
                    if (!emitterComponent.GetAudioSource().loop && !emitterComponent.GetContainer().roarConfiguration.ongGoing)
                    {
                        emitterComponent.AudioClipFinishPlaying();
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
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    if (fadeTime <= 0)
                    {
                        fadeTime = emitterComponent.GetContainer().roarConfiguration.fadeOutvolume;
                    }
                    if (fadeTime <= 0)
                    {
                        emitterComponent.Stop();
                        roarEmitter.gameObject.SetActive(false);
                    }
                    else
                    {
                        emitterComponent.FadeOut(fadeTime, true);
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
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    if (fadeTime <= 0)
                    {
                        emitterComponent.Pause();
                    }
                    else
                    {
                        emitterComponent.FadeOut(fadeTime, false, true);
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
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    if (fadeTime <= 0)
                    {
                        emitterComponent.Resume();
                    }
                    else
                    {
                        emitterComponent.FadeIn(fadeTime, true);
                    }
                }
            }
        }

        private GameObject SearchEmitterInPlay(string musicID)
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                if (emitterComponent.IsPlaying())
                {
                    if (emitterComponent.CheckForContainerName(musicID))
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
                RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                if (roarEmitter.gameObject.activeInHierarchy == true)
                {
                    if (emitterComponent.CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }

        private void Update()
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                if (roarEmitter.gameObject.activeInHierarchy)
                {
                    roarEmitter.GetComponent<RoaREmitter>().UpdateEmitter();
                }
            }
        }
    }
}
