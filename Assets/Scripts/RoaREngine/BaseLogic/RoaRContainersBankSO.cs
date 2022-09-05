using System.Collections.Generic;
using UnityEngine;

namespace RoaREngine
{
    [CreateAssetMenu(fileName = "RoarContainersBank", menuName = "RoarEngine/RoarContainersBank")]
    public class RoarContainersBankSO : ScriptableObject
    {
        public List<RoarContainerSO> RoarContainers;
    }
}
