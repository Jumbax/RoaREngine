using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    public static class RoaREventChannel
    {
        public static UnityAction<string, bool> Play;
        public static UnityAction<string> Pause;
        public static UnityAction<string> Resume;
        public static UnityAction<string> Stop;
        public static UnityAction<RoaRContainer> AddContainer;
        public static UnityAction<RoaRContainer> RemoveContainer;
        public static Func <string, RoaRContainer> GetContainer;
        public static Func<List<RoaRContainer>> GetContainers;
        public static Func<int> GetNumberContainers;
        public static UnityAction<string, GameObject> SetContainer;
        public static UnityAction<string, AudioSequenceMode> ChangeSequenceMode;
        public static Func<string, AudioSource> GetAudioSource;
        public static Func<int> GetNumberAudioSources;
        public static UnityAction<string, EffectType> AddEffect;
        public static Func<string, Behaviour> GetAudioSourceEffect;
        public static UnityAction<string, int> SetBankIndex;
        public static UnityAction<string, float, float> Fade;
        public static UnityAction<string[], float> CrossFadeByParameter;
        public static UnityAction<string[], float[][], float> CrossFadeByParameterWithParam;
        public static UnityAction<string, UnityAction> AddMeasureEvent;
        public static UnityAction<string, UnityAction> RemoveMeasureEvent;
        public static UnityAction<string, UnityAction> AddTimedEvent;
        public static UnityAction<string, UnityAction> RemoveTimedEvent;
        public static UnityAction<string, UnityAction> AddPlayEvent;
        public static UnityAction<string, UnityAction> RemovePlayEvent;
        public static UnityAction<string, UnityAction> AddPauseEvent;
        public static UnityAction<string, UnityAction> RemovePauseEvent;
        public static UnityAction<string, UnityAction> AddResumeEvent;
        public static UnityAction<string, UnityAction> RemoveResumeEvent;
        public static UnityAction<string, UnityAction> AddStopEvent;
        public static UnityAction<string, UnityAction> RemoveStopEvent;
        public static UnityAction<string, UnityAction> AddFinishedEvent;
        public static UnityAction<string, UnityAction> RemoveFinishedEvent;
    }
}
