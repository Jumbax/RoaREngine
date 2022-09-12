using UnityEngine;

namespace RoaREngine
{
    public static class RoarTrackInfo
    {
        #region public functions
        public static float Remap(float value, float min, float max)
        {
            return (value - min) * 1f / (max - min);
        }

        public static double GetTrackDuration(AudioClip clip)
        {
            // Track Duration = samples / freq
            return (double)clip.samples / clip.frequency;
        }

        public static double GetTrackQuarterLength(int bpm)
        {
            // Beat length = 60/bpm track
            return 60d / bpm;
        }

        public static double GetTrackSemiquaverLength(int bpm, int tempo)
        {
            // Semiquaver note length = 60/bpm track in 4/4:
            return GetTrackQuarterLength(bpm) / tempo;
        }

        public static double GetTrackBarLength(int bpm, int tempoL, int tempoR)
        {
            //Bar length = 60/bpm * tempo in 4/8:
            return (GetTrackQuarterLength(bpm) * tempoL) * (tempoL / tempoR);
        }

        public static double GetTimeElapsed(AudioSource audioSource)
        {
            // TimeElapsed = timeSamples / freq
            return (double)audioSource.timeSamples / audioSource.clip.frequency;
        }

        public static double GetTimeRemainder(AudioSource audioSource, int bpm, int tempoL, int tempoR)
        {
            return GetTimeElapsed(audioSource) % GetTrackBarLength(bpm, tempoL, tempoR);
        }

        public static double GetTimeBeforeNextBar(AudioSource audioSource, int bpm, int tempoL, int tempoR)
        {
            double barLength = GetTrackBarLength(bpm, tempoL, tempoR);
            // Use the Modulo Operation to get the time Elapsed in the current bar
            double remainder = GetTimeRemainder(audioSource, bpm, tempoL, tempoR);
            // Calculate time remaining in the current bar
            return barLength - remainder;
        }
        #endregion
    }
}
