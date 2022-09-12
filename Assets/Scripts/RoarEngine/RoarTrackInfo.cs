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

        public static double GetTrackSemiquaverLength(int bpm, int tempo) => GetTrackQuarterLength(bpm) / tempo;

        public static double GetTrackBarLength(int bpm, int tempoL, int tempoR) => (GetTrackQuarterLength(bpm) * tempoL) * (tempoL / tempoR);

        public static double GetTimeElapsed(AudioSource audioSource) => (double)audioSource.timeSamples / audioSource.clip.frequency;
 
        public static double GetTimeRemainder(AudioSource audioSource, int bpm, int tempoL, int tempoR) => GetTimeElapsed(audioSource) % GetTrackBarLength(bpm, tempoL, tempoR);

        public static double GetTimeBeforeNextBar(AudioSource audioSource, int bpm, int tempoL, int tempoR)
        {
            double barLength = GetTrackBarLength(bpm, tempoL, tempoR);
            double remainder = GetTimeRemainder(audioSource, bpm, tempoL, tempoR);
            return barLength - remainder;
        }
        #endregion
    }
}
