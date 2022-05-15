using UnityEngine;

namespace RoaREngine
{
    public class RoaREmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private RoaRContainer container;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetContainer(RoaRContainer otherContainer)
        {
            container = otherContainer;
            audioSource.clip = container.Clip;
        }

        public bool CheckForContainerName(string containerName)
        {
            return container.Name == containerName;
        }

        public void Play()
        {
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void Resume()
        {
            audioSource.UnPause();
        }
    }
}
