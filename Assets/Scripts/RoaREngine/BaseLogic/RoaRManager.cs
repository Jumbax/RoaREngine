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

        public void Play(string musicID, float fadeTime = 0f, float finalVolume = 0f, bool randomStartTime = false, float startTime = 0f, Transform parent = null, float minRandomXYZ = 0f, float maxRandomXYZ = 0f, float delay = 0f)
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
                    emitterComponent.Play(fadeTime, finalVolume, randomStartTime, startTime, parent, minRandomXYZ, maxRandomXYZ, delay);
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

        public void Fade(string musicID, float fadeTime, float finalVolume)
        {
            RoaREmitter emitter = GetEmitter(musicID);
            if (emitter != null)
            {
                emitter.Fade(fadeTime, finalVolume);
            }
        }

        public void CrossFadeByParameter(RoaRCrossFadeDataSO data, float param)
        {
            for (int i = 0; i < data.Names.Length; i++)
            {
                if (param < data.Parameters[i].parameters[0])
                {
                    return;
                }
                if (param >= data.Parameters[i].parameters[0] && param <= data.Parameters[i].parameters[1])
                {
                    if (data.Parameters[i].parameters[0] == 0 && data.Parameters[i].parameters[1] == 0)
                    {
                        GetAudioSource(data.Names[i]).volume = 1f;
                        return;
                    }
                    GetAudioSource(data.Names[i]).volume = TrackInfo.Remap(param, data.Parameters[i].parameters[0], data.Parameters[i].parameters[1]);
                }
                else
                {
                    GetAudioSource(data.Names[i]).volume = 1f - TrackInfo.Remap(param, data.Parameters[i].parameters[2], data.Parameters[i].parameters[3]);
                }
            }

        }

        public void CrossFadeByParameter(string[] musicsID, float[][] crossFadeInput, float param)
        {
            for (int i = 0; i < musicsID.Length; i++)
            {
                if (param < crossFadeInput[i][0])
                {
                    return;
                }
                if (param >= crossFadeInput[i][0] && param <= crossFadeInput[i][1])
                {
                    if (crossFadeInput[i][0] == 0 && crossFadeInput[i][1] == 0)
                    {
                        GetAudioSource(musicsID[i]).volume = 1f;
                        return;
                    }
                    GetAudioSource(musicsID[i]).volume = TrackInfo.Remap(param, crossFadeInput[i][0], crossFadeInput[i][1]);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - TrackInfo.Remap(param, crossFadeInput[i][2], crossFadeInput[i][3]);
                }
            }
        }
    
        public void CrossFadeByParameter(string[] musicsID, float param)
        {
            for (int i = 0; i < musicsID.Length; i++)
            {
                if (param < GetContainer(musicsID[i]).roarConfiguration.parameters[0])
                {
                    return;
                }
                if (param >= GetContainer(musicsID[i]).roarConfiguration.parameters[0] && param <= GetContainer(musicsID[i]).roarConfiguration.parameters[1])
                {
                    if (GetContainer(musicsID[i]).roarConfiguration.parameters[0] == 0 && GetContainer(musicsID[i]).roarConfiguration.parameters[1] == 0)
                    {
                        GetAudioSource(musicsID[i]).volume = 1f;
                        return;
                    }
                    GetAudioSource(musicsID[i]).volume = TrackInfo.Remap(param, GetContainer(musicsID[i]).roarConfiguration.parameters[0], GetContainer(musicsID[i]).roarConfiguration.parameters[1]);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - TrackInfo.Remap(param, GetContainer(musicsID[i]).roarConfiguration.parameters[2], GetContainer(musicsID[i]).roarConfiguration.parameters[3]);
                }
            }
        }
    }
}

