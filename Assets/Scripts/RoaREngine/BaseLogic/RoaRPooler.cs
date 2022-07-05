using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRPooler : MonoBehaviour
    {
        [SerializeField] private GameObject roarEmitter;
        [SerializeField] private int count;
        private List<GameObject> roarEmitters;

        public List<GameObject> RoarEmitters { get => roarEmitters;}

        private void Awake()
        {
            roarEmitters = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                GameObject go = Instantiate(roarEmitter, transform);
                go.SetActive(false);
                roarEmitters.Add(go);
            }
        }

        public GameObject Get()
        {
            for (int i = 0; i < roarEmitters.Count; i++)
            {
                if (!roarEmitters[i].activeInHierarchy)
                {
                    roarEmitters[i].SetActive(true);
                    return roarEmitters[i];
                }
            }

            GameObject go = Instantiate(roarEmitter, transform);
            go.SetActive(true);
            roarEmitters.Add(go);
            return go;
        }

    }
}
