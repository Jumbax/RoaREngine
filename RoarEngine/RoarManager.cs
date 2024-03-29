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
        public static RoarManager Instance;
        #region var
        [SerializeField] private GameObject roarEmitter;
        [SerializeField] private int count;
        [SerializeField] private RoarContainersBank containersBank;
        [SerializeField] private List<AudioMixer> audiomixers;
        private List<RoarContainerSO> roarContainers;
        private List<GameObject> roarEmitters;
        private Dictionary<string, RoarContainerSO> containerDict = new Dictionary<string, RoarContainerSO>();
        private Dictionary<string, AudioMixer> audioMixerDict = new Dictionary<string, AudioMixer>();
        #endregion

        #region functions
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(transform.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(transform.gameObject);
            }
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
            CallSetParent += SetParent;
            CallResetParent += ResetParent;
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
            CallSetParent -= SetParent;
            CallResetParent -= ResetParent;
        }

        private void OnChangeScene(Scene arg0, LoadSceneMode arg1)
        {
            containersBank = FindObjectOfType(typeof(RoarContainersBank)) as RoarContainersBank;
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
                if (roarEmitters[i] != null)
                {
                    if (!roarEmitters[i].activeInHierarchy)
                    {
                        roarEmitters[i].SetActive(true);
                        return roarEmitters[i];
                    }
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

        private void Play(string containerName, Transform parent)
        {
            if (GetContainer(containerName).roarClipBank.audioClips.Length <= 0)
            {
                Debug.LogError("AudioClip not found. Container must have at least one audioclip");
                return;
            }
            GameObject roarEmitter;
            if (GetContainer(containerName).roarConfiguration.esclusive)
            {
                roarEmitter = GetActiveEmitter(containerName);
                if (roarEmitter != null)
                {
                    Stop(containerName, parent);
                }
            }
            roarEmitter = GetEmitterFromPool();
            if (roarEmitter != null)
            {
                SetContainer(containerName, roarEmitter);
                RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                if (parent != null)
                {
                    emitterComponent.SetParent(parent);
                }
                emitterComponent.Play();
            }
            else
            {
                Debug.LogError("The emitters pool cannot get and activate one");
            }
        }

        private void Stop(string containerName, Transform parent)
        {
            RoarEmitter emitterComponent;
            if (parent != null)
            {
                emitterComponent = parent.GetComponentInChildren<RoarEmitter>();
                if (emitterComponent != null)
                {
                    emitterComponent.Stop();
                    return;
                }
            }
            GameObject roarEmitter = GetActiveEmitter(containerName);
            if (roarEmitter != null)
            {
                emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                emitterComponent.Stop();
            }
        }

        private void Pause(string containerName, Transform parent)
        {
            RoarEmitter emitterComponent;
            if (parent != null)
            {
                emitterComponent = parent.GetComponentInChildren<RoarEmitter>();
                if (emitterComponent != null)
                {
                    emitterComponent.Pause();
                    return;
                }
            }
            GameObject roarEmitter = GetActiveEmitter(containerName);
            if (roarEmitter != null)
            {
                emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                emitterComponent.Pause();
            }
        }

        private void Resume(string containerName, Transform parent)
        {
            RoarEmitter emitterComponent;
            if (parent != null)
            {
                emitterComponent = parent.GetComponentInChildren<RoarEmitter>();
                if (emitterComponent != null)
                {
                    emitterComponent.Resume();
                    return;
                }
            }
            GameObject roarEmitter = GetActiveEmitter(containerName);
            if (roarEmitter != null)
            {
                emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                emitterComponent.Resume();
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
            roarContainers = containersBank.ContainersBank.RoarContainers;
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

        private bool ContainerNameIsValid(string containerName)
        {
            if (containerDict.ContainsKey(containerName))
            {
                return true;
            }
            else
            {
                Debug.LogError("ContainerName not found", this);
                return false;
            }
        }

        private GameObject GetEmitter(string containerName)
        {
            if (ContainerNameIsValid(containerName))
            {
                foreach (GameObject roarEmitter in roarEmitters)
                {
                    if (roarEmitter != null)
                    {
                        RoarEmitter emitterComponent = roarEmitter.GetComponent<RoarEmitter>();
                        if (emitterComponent.CheckForContainerName(containerName))
                        {
                            return roarEmitter;
                        }
                    }
                }
            }
            return null;
        }

        private GameObject GetActiveEmitter(string containerName)
        {
            GameObject emitter = GetEmitter(containerName);
            if (emitter != null)
            {
                if (emitter.activeInHierarchy)
                {
                    return emitter;
                }
            }
            return null;
        }

        private List<RoarEmitter> GetActiveEmitters()
        {
            List<RoarEmitter> activeEmitters = new List<RoarEmitter>();
            foreach (GameObject roarEmitter in roarEmitters)
            {
                if (roarEmitter != null)
                {
                    if (roarEmitter.gameObject.activeInHierarchy == true)
                    {
                        activeEmitters.Add(roarEmitter.GetComponent<RoarEmitter>());
                    }
                }
            }
            return activeEmitters;
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

        private RoarContainerSO GetContainer(string containerName)
        {
            if (ContainerNameIsValid(containerName))
            {
                return containerDict[containerName];
            }
            return null;
        }

        private void SetContainer(string containerName, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoarEmitter>().SetContainer(containerDict[containerName]);
        }

        private List<RoarContainerSO> GetContainers()
        {
            return roarContainers;
        }

        private void ChangeSequenceMode(string containerName, AudioSequenceMode audioSequenceMode)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.roarClipBank.sequenceMode = audioSequenceMode;
            }
        }

        private AudioSource GetAudioSource(string containerName)
        {

            GameObject emitter = GetActiveEmitter(containerName);
            RoarEmitter emitterComponent = null;
            if (emitter != null)
            {
                emitterComponent = emitter.GetComponent<RoarEmitter>();
            }
            if (emitterComponent != null)
            {
                return emitterComponent.GetAudioSource();
            }
            return null;
        }

        private void SetParent(string containerName, Transform parent)
        {
            GameObject emitter = GetEmitter(containerName);
            if (emitter != null)
            {
                RoarEmitter emitterComponent = emitter.GetComponent<RoarEmitter>();
                emitterComponent.SetParent(parent);
            }
        }

        private void ResetParent(string containerName)
        {
            GameObject emitter = GetEmitter(containerName);
            if (emitter != null)
            {
                RoarEmitter emitterComponent = emitter.GetComponent<RoarEmitter>();
                emitterComponent.ResetParent();
            }
        }

        private void AddEffect(string containerName, EffectType type)
        {
            GameObject emitter = GetEmitter(containerName);
            if (emitter != null)
            {
                RoarEmitter emitterComponent = emitter.GetComponent<RoarEmitter>();
                emitterComponent.AddEffect(type);
            }
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

        private void SetBankIndex(string containerName, int bankIndex)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.roarClipBank.ClipIndex = Mathf.Min(bankIndex, container.roarClipBank.audioClips.Length);
            }
        }

        private void Fade(string containerName, float fadeTime, float finalVolume)
        {
            RoarEmitter emitter = GetActiveEmitter(containerName).GetComponent<RoarEmitter>();
            if (emitter != null)
            {
                emitter.Fade(fadeTime, finalVolume);
            }
        }

        private void CrossFadeByParameter(string[] containersNames, float param)
        {
            for (int i = 0; i < containersNames.Length; i++)
            {
                float fadeInParamValueStart = GetContainer(containersNames[i]).roarConfiguration.fadeInParamValueStart;
                float fadeInParamValueEnd = GetContainer(containersNames[i]).roarConfiguration.fadeInParamValueEnd;
                float fadeOutParamValueStart = GetContainer(containersNames[i]).roarConfiguration.fadeOutParamValueStart;
                float fadeOutParamValueEnd = GetContainer(containersNames[i]).roarConfiguration.fadeOutParamValueEnd;

                if (param < fadeInParamValueStart)
                {
                    GetAudioSource(containersNames[i]).volume = 0f;
                    continue;
                }
                if (param >= fadeInParamValueStart && param <= fadeInParamValueEnd)
                {
                    if (fadeInParamValueStart == 0 && fadeInParamValueEnd == 0)
                    {
                        GetAudioSource(containersNames[i]).volume = 1f;
                        continue;
                    }
                    GetAudioSource(containersNames[i]).volume = RoarTrackInfo.Remap(param, fadeInParamValueStart, fadeInParamValueEnd);
                }
                else
                {
                    GetAudioSource(containersNames[i]).volume = 1f - RoarTrackInfo.Remap(param, fadeOutParamValueStart, fadeOutParamValueEnd);
                }
            }
        }

        private void CrossFadeByParameterWithParam(string[] containersNames, float[][] crossFadeInput, float param)
        {
            for (int i = 0; i < containersNames.Length; i++)
            {
                if (param < crossFadeInput[i][0])
                {
                    GetAudioSource(containersNames[i]).volume = 0f;
                    continue;
                }
                if (param >= crossFadeInput[i][0] && param <= crossFadeInput[i][1])
                {
                    if (crossFadeInput[i][0] == 0 && crossFadeInput[i][1] == 0)
                    {
                        GetAudioSource(containersNames[i]).volume = 1f;
                        continue;
                    }
                    GetAudioSource(containersNames[i]).volume = RoarTrackInfo.Remap(param, crossFadeInput[i][0], crossFadeInput[i][1]);
                }
                else
                {
                    GetAudioSource(containersNames[i]).volume = 1f - RoarTrackInfo.Remap(param, crossFadeInput[i][2], crossFadeInput[i][3]);
                }
            }
        }

        private void AddMeasureEvent(string containerName, UnityAction measureAction)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.MeasureEvent += measureAction;
            }
        }

        private void RemoveMeasureEvent(string containerName, UnityAction measureAction)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.MeasureEvent -= measureAction;
            }
        }

        private void AddTimedEvent(string containerName, UnityAction markerAction)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.TimedEvent += markerAction;
            }
        }

        private void RemoveTimedEvent(string containerName, UnityAction markerAction)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.TimedEvent -= markerAction;
            }
        }

        private void AddPlayEvent(string containerName, UnityAction playEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnPlayEvent += playEvent;
            }
        }

        private void RemovePlayEvent(string containerName, UnityAction playEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnPlayEvent -= playEvent;
            }
        }

        private void AddPauseEvent(string containerName, UnityAction pauseEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnPauseEvent += pauseEvent;
            }
        }

        private void RemovePauseEvent(string containerName, UnityAction pauseEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnPauseEvent -= pauseEvent;
            }
        }

        private void AddResumeEvent(string containerName, UnityAction resumeEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnResumeEvent += resumeEvent;
            }
        }

        private void RemoveResumeEvent(string containerName, UnityAction resumeEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnResumeEvent -= resumeEvent;
            }
        }

        private void AddStopEvent(string containerName, UnityAction stopEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnStopEvent += stopEvent;
            }
        }

        private void RemoveStopEvent(string containerName, UnityAction stopEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnStopEvent -= stopEvent;
            }
        }

        private void AddFinishedEvent(string containerName, UnityAction finishEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnFinishedEvent += finishEvent;
            }
        }

        private void RemoveFinishedEvent(string containerName, UnityAction finishEvent)
        {
            RoarContainerSO container = GetContainer(containerName);
            if (container != null)
            {
                container.OnFinishedEvent -= finishEvent;
            }
        }

        private bool AudioMixerNameIsValid(string audioMixerName)
        {
            if (audioMixerDict.ContainsKey(audioMixerName))
            {
                return true;
            }
            else
            {
                Debug.LogError("AudioMixerName not found");
                return false;
            }
        }

        private float GetAudioMixerParameter(string audioMixerName, string parameter)
        {
            if (AudioMixerNameIsValid(audioMixerName))
            {
                bool value = audioMixerDict[audioMixerName].GetFloat(parameter, out float param);
                if (value)
                {
                    return param;
                }
                else
                {
                    Debug.LogError("Mixer Parameter Name not found");
                }
            }
            return float.MinValue;
        }

        private void SetAudioMixerParameter(string audioMixerName, string parameter, float parameterValue)
        {
            if (AudioMixerNameIsValid(audioMixerName))
            {
                bool value = audioMixerDict[audioMixerName].GetFloat(parameter, out float param);
                if (value)
                {
                    audioMixerDict[audioMixerName].SetFloat(parameter, parameterValue);
                }
                else
                {
                    Debug.LogError("Mixer Parameter Name not found");
                }
            }
        }

        private void ChangeAudioMixerSnapshot(string audioMixerName, string snapshot, float time)
        {
            if (AudioMixerNameIsValid(audioMixerName))
            {
                if (audioMixerDict[audioMixerName].FindSnapshot(snapshot))
                {
                    audioMixerDict[audioMixerName].FindSnapshot(snapshot).TransitionTo(time);
                }
                else
                {
                    Debug.LogError("Snapshot Name not found");
                }
            }
        }

        private AudioMixer GetAudioMixer(string audioMixerName)
        {
            if (AudioMixerNameIsValid(audioMixerName))
            {
                return audioMixerDict[audioMixerName];
            }
            return null;
        }

        private void SetAudioMixerVolumeWithSlider(string audioMixerName, string volumeNameParameter, float volume)
        {
            if (AudioMixerNameIsValid(audioMixerName))
            {
                bool value = audioMixerDict[audioMixerName].GetFloat(volumeNameParameter, out float param);
                if (value)
                {
                    audioMixerDict[audioMixerName].SetFloat(volumeNameParameter, RoarTrackInfo.NormalizedMixerValue(volume));
                }
                else
                {
                    Debug.LogError("Mixer Parameter Name not found");
                }
            }
        }
        #endregion

        #region delegate
        public static UnityAction<string, Transform> CallPlay;
        public static UnityAction<string, Transform> CallPause;
        public static UnityAction<string, Transform> CallResume;
        public static UnityAction<string, Transform> CallStop;
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
        public static UnityAction<string, Transform> CallSetParent;
        public static UnityAction<string> CallResetParent;
        public static Func<string, RoarContainerSO> CallGetContainer;
        public static Func<List<RoarContainerSO>> CallGetContainers;
        public static Func<int> CallGetNumberContainers;
        public static Func<string, AudioSource> CallGetAudioSource;
        public static Func<int> CallGetNumberAudioSources;
        public static Func<string, AudioMixer> CallGetAudioMixer;
        public static Func<string, string, float> CallGetAudioMixerParameter;
        #endregion
    }
}

