using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRManager : MonoBehaviour
    {
        private RoaRPooler roarPooler;
        public RoaRContainerMap RoarContainerMap = default;

        private void Start()
        {
            roarPooler = GetComponent<RoaRPooler>();
            RoarContainerMap.SetNames();
        }

        public void Play(string musicID)
        {
            GameObject roarEmitter = roarPooler.Get();
            if (roarEmitter != null)
            {
                if (RoarContainerMap.MusicIDIsValid(musicID))
                {
                    RoarContainerMap.SetContainer(musicID, roarEmitter);
                    roarEmitter.GetComponent<RoaREmitter>().Play();
                }
            }
        }

        public void Stop(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitterInPlay(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Stop();
                }
            }
        }

        public void Pause(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitterInPlay(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Pause();
                }
            }
        }

        public void Resume(string musicID)
        {
            if (RoarContainerMap.MusicIDIsValid(musicID))
            {
                GameObject roarEmitter = SearchActiveEmitter(musicID);
                if (roarEmitter != null)
                {
                    roarEmitter.GetComponent<RoaREmitter>().Resume();
                }
            }
        }

        private GameObject SearchActiveEmitterInPlay(string musicID)
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                if (roarEmitter.GetComponent<RoaREmitter>().IsPlaying())
                {
                    if (roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }

        private GameObject SearchActiveEmitter(string musicID)
        {
            foreach (GameObject roarEmitter in roarPooler.RoarEmitters)
            {
                if (!roarEmitter.GetComponent<RoaREmitter>().IsPlaying() && roarEmitter.activeInHierarchy == true)
                {
                    if (roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID))
                    {
                        return roarEmitter;
                    }
                }
            }
            return null;
        }
    }
}
