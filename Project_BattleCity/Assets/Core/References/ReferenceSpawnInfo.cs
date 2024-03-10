using UnityEngine;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Spawn Info")]
    public class ReferenceSpawnInfo : ScriptableObject
    {
        public string label;
        public Sprite image;
    }
}   