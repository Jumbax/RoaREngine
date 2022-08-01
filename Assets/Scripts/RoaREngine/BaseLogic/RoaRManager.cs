using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    public class RoaRManager : MonoBehaviour
    {
        #region var
        private RoaRPooler roarPooler;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();
        public List<RoaRContainer> roarContainers;
        #endregion

        #region delegate
        public static UnityAction<string, bool> PlayAction;
        public static UnityAction<string> PauseAction;
        public static UnityAction<string> ResumeAction;
        public static UnityAction<string> StopAction;
        public static UnityAction<RoaRContainer> AddContainerAction;
        public static UnityAction<RoaRContainer> RemoveContainerAction;
        public static Func<string, RoaRContainer> GetContainerFunc;
        public static Func<List<RoaRContainer>> GetContainersFunc;
        public static Func<int> GetNumberContainersFunc;
        public static UnityAction<string, AudioSequenceMode> ChangeSequenceModeAction;
        public static Func<string, AudioSource> GetAudioSourceFunc;
        public static Func<int> GetNumberAudioSourcesFunc;
        public static UnityAction<string, EffectType> AddEffectAction;
        public static Func<string, Behaviour> GetAudioSourceEffectFunc;
        public static UnityAction<string, int> SetBankIndexAction;
        public static UnityAction<string, float, float> FadeAction;
        public static UnityAction<string[], float> CrossFadeByParameterAction;
        public static UnityAction<string[], float[][], float> CrossFadeByParameterWithParamAction;
        public static UnityAction<string, UnityAction> AddMeasureEventAction;
        public static UnityAction<string, UnityAction> RemoveMeasureEventAction;
        public static UnityAction<string, UnityAction> AddTimedEventAction;
        public static UnityAction<string, UnityAction> RemoveTimedEventAction;
        public static UnityAction<string, UnityAction> AddPlayEventAction;
        public static UnityAction<string, UnityAction> RemovePlayEventAction;
        public static UnityAction<string, UnityAction> AddPauseEventAction;
        public static UnityAction<string, UnityAction> RemovePauseEventAction;
        public static UnityAction<string, UnityAction> AddResumeEventAction;
        public static UnityAction<string, UnityAction> RemoveResumeEventAction;
        public static UnityAction<string, UnityAction> AddStopEventAction;
        public static UnityAction<string, UnityAction> RemoveStopEventAction;
        public static UnityAction<string, UnityAction> AddFinishedEventAction;
        public static UnityAction<string, UnityAction> RemoveFinishedEventAction;
        #endregion

        #region private functions
        private void Awake()
        {
            //RoarContainerMap.Init();
            roarPooler = GetComponent<RoaRPooler>();
            SetNames();
            ResetContainersBankIndex();
        }

        private void OnEnable()
        {
            PlayAction += Play;
            PauseAction += Pause;
            ResumeAction += Resume;
            StopAction += Stop;
            AddContainerAction += AddContainer;
            RemoveContainerAction += RemoveContainer;
            GetContainerFunc += GetContainer;
            GetContainersFunc += GetContainers;
            GetNumberContainersFunc += GetNumberContainers;
            ChangeSequenceModeAction += ChangeSequenceMode;
            GetAudioSourceFunc += GetAudioSource;
            GetNumberAudioSourcesFunc += GetNumberAudioSources;
            AddEffectAction += AddEffect;
            GetAudioSourceEffectFunc += GetAudioSourceEffect;
            SetBankIndexAction += SetBankIndex;
            FadeAction += Fade;
            CrossFadeByParameterAction += CrossFadeByParameter;
            CrossFadeByParameterWithParamAction += CrossFadeByParameterWithParam;
            AddMeasureEventAction += AddMeasureEvent;
            RemoveMeasureEventAction += RemoveMeasureEvent;
            AddTimedEventAction += AddTimedEvent;
            RemoveTimedEventAction += RemoveTimedEvent;
            AddPlayEventAction += AddPlayEvent;
            RemovePlayEventAction += RemovePlayEvent;
            AddPauseEventAction += AddPauseEvent;
            RemovePauseEventAction += RemovePauseEvent;
            AddResumeEventAction += AddResumeEvent;
            RemoveResumeEventAction += RemoveResumeEvent;
            AddStopEventAction += AddStopEvent;
            RemoveStopEventAction += RemoveStopEvent;
            AddFinishedEventAction += AddFinishedEvent;
            RemoveFinishedEventAction += RemoveFinishedEvent;
        }

        private void OnDisable()
        {
            PlayAction -= Play;
            PauseAction -= Pause;
            ResumeAction -= Resume;
            StopAction -= Stop;
            AddContainerAction -= AddContainer;
            RemoveContainerAction -= RemoveContainer;
            GetContainerFunc -= GetContainer;
            GetContainersFunc -= GetContainers;
            GetNumberContainersFunc -= GetNumberContainers;
            ChangeSequenceModeAction -= ChangeSequenceMode;
            GetAudioSourceFunc -= GetAudioSource;
            GetNumberAudioSourcesFunc -= GetNumberAudioSources;
            AddEffectAction -= AddEffect;
            GetAudioSourceEffectFunc -= GetAudioSourceEffect;
            SetBankIndexAction -= SetBankIndex;
            FadeAction -= Fade;
            CrossFadeByParameterAction -= CrossFadeByParameter;
            CrossFadeByParameterWithParamAction -= CrossFadeByParameterWithParam;
            AddMeasureEventAction -= AddMeasureEvent;
            RemoveMeasureEventAction -= RemoveMeasureEvent;
            AddTimedEventAction -= AddTimedEvent;
            RemoveTimedEventAction -= RemoveTimedEvent;
            AddPlayEventAction -= AddPlayEvent;
            RemovePlayEventAction -= RemovePlayEvent;
            AddPauseEventAction -= AddPauseEvent;
            RemovePauseEventAction -= RemovePauseEvent;
            AddResumeEventAction -= AddResumeEvent;
            RemoveResumeEventAction -= RemoveResumeEvent;
            AddStopEventAction -= AddStopEvent;
            RemoveStopEventAction -= RemoveStopEvent;
            AddFinishedEventAction -= AddFinishedEvent;
            RemoveFinishedEventAction -= RemoveFinishedEvent;
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
        #endregion

        #region public functions
        public static void CallPlay(string musicID, bool esclusive = false) => PlayAction?.Invoke(musicID, esclusive);
        public static void CallPause(string musicID) => PauseAction?.Invoke(musicID);
        public static void CallResume(string musicID) => ResumeAction?.Invoke(musicID);
        public static void CallStop(string musicID) => StopAction?.Invoke(musicID);
        public static void CallAddContainer(RoaRContainer container) => AddContainerAction?.Invoke(container);
        public static void CallRemoveContainer(RoaRContainer container) => RemoveContainerAction?.Invoke(container);
        public static RoaRContainer CallGetContainer(string musicID) => GetContainerFunc?.Invoke(musicID);
        public static List<RoaRContainer> CallGetContainers() => GetContainersFunc?.Invoke();
        public static int CallGetNumberContainers() => (int)GetNumberContainersFunc?.Invoke();
        public static void CallChangeSequenceMode(string musicID, AudioSequenceMode audioSequenceMode) => ChangeSequenceModeAction?.Invoke(musicID, audioSequenceMode);
        public static AudioSource CallGetAudioSource(string musicID) => GetAudioSourceFunc?.Invoke(musicID);
        public static int CallGetNumberAudioSources() => (int)GetNumberAudioSourcesFunc?.Invoke();
        public static void CallAddEffect(string musicID, EffectType type) => AddEffectAction?.Invoke(musicID, type);
        public static Behaviour CallGetAudioSourceEffect(string musicID, Behaviour effect) => GetAudioSourceEffectFunc?.Invoke(musicID);
        public static void CallSetBankIndex(string musicID, int index) => SetBankIndexAction?.Invoke(musicID, index);
        public static void CallFade(string musicID, float fadeTime, float finalVolume) => FadeAction?.Invoke(musicID, fadeTime, finalVolume);
        public static void CallCrossFadeByParameter(string[] musicIDs, float parameter) => CrossFadeByParameterAction?.Invoke(musicIDs, parameter);
        public static void CallCrossFadeByParameterWithParam(string[] musicIDs, float[][] parameters, float parameter) => CrossFadeByParameterWithParamAction?.Invoke(musicIDs, parameters, parameter);
        public static void CallAddMeasureEvent(string musicIDs, UnityAction measureEvent) => AddMeasureEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemoveMeasureEvent(string musicIDs, UnityAction measureEvent) => RemoveMeasureEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddTimedEvent(string musicIDs, UnityAction measureEvent) => AddTimedEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemoveTimedEvent(string musicIDs, UnityAction measureEvent) => RemoveTimedEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddPlayEvent(string musicIDs, UnityAction measureEvent) => AddPlayEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemovePlayEvent(string musicIDs, UnityAction measureEvent) => RemovePlayEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddPauseEvent(string musicIDs, UnityAction measureEvent) => AddPauseEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemovePauseEvent(string musicIDs, UnityAction measureEvent) => RemovePauseEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddResumeEvent(string musicIDs, UnityAction measureEvent) => AddResumeEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemoveResumeEvent(string musicIDs, UnityAction measureEvent) => RemoveResumeEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddStopEvent(string musicIDs, UnityAction measureEvent) => AddStopEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemoveStopEvent(string musicIDs, UnityAction measureEvent) => RemoveStopEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallAddFinishedEvent(string musicIDs, UnityAction measureEvent) => AddFinishedEventAction?.Invoke(musicIDs, measureEvent);
        public static void CallRemoveFinishedEvent(string musicIDs, UnityAction measureEvent) => RemoveFinishedEventAction?.Invoke(musicIDs, measureEvent);

        #endregion
    }
}

