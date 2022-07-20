using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{

    public class RoaRManager : MonoBehaviour
    {
        private RoaRPooler roarPooler;
        public RoaRContainerMap RoarContainerMap = default;

        private void Start()
        {
            roarPooler = GetComponent<RoaRPooler>();
            RoarContainerMap.Init();
        }

        public void Play(string musicID, float fadeTime = 0f, float finalVolume = 0f, bool randomStartTime = false, float startTime = 0f, Transform parent = null, float minRandomXYZ = 0f, float maxRandomXYZ = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    Stop(musicID);
                }
                roarEmitter = roarPooler.Get();
                if (roarEmitter != null)
                {
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Play(fadeTime, finalVolume, randomStartTime, startTime, parent, minRandomXYZ, maxRandomXYZ);
                }
            }
        }

        public void Stop(string musicID, float fadeTime = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Stop(fadeTime);
                }
            }
        }

        public void Pause(string musicID, float fadeTime = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetEmitterObjectInPlay(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Pause(fadeTime);
                }
            }
        }

        public void Resume(string musicID, float fadeTime = 0f, float finalVolume = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Resume(fadeTime, finalVolume);
                }
            }
        }

        public void ChangeSequenceMode(string musicID, AudioSequenceMode audioSequenceMode)
        {
            RoaRContainer container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.sequenceMode = audioSequenceMode;
            }
        }

        private GameObject GetEmitterObjectInPlay(string musicID)
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

        private GameObject GetActiveEmitterObject(string musicID)
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

        private RoaREmitter GetEmitter(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    return roarEmitter.GetComponent<RoaREmitter>();
                }
            }
            return null;
        }

        public AudioSource GetAudioSource(string musicID)
        {
            RoaREmitter emitterComponent = GetEmitter(musicID);
            if (emitterComponent != null)
            {
                return emitterComponent.GetAudioSource();
            }
            return null;
        }

        public void AddEffect(string musicID, EffectType type)
        {
            GameObject emitter = GetActiveEmitterObject(musicID);
            if (emitter != null)
            {
                RoaREmitter emitterComponent = emitter.GetComponent<RoaREmitter>();
                emitterComponent.AddEffect(type);
            }
        }

        public T GetAudioSourceEffect<T>(string musicID)
        {
            T filter = GetAudioSource(musicID).GetComponent<T>();
            return filter;
        }

        public void AddMeasureEvent(string musicID, UnityAction measureAction)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                RoaRContainer container = RoarContainerMap.GetContainer(musicID);
                if (container != null)
                {
                    container.MeasureEvent += measureAction;
                }
            }
        }

        public void StopMeasureEvent(string musicID, UnityAction measureAction)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                RoaRContainer container = RoarContainerMap.GetContainer(musicID);
                if (container != null)
                {
                    container.MeasureEvent -= measureAction;
                }
            }
        }

        public void AddContainer(RoaRContainer container)
        {
            RoarContainerMap.AddContainer(container);
        }

        public void RemoveContainer(RoaRContainer container)
        {
            RoarContainerMap.RemoveContainer(container);
        }

        public RoaRContainer GetContainer(string musicID)
        {
            return RoarContainerMap.GetContainer(musicID);
        }

        public List<RoaRContainer> GetContainers()
        {
            return RoarContainerMap.roarContainers;
        }

        public int GetNumberContainers()
        {
            return RoarContainerMap.roarContainers.Count;
        }

        public int GetNumberAudioSources()
        {
            AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            return sources.Length;
        }

        public void SetBankIndex(string musicID, int value)
        {
            RoaRContainer container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.IndexClip = Mathf.Min(value, container.roarClipBank.audioClips.Length);
            }
        }
    
        public void Fade(string musicID, float fadeTime, float volume)
        {
            RoaREmitter emitter = GetEmitter(musicID);
            if (emitter != null)
            {
                Coroutine fade = StartCoroutine(emitter.FadeCoroutine(fadeTime, volume));
            }
        }
    }
}

