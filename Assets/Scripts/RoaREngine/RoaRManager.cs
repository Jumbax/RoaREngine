using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    public enum EffectType
    {
        Chorus,
        Distortion,
        Echo,
        HF,
        LP,
        ReverbFilter,
        ReverbZone
    }

    public class RoaRManager : MonoBehaviour
    {
        private RoaRPooler roarPooler;
        public RoaRContainerMap RoarContainerMap = default;

        private void Start()
        {
            roarPooler = GetComponent<RoaRPooler>();
            RoarContainerMap.SetNames();
        }

        public void Play(string musicID, float fadeTime = 0f, float finalVolume = 0f, bool randomStartTime = false, float startTime = 0f, Transform parent = null, float minRandomXYZ = 0f, float maxRandomXYZ = 0f)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = roarPooler.Get();
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

        public AudioSource SetAudioSourceProperty(string musicID)
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
                switch (type)
                {
                    case EffectType.Chorus:
                        emitter.AddComponent<AudioChorusFilter>();
                        break;
                    case EffectType.Distortion:
                        emitter.AddComponent<AudioDistortionFilter>();
                        break;
                    case EffectType.Echo:
                        emitter.AddComponent<AudioEchoFilter>();
                        break;
                    case EffectType.HF:
                        emitter.AddComponent<AudioHighPassFilter>();
                        break;
                    case EffectType.LP:
                        emitter.AddComponent<AudioLowPassFilter>();
                        break;
                    case EffectType.ReverbFilter:
                        emitter.AddComponent<AudioReverbFilter>();
                        break;
                    case EffectType.ReverbZone:
                        emitter.AddComponent<AudioReverbZone>();
                        break;
                }
            }
        }

        public T SetEffectProperty<T>(string musicID, EffectType type)
        {
            AudioChorusFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioChorusFilter>();
            if (filter != null)
            {
                return (T)(object)filter;
            }
            return (T)(object)null;
        }
        public AudioChorusFilter SetChorusFilterProperty(string musicID)
        {
            AudioChorusFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioChorusFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;
        }

        public AudioDistortionFilter SetDistortionFilterProperty(string musicID)
        {
            AudioDistortionFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioDistortionFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;
        }

        public AudioEchoFilter SetEchoFilterProperty(string musicID)
        {
            AudioEchoFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioEchoFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;
        }

        public AudioHighPassFilter SetHPFilterProperty(string musicID)
        {
            AudioHighPassFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioHighPassFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;
        }

        public AudioLowPassFilter SetLPFilterProperty(string musicID)
        {
            AudioLowPassFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioLowPassFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;
        }
        
        public AudioReverbFilter SetReverbFilterProperty(string musicID)
        {
            AudioReverbFilter filter = SetAudioSourceProperty(musicID).GetComponent<AudioReverbFilter>();
            if (filter != null)
            {
                return filter;
            }
            return null;            
        }

        public AudioReverbZone SetReverbZoneProperty(string musicID)
        {
            AudioReverbZone zone = SetAudioSourceProperty(musicID).GetComponent<AudioReverbZone>();
            if (zone != null)
            {
                return zone;
            }
            return null;
        }
    }
}

