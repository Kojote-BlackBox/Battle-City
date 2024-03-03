using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Reference
{
    [CreateAssetMenu(fileName = "NewReference", menuName = "Reference/Game Object")]
    public class ReferenceGameObject : ScriptableObject
    {
        public GameObject gameObject;
    }
}
