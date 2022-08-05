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
        #region var
        public AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        public AudioClip[] audioClips;
        private int currentIndex = -1;
        private int previousIndex = -1;
        public int IndexClip = 0;
        #endregion

        #region private functions
        private AudioClip NextClip()
        {
            if (audioClips.Length == 1)
            {
                return audioClips[0];
            }
            switch (sequenceMode)
            {
                case AudioSequenceMode.Sequential:
                    currentIndex++;
                    if (currentIndex >= audioClips.Length)
                    {
                        currentIndex = 0;
                    }
                    break;
                case AudioSequenceMode.Random:
                    do
                    {
                        currentIndex = UnityEngine.Random.Range(0, audioClips.Length);
                    }
                    while (currentIndex == previousIndex);
                    break;
                case AudioSequenceMode.Choise:
                    currentIndex = IndexClip = Mathf.Min(IndexClip, audioClips.Length - 1);
                    break;
            }

            previousIndex = currentIndex;

            return audioClips[currentIndex];
        }
        #endregion
        
        #region public functions
        public void ResetIndex()
        {
            currentIndex = -1;
            previousIndex = -1;
        }
        public AudioClip GetClip() => NextClip();
        #endregion
    }
}
