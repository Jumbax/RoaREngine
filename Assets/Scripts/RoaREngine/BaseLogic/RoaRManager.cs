using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    public class RoaRManager : MonoBehaviour
    {
        #region var
        [SerializeField] private GameObject roarEmitter;
        [SerializeField] private int count;
        [SerializeField] private List<RoaRContainer> roarContainers;
        private List<GameObject> roarEmitters;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();
        #endregion

        #region functions
        private void Awake()
        {
            //RoarContainerMap.Init();
            //roarPooler = GetComponent<RoaRPooler>();
            SetNames();
            ResetContainersBankIndex();
            SetInitialEmitters();
        }

        private void SetInitialEmitters()
        {
            roarEmitters = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(roarEmitter, transform);
                go.SetActive(false);
                roarEmitters.Add(go);
            }
        }
        
        public GameObject GetEmitter()
        {
            for (int i = 0; i < roarEmitters.Count; i++)
            {
                if (!roarEmitters[i].activeInHierarchy)
                {
                    roarEmitters[i].SetActive(true);
                    return roarEmitters[i];
                }
            }

            GameObject go = Instantiate(roarEmitter, transform);
            go.SetActive(true);
            roarEmitters.Add(go);
            return go;
        }

        private void Play(string musicID, bool esclusive = false)
        {
            if (MusicIDIsValid(musicID))
            {
                if (GetContainer(musicID).roarClipBank.audioClips.Length <= 0)
                {
                    return;
                }
                GameObject roarEmitter;
                if (esclusive)
                {
                    roarEmitter = GetActiveEmitterObject(musicID);
                    if (roarEmitter != null)
                    {
                        Stop(musicID);
                    }
                }
                roarEmitter = GetEmitter();
                if (roarEmitter != null)
                {
                    SetContainer(musicID, roarEmitter);
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Play();
                }
            }
        }

        private void Stop(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Stop();
                }
            }
        }

        private void Pause(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Pause();
                }
            }
        }

        private void Resume(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Resume();
                }
            }
        }

        private void SetNames()
        {
            foreach (RoaRContainer container in roarContainers)
            {
                //CONTROLLO PER VEDERE SE CI SONO CONTAINER CON LO STESSO NOME
                containerDict[container.Name] = container;
            }
        }

        private void ResetContainersBankIndex()
        {
            foreach (RoaRContainer container in roarContainers)
            {
                container.ResetBankIndex();
            }
        }

        private bool MusicIDIsValid(string musicID)
        {
            return containerDict.ContainsKey(musicID);
        }

        private GameObject GetActiveEmitterObject(string musicID)
        {
            foreach (GameObject roarEmitter in roarEmitters)
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
            if (MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    return roarEmitter.GetComponent<RoaREmitter>();
                }
            }
            return null;
        }
     
        private void AddContainer(RoaRContainer container)
        {
            roarContainers.Add(container);
            containerDict[container.Name] = container;
        }

        private void RemoveContainer(RoaRContainer container)
        {
            roarContainers.Remove(container);
            containerDict.Remove(container.Name);
        }

        private RoaRContainer GetContainer(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                return containerDict[musicID];
            }
            return null;
        }

        private void SetContainer(string musicID, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoaREmitter>().SetContainer(containerDict[musicID]);
        }
        
        private List<RoaRContainer> GetContainers()
        {
            return roarContainers;
        }

        private void ChangeSequenceMode(string musicID, AudioSequenceMode audioSequenceMode)
        {
            RoaRContainer container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.sequenceMode = audioSequenceMode;
            }
        }

        private AudioSource GetAudioSource(string musicID)
        {
            RoaREmitter emitterComponent = GetEmitter(musicID);
            if (emitterComponent != null)
            {
                return emitterComponent.GetAudioSource();
            }
            return null;
        }

        private void AddEffect(string musicID, EffectType type)
        {
            GameObject emitter = GetActiveEmitterObject(musicID);
            if (emitter != null)
            {
                RoaREmitter emitterComponent = emitter.GetComponent<RoaREmitter>();
                emitterComponent.AddEffect(type);
            }
        }

        private T GetAudioSourceEffect<T>(string musicID)
        {
            T filter = GetAudioSource(musicID).GetComponent<T>();
            return filter;
        }

        private int GetNumberContainers()
        {
            return roarContainers.Count;
        }

        private int GetNumberAudioSources()
        {
            AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            return sources.Length;
        }

        private void SetBankIndex(string musicID, int value)
        {
            RoaRContainer container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.IndexClip = Mathf.Min(value, container.roarClipBank.audioClips.Length);
            }
        }

        private void Fade(string musicID, float fadeTime, float finalVolume)
        {
            RoaREmitter emitter = GetEmitter(musicID);
            if (emitter != null)
            {
                emitter.Fade(fadeTime, finalVolume);
            }
        }

        private void CrossFadeByParameter(string[] musicsID, float param)
        {
            for (int i = 0; i < musicsID.Length; i++)
            {
                float fadeInParamValueStart = GetContainer(musicsID[i]).roarConfiguration.fadeInParamValueStart;
                float fadeInParamValueEnd = GetContainer(musicsID[i]).roarConfiguration.fadeInParamValueEnd;
                float fadeOutParamValueStart = GetContainer(musicsID[i]).roarConfiguration.fadeOutParamValueStart;
                float fadeOutParamValueEnd = GetContainer(musicsID[i]).roarConfiguration.fadeOutParamValueEnd;

                if (param < fadeInParamValueStart)
                {
                    GetAudioSource(musicsID[i]).volume = 0f;
                    continue;
                }
                if (param >= fadeInParamValueStart && param <= fadeInParamValueEnd)
                {
                    if (fadeInParamValueStart == 0 && fadeInParamValueEnd == 0)
                    {
                        GetAudioSource(musicsID[i]).volume = 1f;
                        continue;
                    }
                    GetAudioSource(musicsID[i]).volume = TrackInfo.Remap(param, fadeInParamValueStart, fadeInParamValueEnd);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - TrackInfo.Remap(param, fadeOutParamValueStart, fadeOutParamValueEnd);
                }
            }
        }

        private void CrossFadeByParameterWithParam(string[] musicsID, float[][] crossFadeInput, float param)
        {
            for (int i = 0; i < musicsID.Length; i++)
            {
                if (param < crossFadeInput[i][0])
                {
                    GetAudioSource(musicsID[i]).volume = 0f;
                    continue;
                }
                if (param >= crossFadeInput[i][0] && param <= crossFadeInput[i][1])
                {
                    if (crossFadeInput[i][0] == 0 && crossFadeInput[i][1] == 0)
                    {
                        GetAudioSource(musicsID[i]).volume = 1f;
                        continue;
                    }
                    GetAudioSource(musicsID[i]).volume = TrackInfo.Remap(param, crossFadeInput[i][0], crossFadeInput[i][1]);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - TrackInfo.Remap(param, crossFadeInput[i][2], crossFadeInput[i][3]);
                }
            }
        }

        private void AddMeasureEvent(string musicID, UnityAction measureAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.MeasureEvent += measureAction;
                }
            }
        }

        private void RemoveMeasureEvent(string musicID, UnityAction measureAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.MeasureEvent -= measureAction;
                }
            }
        }

        private void AddTimedEvent(string musicID, UnityAction markerAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.TimedEvent += markerAction;
                }
            }
        }

        private void RemoveTimedEvent(string musicID, UnityAction markerAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.TimedEvent -= markerAction;
                }
            }
        }

        private void AddPlayEvent(string musicID, UnityAction playEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnPlayEvent += playEvent;
                }
            }
        }

        private void RemovePlayEvent(string musicID, UnityAction playEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnPlayEvent -= playEvent;
                }
            }
        }

        private void AddPauseEvent(string musicID, UnityAction pauseEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnPauseEvent += pauseEvent;
                }
            }
        }
   
        private void RemovePauseEvent(string musicID, UnityAction pauseEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnPauseEvent -= pauseEvent;
                }
            }
        }
      
        private void AddResumeEvent(string musicID, UnityAction resumeEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnResumeEvent += resumeEvent;
                }
            }
        }
       
        private void RemoveResumeEvent(string musicID, UnityAction resumeEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnResumeEvent -= resumeEvent;
                }
            }
        }
       
        private void AddStopEvent(string musicID, UnityAction stopEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnStopEvent += stopEvent;
                }
            }
        }

        private void RemoveStopEvent(string musicID, UnityAction stopEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnStopEvent -= stopEvent;
                }
            }
        }
   
        private void AddFinishedEvent(string musicID, UnityAction finishEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnFinishedEvent += finishEvent;
                }
            }
        }
       
        private void RemoveFinishedEvent(string musicID, UnityAction finishEvent)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnFinishedEvent -= finishEvent;
                }
            }
        }
        #endregion
    }
}

