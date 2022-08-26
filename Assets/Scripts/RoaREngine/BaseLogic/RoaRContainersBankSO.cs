using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoaRContainersBank", menuName = "RoaREngine/RoaRContainersBank")]
    public class RoaRContainersBankSO : ScriptableObject
    {
        public List<RoaRContainerSO> roarContainers;
    }
}
