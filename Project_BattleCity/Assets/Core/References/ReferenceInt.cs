using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Int")]
    public class ReferenceInt : ScriptableObject
    {
        public int value;
    }
}
