using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RoaREngine
{
    public class RoarManager : MonoBehaviour
    {
        #region var
        [SerializeField] private GameObject roarEmitter;
        [SerializeField] private int count;
        [SerializeField] private RoarContainersBank bank;
        [SerializeField] private List<AudioMixer> audiomixers;
        private List<RoarContainerSO> roarContainers;
        private List<GameObject> roarEmitters;
        private Dictionary<string, RoarContainerSO> containerDict = new Dictionary<string, RoarContainerSO>();
        private Dictionary<string, AudioMixer> audioMixerDict = new Dictionary<string, AudioMixer>();
        #endregion

        #region functions
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            SetContainersNames();
            ResetContainersBankIndex();
            SetInitialEmitters();
            SetMixersNames();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnChangeScene;
            CallPlay += Play;
            CallPause += Pause;
            CallResume += Resume;
            CallStop += Stop;
            CallStopAll += StopAll;
            CallPauseAll += PauseAll;
            CallResumeAll += ResumeAll;
            CallAddContainer += AddContainer;
            CallRemoveContainer += RemoveContainer;
            CallGetContainer += GetContainer;
            CallGetContainers += GetContainers;
            CallGetNumberContainers += GetNumberContainers;
            CallChangeSequenceMode += ChangeSequenceMode;
            CallGetAudioSource += GetAudioSource;
            CallGetNumberAudioSources += GetNumberAudioSources;
            CallAddEffect += AddEffect;
            //CallGetAudioSourceEffect += GetAudioSourceEffect;
            CallSetBankIndex += SetBankIndex;
            CallFade += Fade;
            CallCrossFadeByParameter += CrossFadeByParameter;
            CallCrossFadeByParameterWithParam += CrossFadeByParameterWithParam;
            CallAddMeasureEvent += AddMeasureEvent;
            CallRemoveMeasureEvent += RemoveMeasureEvent;
            CallAddTimedEvent += AddTimedEvent;
            CallRemoveTimedEvent += RemoveTimedEvent;
            CallAddPlayEvent += AddPlayEvent;
            CallRemovePlayEvent += RemovePlayEvent;
            CallAddPauseEvent += AddPauseEvent;
            CallRemovePauseEvent += RemovePauseEvent;
            CallAddResumeEvent += AddResumeEvent;
            CallRemoveResumeEvent += RemoveResumeEvent;
            CallAddStopEvent += AddStopEvent;
            CallRemoveStopEvent += RemoveStopEvent;
            CallAddFinishedEvent += AddFinishedEvent;
            CallRemoveFinishedEvent += RemoveFinishedEvent;
            CallGetAudioMixer += GetAudioMixer;
            CallGetAudioMixerParameter += GetAudioMixerParameter;
            CallSetAudioMixerParameter += SetAudioMixerParameter;
            CallChangeAudioMixerSnapshot += ChangeAudioMixerSnapshot;
            CallSetAudioMixerVolumeWithSlider += SetAudioMixerVolumeWithSlider;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnChangeScene;
            CallPlay -= Play;
            CallPause -= Pause;
            CallResume -= Resume;
            CallStop -= Stop;
            CallStopAll -= StopAll;
            CallPauseAll -= PauseAll;
            CallResumeAll -= ResumeAll;
            CallAddContainer -= AddContainer;
            CallRemoveContainer -= RemoveContainer;
            CallGetContainer -= GetContainer;
            CallGetContainers -= GetContainers;
            CallGetNumberContainers -= GetNumberContainers;
            CallChangeSequenceMode -= ChangeSequenceMode;
            CallGetAudioSource -= GetAudioSource;
            CallGetNumberAudioSources -= GetNumberAudioSources;
            CallAddEffect -= AddEffect;
            //CallGetAudioSourceEffect -= GetAudioSourceEffect;
            CallSetBankIndex -= SetBankIndex;
            CallFade -= Fade;
            CallCrossFadeByParameter -= CrossFadeByParameter;
            CallCrossFadeByParameterWithParam -= CrossFadeByParameterWithParam;
            CallAddMeasureEvent -= AddMeasureEvent;
            CallRemoveMeasureEvent -= RemoveMeasureEvent;
            CallAddTimedEvent -= AddTimedEvent;
            CallRemoveTimedEvent -= RemoveTimedEvent;
            CallAddPlayEvent -= AddPlayEvent;
            CallRemovePlayEvent -= RemovePlayEvent;
            CallAddPauseEvent -= AddPauseEvent;
            CallRemovePauseEvent -= RemovePauseEvent;
            CallAddResumeEvent -= AddResumeEvent;
            CallRemoveResumeEvent -= RemoveResumeEvent;
            CallAddStopEvent -= AddStopEvent;
            CallRemoveStopEvent -= RemoveStopEvent;
            CallAddFinishedEvent -= AddFinishedEvent;
            CallRemoveFinishedEvent -= RemoveFinishedEvent;
            CallGetAudioMixer -= GetAudioMixer;
            CallGetAudioMixerParameter -= GetAudioMixerParameter;
            CallSetAudioMixerParameter -= SetAudioMixerParameter;
            CallChangeAudioMixerSnapshot -= ChangeAudioMixerSnapshot;
            CallSetAudioMixerVolumeWithSlider -= SetAudioMixerVolumeWithSlider;
        }

        private void OnChangeScene(Scene arg0, LoadSceneMode arg1)
        {
            bank = FindObjectOfType(typeof(RoarContainersBank)) as RoarContainersBank;
            SetContainersNames();
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

        private GameObject GetEmitterFromPool()
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

        private void SetMixersNames()
        {
            if (audiomixers.Count > 0)
            {
                foreach (AudioMixer audiomixer in audiomixers)
                {
                    audioMixerDict[audiomixer.name] = audiomixer;
                }
            }
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
                roarEmitter = GetEmitterFromPool();
                if (roarEmitter != null)
                {
                    SetContainer(musicID, roarEmitter);
                    RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
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
                    RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
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
                    RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
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
                    RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                    emitterComponent.Resume();
                }
            }
        }

        private void StopAll()
        {
            List<RoarEmitter> emitters = GetActiveEmitters();
            foreach (RoarEmitter emitter in emitters)
            {
                emitter.Stop();
            }
        }

        private void PauseAll()
        {
            List<RoarEmitter> emitters = GetActiveEmitters();
            foreach (RoarEmitter emitter in emitters)
            {
                emitter.Pause();
            }
        }

        private void ResumeAll()
        {
            List<RoarEmitter> emitters = GetActiveEmitters();
            foreach (RoarEmitter emitter in emitters)
            {
                emitter.Resume();
            }
        }

        private void SetContainersNames()
        {
            containerDict.Clear();
            roarContainers = bank.ContainersBank.RoarContainers;
            foreach (RoarContainerSO container in roarContainers)
            {
                containerDict[container.Name] = container;
            }
        }

        private void ResetContainersBankIndex()
        {
            foreach (RoarContainerSO container in roarContainers)
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
                RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
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

        private List<RoarEmitter> GetActiveEmitters()
        {
            List<RoarEmitter> activeEmitters = new List<RoarEmitter>();
            foreach (GameObject roarEmitter in roarEmitters)
            {
                if (roarEmitter.gameObject.activeInHierarchy == true)
                {
                    activeEmitters.Add(roarEmitter.GetComponent<RoarEmitter>());
                }
            }
            return activeEmitters;
        }

        private RoarEmitter GetEmitter(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = GetActiveEmitterObject(musicID);
                if (roarEmitter != null)
                {
                    return roarEmitter.GetComponent<RoarEmitter>();
                }
            }
            return null;
        }

        private void AddContainer(RoarContainerSO container)
        {
            roarContainers.Add(container);
            containerDict[container.Name] = container;
        }

        private void RemoveContainer(RoarContainerSO container)
        {
            roarContainers.Remove(container);
            containerDict.Remove(container.Name);
        }

        private RoarContainerSO GetContainer(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                return containerDict[musicID];
            }
            return null;
        }

        private void SetContainer(string musicID, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoarEmitter>().SetContainer(containerDict[musicID]);
        }

        private List<RoarContainerSO> GetContainers()
        {
            return roarContainers;
        }

        private void ChangeSequenceMode(string musicID, AudioSequenceMode audioSequenceMode)
        {
            RoarContainerSO container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.sequenceMode = audioSequenceMode;
            }
        }

        private AudioSource GetAudioSource(string musicID)
        {
            RoarEmitter emitterComponent = GetEmitter(musicID);
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
                RoarEmitter emitterComponent = emitter.GetComponent<RoarEmitter>();
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
            RoarContainerSO container = GetContainer(musicID);
            if (container != null)
            {
                container.roarClipBank.IndexClip = Mathf.Min(value, container.roarClipBank.audioClips.Length);
            }
        }

        private void Fade(string musicID, float fadeTime, float finalVolume)
        {
            RoarEmitter emitter = GetEmitter(musicID);
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
                    GetAudioSource(musicsID[i]).volume = RoarTrackInfo.Remap(param, fadeInParamValueStart, fadeInParamValueEnd);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - RoarTrackInfo.Remap(param, fadeOutParamValueStart, fadeOutParamValueEnd);
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
                    GetAudioSource(musicsID[i]).volume = RoarTrackInfo.Remap(param, crossFadeInput[i][0], crossFadeInput[i][1]);
                }
                else
                {
                    GetAudioSource(musicsID[i]).volume = 1f - RoarTrackInfo.Remap(param, crossFadeInput[i][2], crossFadeInput[i][3]);
                }
            }
        }

        private void AddMeasureEvent(string musicID, UnityAction measureAction)
        {
            if (MusicIDIsValid(musicID))
            {
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
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
                RoarContainerSO container = GetContainer(musicID);
                if (container != null)
                {
                    container.OnFinishedEvent -= finishEvent;
                }
            }
        }

        private float GetAudioMixerParameter(string audioMixerName, string parameter)
        {
            bool value = audioMixerDict[audioMixerName].GetFloat(parameter, out float param);
            if (value)
            {
                return param;
            }
            else
            {
                return float.MinValue;
            }
        }

        private void SetAudioMixerParameter(string audioMixerName, string parameter, float volume)
        {
            audioMixerDict[audioMixerName].SetFloat(parameter, volume);
        }

        private void ChangeAudioMixerSnapshot(string audioMixerName, string snapshot, float time)
        {
            audioMixerDict[audioMixerName].FindSnapshot(snapshot).TransitionTo(time);
        }

        private AudioMixer GetAudioMixer(string audioMixerName)
        {
            return audioMixerDict[audioMixerName];
        }

        private void SetAudioMixerVolumeWithSlider(string audioMixerName, string volumeNameParameter,float volume)
        {
            audioMixerDict[audioMixerName].SetFloat(volumeNameParameter, NormalizedMixerValue(volume));
        }

        private float NormalizedMixerValue(float normalizedValue) => Mathf.Log10(normalizedValue) * 20f;
        #endregion

        #region delegate
        public static UnityAction<string, bool> CallPlay;
        public static UnityAction<string> CallPause;
        public static UnityAction<string> CallResume;
        public static UnityAction<string> CallStop;
        public static UnityAction CallStopAll;
        public static UnityAction CallPauseAll;
        public static UnityAction CallResumeAll;
        public static UnityAction<RoarContainerSO> CallAddContainer;
        public static UnityAction<RoarContainerSO> CallRemoveContainer;
        public static UnityAction<string, AudioSequenceMode> CallChangeSequenceMode;
        public static UnityAction<string, EffectType> CallAddEffect;
        public static UnityAction<string, int> CallSetBankIndex;
        public static UnityAction<string, float, float> CallFade;
        public static UnityAction<string[], float> CallCrossFadeByParameter;
        public static UnityAction<string[], float[][], float> CallCrossFadeByParameterWithParam;
        public static UnityAction<string, UnityAction> CallAddMeasureEvent;
        public static UnityAction<string, UnityAction> CallRemoveMeasureEvent;
        public static UnityAction<string, UnityAction> CallAddTimedEvent;
        public static UnityAction<string, UnityAction> CallRemoveTimedEvent;
        public static UnityAction<string, UnityAction> CallAddPlayEvent;
        public static UnityAction<string, UnityAction> CallRemovePlayEvent;
        public static UnityAction<string, UnityAction> CallAddPauseEvent;
        public static UnityAction<string, UnityAction> CallRemovePauseEvent;
        public static UnityAction<string, UnityAction> CallAddResumeEvent;
        public static UnityAction<string, UnityAction> CallRemoveResumeEvent;
        public static UnityAction<string, UnityAction> CallAddStopEvent;
        public static UnityAction<string, UnityAction> CallRemoveStopEvent;
        public static UnityAction<string, UnityAction> CallAddFinishedEvent;
        public static UnityAction<string, UnityAction> CallRemoveFinishedEvent;
        public static UnityAction<string, string, float> CallSetAudioMixerParameter;
        public static UnityAction<string, string, float> CallChangeAudioMixerSnapshot;
        public static UnityAction<string, string, float> CallSetAudioMixerVolumeWithSlider;
        public static Func<string, RoarContainerSO> CallGetContainer;
        public static Func<List<RoarContainerSO>> CallGetContainers;
        public static Func<int> CallGetNumberContainers;
        public static Func<string, AudioSource> CallGetAudioSource;
        public static Func<int> CallGetNumberAudioSources;
        public static Func<string> CallGetAudioSourceEffect;
        public static Func<string, AudioMixer> CallGetAudioMixer;
        public static Func<string, string, float> CallGetAudioMixerParameter;
        #endregion
    }
}

