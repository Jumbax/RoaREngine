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

        public void Init()
        {
            SetNames();
            ResetContainersBankIndex();
        }

        public void SetNames()
        {
            foreach (RoaRContainer container in roarContainers)
            {
                //CONTROLLO PER VEDERE SE CI SONO CONTAINER CON LO STESSO NOME
                containerDict[container.Name] = container;
            }
        }

        public void ResetContainersBankIndex()
        {
            foreach (RoaRContainer container in roarContainers)
            {
                container.ResetBankIndex();
            }
        }

        public void AddContainer(RoaRContainer container)
        {
            roarContainers.Add(container);
            containerDict[container.Name] = container;
        }

        public void RemoveContainer(RoaRContainer container)
        {
            roarContainers.Remove(container);
            containerDict.Remove(container.Name);
        }

        public RoaRContainer GetContainer(string musicID)
        {
            if (MusicIDIsValid(musicID))
            {
                return containerDict[musicID];
            }
            return null;
        }

        public void SetContainer(string musicID, GameObject roarEmitter)
        {
            roarEmitter.GetComponent<RoaREmitter>().SetContainer(containerDict[musicID]);
        }

        public bool MusicIDIsValid(string musicID)
        {
            return containerDict.ContainsKey(musicID);
        }

    }
}
