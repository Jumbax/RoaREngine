using UnityEngine;

namespace RoaREngine
{
    public static class RoarTrackInfo
    {
        #region public functions
        public static float Remap(float value, float min, float max) => (value - min) * 1f / (max - min);

        public static float NormalizedMixerValue(float normalizedValue) => Mathf.Log10(normalizedValue) * 20f;

        public static double GetTrackDuration(AudioClip clip) => (double)clip.samples / clip.frequency;

        public static double GetTrackQuarterLength(int bpm) => 60d / bpm;

        public static double GetTrackSemiquaverLength(int bpm, int beats) => GetTrackQuarterLength(bpm) / beats;

        public static double GetTrackBarLength(int bpm, int beats, int measure) => (GetTrackQuarterLength(bpm) * beats) * (beats / measure);

        public static double GetTimeElapsed(AudioSource audioSource) => (double)audioSource.timeSamples / audioSource.clip.frequency;
 
        public static double GetTimeRemainder(AudioSource audioSource, int bpm, int beats, int measure) => GetTimeElapsed(audioSource) % GetTrackBarLength(bpm, beats, measure);

        public static double GetTimeBeforeNextBar(AudioSource audioSource, int bpm, int beats, int measure)
        {
            double barLength = GetTrackBarLength(bpm, beats, measure);
            double remainder = GetTimeRemainder(audioSource, bpm, beats, measure);
            return barLength - remainder;
        }
        #endregion
    }
}
