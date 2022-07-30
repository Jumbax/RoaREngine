using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{

    public class RoaRManager : MonoBehaviour
    {
        //public RoaRContainerMap RoarContainerMap = default;
        private RoaRPooler roarPooler;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();
        public List<RoaRContainer> roarContainers;

        #region private
        private void Awake()
        {
            //RoarContainerMap.Init();
            roarPooler = GetComponent<RoaRPooler>();
            SetNames();
            ResetContainersBankIndex();
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
        #endregion

        #region public
        public void AddContainer(RoaRContainer container)
        {
            roarContainers.Add(container);
            containerDict[container.Name] = container;
        }

        public void RemoveContainer(RoaRContainer container)
        {
            roarContainers.Remove(container);
            containerDict.Remove(container.Name);
        }

        public RoaRContainer GetContainer(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                return containerDict[musicID];
            }
            return null;
        }

        public void SetContainer(string musicID, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoaREmitter>().SetContainer(containerDict[musicID]);
        }

        public void Play(string musicID, bool esclusive = false)
        {
            if (MusicIDIsValid(musicID))
            {
                if (GetContainer(musicID).roarClipBank.GetClip() == null)
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
                roarEmitter = roarPooler.Get();
                if (roarEmitter != null)
                {
                    SetContainer(musicID, roarEmitter);
                    RoaREmitter emitterComponent = roarEmitter.GetComponent<RoaREmitter>();
                    emitterComponent.Play();
                }
            }
        }

        public void Stop(string musicID)
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

        public void Pause(string musicID)
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

        public void Resume(string musicID)
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

        public void ChangeSequenceMode(string musicID, AudioSequenceMode audioSequenceMode)
        {
            RoaRContainer container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.sequenceMode = audioSequenceMode;
            }
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
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.MeasureEvent += measureAction;
                }
            }
        }

        public void RemoveMeasureEvent(string musicID, UnityAction measureAction)
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

        public void AddMarkerEvent(string musicID, UnityAction markerAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.MarkerEvent += markerAction;
                }
            }
        }

        public void RemoveMarkerEvent(string musicID, UnityAction markerAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoaRContainer container = GetContainer(musicID);
                if (container != null)
                {
                    container.MarkerEvent -= markerAction;
                }
            }
        }

        public void AddFinishedEvent(string musicID, UnityAction finishEvent)
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

        public void RemoveFinishedEvent(string musicID, UnityAction finishEvent)
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

        public void AddPlayEvent(string musicID, UnityAction playEvent)
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

        public void RemovePlayEvent(string musicID, UnityAction playEvent)
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

        public void AddPauseEvent(string musicID, UnityAction pauseEvent)
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

        public void RemovePauseEvent(string musicID, UnityAction pauseEvent)
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

        public void AddStopEvent(string musicID, UnityAction stopEvent)
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

        public void RemoveStopEvent(string musicID, UnityAction stopEvent)
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

        public void AddResumeEvent(string musicID, UnityAction resumeEvent)
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

        public void RemoveResumeEvent(string musicID, UnityAction resumeEvent)
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

        public List<RoaRContainer> GetContainers()
        {
            return roarContainers;
        }

        public int GetNumberContainers()
        {
            return roarContainers.Count;
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

        public void CrossFadeByParameter(string[] musicsID, float[][] crossFadeInput, float param)
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

        public void CrossFadeByParameter(string[] musicsID, float param)
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
        #endregion
    }
}

