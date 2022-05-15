using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRContainerMap : MonoBehaviour
    {
        //DEBUG: dev'essere privato ed impostarsi da editor
        public List<RoaRContainer> roarContainers;
        private Dictionary<string, RoaRContainer> containerDict = new Dictionary<string, RoaRContainer>();

        private void Start()
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

        public bool Test(List<GameObject> roarEmitters, string musicID)
        {
            foreach (GameObject roarEmitter in roarEmitters)
            {
                return roarEmitter.GetComponent<RoaREmitter>().CheckForContainerName(musicID);
            }
            return false;
        }
    }
}
