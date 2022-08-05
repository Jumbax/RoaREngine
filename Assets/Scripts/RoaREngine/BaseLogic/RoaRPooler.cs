using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    public class RoaRPooler : MonoBehaviour
    {
        #region var
        [SerializeField] private GameObject roarEmitter;
        [SerializeField] private int count;
        private List<GameObject> roarEmitters;

        public List<GameObject> RoarEmitters { get => roarEmitters;}
        #endregion

        #region private functions
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
        #endregion

        #region public functions
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
        #endregion
    }
}
