using System.Collections.Generic;
using Core.Pattern;
using UnityEngine;

namespace Core.Tag
{
    public class TagManager : Singleton<TagManager>
    {
        #region tags
        [Header("Tags")]
        public List<DataTag> availableTags;
        private Dictionary<string, DataTag> availableTagByName = new Dictionary<string, DataTag>();
        #endregion

        void Start()
        {
            foreach (var availableTag in availableTags)
            {
                availableTagByName[availableTag.identifier] = availableTag;
            }
        }

        public DataTag GetTagByIdentifier(string identifier)
        {
            if (availableTagByName.ContainsKey(identifier))
            {
                return availableTagByName[identifier];
            }

            return null;
        }
    }
}
