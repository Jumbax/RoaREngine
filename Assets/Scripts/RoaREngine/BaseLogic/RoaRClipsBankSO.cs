using UnityEngine;

namespace RoaREngine
{
    public enum AudioSequenceMode
    {
        Sequential,
        Random,
        Choise
    }

    [CreateAssetMenu(fileName = "RoaRClipsBank", menuName = "RoaREngine/RoaRClipsBank")]
    public class RoaRClipsBankSO : ScriptableObject
    {
        public AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        public AudioClip[] audioClips;
        private int currentIndex = -1;
        private int previousIndex = -1;
        private int index;

        private AudioClip NextClip()
        {
            if (audioClips.Length == 1) return audioClips[0];

            switch (sequenceMode)
            {
                case AudioSequenceMode.Sequential:
                    currentIndex = (int)Mathf.Repeat(++currentIndex, audioClips.Length);
                    break;
                case AudioSequenceMode.Random:
                    do
                    {
                        currentIndex = UnityEngine.Random.Range(0, audioClips.Length);
                    }
                    while (currentIndex == previousIndex);
                    break;
                case AudioSequenceMode.Choise:
                    currentIndex = index;
                    break;
            }

            previousIndex = currentIndex;

            return audioClips[currentIndex];
        }

        public AudioClip GetClip() => NextClip();

        public int GetClipIndex() => index;

        public void SetClipIndex(int index) => this.index = index;

    }
}
