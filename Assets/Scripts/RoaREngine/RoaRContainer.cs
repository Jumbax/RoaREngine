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

        public void SetConfiguration(AudioSource audioSource, GameObject roarEmitter)
        {
            roarConfiguration.ApplyTo(audioSource, roarEmitter);
        }
    }
}
