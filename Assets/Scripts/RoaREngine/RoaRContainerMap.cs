using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    [Serializable]
    public class RoaRContainerMap
    {
        public List<RoaRContainer> roarContainers;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();

        public void SetNames()
        {
            foreach (RoaRContainer container in roarContainers)
            {
                containerDict[container.Name] = container;
            }
        }

        public void SetContainer(string musicID, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoaREmitter>().SetContainer(containerDict[musicID]);
        }

        public bool MusicIDIsValid(string musicID)
        {
            return containerDict.ContainsKey(musicID);
        }

        public RoaRContainer GetContainer(string musicID)
        {
            return containerDict[musicID];
        }
    }
}
