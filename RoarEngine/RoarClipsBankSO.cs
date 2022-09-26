using UnityEngine;

namespace RoaREngine
{

    #region enum
    public enum AudioSequenceMode
    {
        Sequential,
        Random,
        Choice
    }
    #endregion

    [CreateAssetMenu(fileName = "RoarClipsBank", menuName = "RoarEngine/RoarClipsBank")]
    public class RoarClipsBankSO : ScriptableObject
    {
        #region var
        public AudioSequenceMode sequenceMode = AudioSequenceMode.Sequential;
        public AudioClip[] audioClips;
        private int currentIndex = -1;
        private int previousIndex = -1;
        public int ClipIndex = 0;
        #endregion
        
        #region public functions
        public void ResetIndex()
        {
            currentIndex = -1;
            previousIndex = -1;
        }
        public AudioClip GetClip()
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
                case AudioSequenceMode.Choice:
                    currentIndex = ClipIndex = Mathf.Min(ClipIndex, audioClips.Length - 1);
                    break;
            }

            previousIndex = currentIndex;

            return audioClips[currentIndex];
        }
        #endregion
    }
}
