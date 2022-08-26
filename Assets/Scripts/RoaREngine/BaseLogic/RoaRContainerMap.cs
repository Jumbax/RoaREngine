using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    [Serializable]
    public class RoaRContainerMap
    {
        #region var
        public List<RoaRContainerSO> roarContainers;
        private Dictionary<string, RoaRContainerSO> containerDict = new Dictionary<string, RoaRContainerSO>();
        #endregion

        #region public functions
        public void Init()
        {
            SetNames();
            ResetContainersBankIndex();
        }

        public void SetNames()
        {
            foreach (RoaRContainerSO container in roarContainers)
            {
                //CONTROLLO PER VEDERE SE CI SONO CONTAINER CON LO STESSO NOME
                containerDict[container.Name] = container;
            }
        }

        public void ResetContainersBankIndex()
        {
            foreach (RoaRContainerSO container in roarContainers)
            {
                container.ResetBankIndex();
            }
        }

        public void AddContainer(RoaRContainerSO container)
        {
            roarContainers.Add(container);
            containerDict[container.Name] = container;
        }

        public void RemoveContainer(RoaRContainerSO container)
        {
            roarContainers.Remove(container);
            containerDict.Remove(container.Name);
        }

        public RoaRContainerSO GetContainer(string musicID)
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
        #endregion
    }
}
