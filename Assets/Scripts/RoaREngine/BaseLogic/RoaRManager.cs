using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    public class RoaRManager : MonoBehaviour
    {
        private RoaRPooler roarPooler;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();
        public List<RoaRContainer> roarContainers;

        private void Awake()
        {
            //RoarContainerMap.Init();
            roarPooler = GetComponent<RoaRPooler>();
            SetNames();
            ResetContainersBankIndex();
        }

        private void OnEnable()
        {
            RoaREventChannel.Play += Play;
            RoaREventChannel.Pause += Pause;
            RoaREventChannel.Resume += Resume;
            RoaREventChannel.Stop += Stop;
            RoaREventChannel.AddContainer += AddContainer;
            RoaREventChannel.RemoveContainer += RemoveContainer;
            RoaREventChannel.GetContainer += GetContainer;
            RoaREventChannel.GetContainers += GetContainers;
            RoaREventChannel.GetNumberContainers += GetNumberContainers;
            RoaREventChannel.SetContainer += SetContainer;
            RoaREventChannel.ChangeSequenceMode += ChangeSequenceMode;
            RoaREventChannel.GetAudioSource += GetAudioSource;
            RoaREventChannel.GetNumberAudioSources += GetNumberAudioSources;
            RoaREventChannel.AddEffect += AddEffect;
            RoaREventChannel.GetAudioSourceEffect += GetAudioSourceEffect;
            RoaREventChannel.SetBankIndex += SetBankIndex;
            RoaREventChannel.Fade += Fade;
            RoaREventChannel.CrossFadeByParameter += CrossFadeByParameter;
            RoaREventChannel.CrossFadeByParameterWithParam += CrossFadeByParameterWithParam;
            RoaREventChannel.AddMeasureEvent += AddMeasureEvent;
            RoaREventChannel.RemoveMeasureEvent += RemoveMeasureEvent;
            RoaREventChannel.AddTimedEvent += AddTimedEvent;
            RoaREventChannel.RemoveTimedEvent += RemoveTimedEvent;
            RoaREventChannel.AddPlayEvent += AddPlayEvent;
            RoaREventChannel.RemovePlayEvent += RemovePlayEvent;
            RoaREventChannel.AddPauseEvent += AddPauseEvent;
            RoaREventChannel.RemovePauseEvent += RemovePauseEvent;
            RoaREventChannel.AddResumeEvent += AddResumeEvent;
            RoaREventChannel.RemoveResumeEvent += RemoveResumeEvent;
            RoaREventChannel.AddStopEvent += AddStopEvent;
            RoaREventChannel.RemoveStopEvent += RemoveStopEvent;
            RoaREventChannel.AddFinishedEvent += AddFinishedEvent;
            RoaREventChannel.RemoveFinishedEvent += RemoveFinishedEvent;
        }

        private void OnDisable()
        {
            RoaREventChannel.Play -= Play;
            RoaREventChannel.Pause -= Pause;
            RoaREventChannel.Resume -= Resume;
            RoaREventChannel.Stop -= Stop;
            RoaREventChannel.AddContainer -= AddContainer;
            RoaREventChannel.RemoveContainer -= RemoveContainer;
            RoaREventChannel.GetContainer -= GetContainer;
            RoaREventChannel.GetContainers -= GetContainers;
            RoaREventChannel.GetNumberContainers -= GetNumberContainers;
            RoaREventChannel.SetContainer -= SetContainer;
            RoaREventChannel.ChangeSequenceMode -= ChangeSequenceMode;
            RoaREventChannel.GetAudioSource -= GetAudioSource;
            RoaREventChannel.GetNumberAudioSources -= GetNumberAudioSources;
            RoaREventChannel.AddEffect -= AddEffect;
            RoaREventChannel.GetAudioSourceEffect -= GetAudioSourceEffect;
            RoaREventChannel.SetBankIndex -= SetBankIndex;
            RoaREventChannel.Fade -= Fade;
            RoaREventChannel.CrossFadeByParameter -= CrossFadeByParameter;
            RoaREventChannel.CrossFadeByParameterWithParam -= CrossFadeByParameterWithParam;
            RoaREventChannel.AddMeasureEvent -= AddMeasureEvent;
            RoaREventChannel.RemoveMeasureEvent -= RemoveMeasureEvent;
            RoaREventChannel.AddTimedEvent -= AddTimedEvent;
            RoaREventChannel.RemoveTimedEvent -= RemoveTimedEvent;
            RoaREventChannel.AddPlayEvent -= AddPlayEvent;
            RoaREventChannel.RemovePlayEvent -= RemovePlayEvent;
            RoaREventChannel.AddPauseEvent -= AddPauseEvent;
            RoaREventChannel.RemovePauseEvent -= RemovePauseEvent;
            RoaREventChannel.AddResumeEvent -= AddResumeEvent;
            RoaREventChannel.RemoveResumeEvent -= RemoveResumeEvent;
            RoaREventChannel.AddStopEvent -= AddStopEvent;
            RoaREventChannel.RemoveStopEvent -= RemoveStopEvent;
            RoaREventChannel.AddFinishedEvent -= AddFinishedEvent;
            RoaREventChannel.RemoveFinishedEvent -= RemoveFinishedEvent;
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
                roarEmitter = roarPooler.Get();
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

        //private T GetAudioSourceEffect<T>(string musicID)
        //{
        //    //TEST
        //    T filter = GetAudioSource(musicID).GetComponent<T>();
        //    return filter;
        //}

        private Behaviour GetAudioSourceEffect(string musicID)
        {
            //TEST
            Behaviour filter = GetAudioSource(musicID).GetComponent<Behaviour>();
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
    }
}

