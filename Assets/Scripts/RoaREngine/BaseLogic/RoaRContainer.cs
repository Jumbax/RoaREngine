using UnityEngine;
using UnityEngine.Events;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoaRContainer", menuName = "RoaREngine/RoaRContainer")]
    public class RoaRContainer : ScriptableObject
    {
        public string Name;
        public AudioClip Clip { get => roarClipBank.GetClip();}
        public RoaRClipsBankSO roarClipBank;
        public RoaRConfigurationSO roarConfiguration;
        public UnityAction MeasureEvent;
        public UnityAction MarkerEvent;

        public void ResetBankIndex()
        {
            roarClipBank.ResetIndex();
        }

        public void SetConfiguration(AudioSource audioSource)
        {
            roarConfiguration.ApplyTo(audioSource);
        }
    }
}
