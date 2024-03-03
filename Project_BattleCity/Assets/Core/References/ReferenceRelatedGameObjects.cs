using System.Collections.Generic;
using UnityEngine;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Related Game Objects")]
    public class ReferenceRelatedGameObjects : ScriptableObject
    {
        public GameObject gameObject;
        public List<GameObject> relatedGameObjects;
    }
}
