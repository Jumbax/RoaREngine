using UnityEngine;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoaRContainer", menuName = "RoaREngine/RoaRContainer")]
    public class RoaRContainer : ScriptableObject
    {
        public string Name;
        public AudioClip Clip { get => roarClipBank.GetClip();}
        public RoaRClipsBankSO roarClipBank;
        public RoaRConfigurationSO roarConfiguration;

        public void SetConfiguration(AudioSource audioSource)
        {
            roarConfiguration.ApplyTo(audioSource);
        }
    }
}
