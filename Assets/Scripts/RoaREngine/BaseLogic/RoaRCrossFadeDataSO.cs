using System;
using UnityEngine;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoaRCrossFadeData", menuName = "RoaREngine/RoaRCrossFadeData")]
    public class RoaRCrossFadeDataSO : ScriptableObject
    {
        public string[] Names;
        public Params[] Parameters;

        [Serializable]
        public class Params
        {
            public float[] parameters;
        }

    }
}
