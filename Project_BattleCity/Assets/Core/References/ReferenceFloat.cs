using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Float")]
    public class ReferenceFloat : ScriptableObject
    {
        public float value;
    }
}
