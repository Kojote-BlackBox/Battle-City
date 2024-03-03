using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Tag
{
    [CreateAssetMenu(fileName = "NewTag", menuName = "Data/Tag")]
    public class DataTag : ScriptableObject
    {
        #region tag
        [Header("tag")]
        public string identifier = "not set";
        #endregion
    }
}