using System.Collections.Generic;
using UnityEngine;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Game Objects")]
    public class ReferenceGameObjects : ScriptableObject
    {
        public int totalGameObjects;
        public int destroyedGameObjects;
        public List<GameObject> activeGameObjects;
    }
}
