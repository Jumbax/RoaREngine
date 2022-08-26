using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoaRContainer", menuName = "RoaREngine/RoaRContainer")]
    public class RoaRContainerSO : ScriptableObject
    {
        #region var
        public string Name;
        public AudioClip Clip { get => roarClipBank.GetClip();}
        public RoaRClipsBankSO roarClipBank;
        public RoaRConfigurationSO roarConfiguration;
        public UnityAction OnPlayEvent;
        public UnityAction OnPauseEvent;
        public UnityAction OnStopEvent;
        public UnityAction OnResumeEvent;
        public UnityAction OnFinishedEvent;
        public UnityAction MeasureEvent;
        public UnityAction TimedEvent;
        #endregion

        #region public functions
        public void ResetBankIndex()
        {
            roarClipBank.ResetIndex();
        }

        public void SetConfiguration(AudioSource audioSource, RoaREmitter roarEmitter)
        {
            roarConfiguration.ApplyTo(audioSource, roarEmitter);
        }
        #endregion
    }
}
