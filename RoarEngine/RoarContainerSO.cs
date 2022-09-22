using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoarContainer", menuName = "RoarEngine/RoarContainer")]
    public class RoarContainerSO : ScriptableObject
    {
        #region var
        public string Name;
        public AudioClip Clip { get => roarClipBank.GetClip();}
        public RoarClipsBankSO roarClipBank;
        public RoarConfigurationSO roarConfiguration;
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

        public void SetConfiguration(AudioSource audioSource)
        {
            roarConfiguration.ApplyTo(audioSource);
        }
        #endregion
    }
}
