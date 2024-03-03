using System.Collections.Generic;
using UnityEngine;

namespace Core.Tag
{
    public class ComponentTags : MonoBehaviour
    {
        [SerializeField] private List<DataTag> _tags = new List<DataTag>();

        public bool ContainsTag(DataTag tag)
        {
            return _tags.Contains(tag);
        }

        public void AddTag(DataTag tag)
        {
            _tags.Add(tag);
        }

        public bool RemoveTag(DataTag tag)
        {
            return _tags.Remove(tag);
        }
    }
}