using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Bool")]
    public class ReferenceBool : ScriptableObject
    {
        public bool value;
    }
}
