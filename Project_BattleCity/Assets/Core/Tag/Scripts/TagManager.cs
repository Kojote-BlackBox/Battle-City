using System.Collections.Generic;
using Core.Pattern;
using UnityEngine;

namespace Core.Tag
{
    public class TagManager : Singleton<TagManager>
    {
        private bool _initialized = false;

        #region tags
        [Header("Tags")]
        public List<DataTag> availableTags;
        private Dictionary<string, DataTag> availableTagByName = new Dictionary<string, DataTag>();
        #endregion

        public DataTag GetTagByIdentifier(string identifier)
        {
            if (!_initialized) initialize();

            if (availableTagByName.ContainsKey(identifier))
            {
                return availableTagByName[identifier];
            }

            return null;
        }

        private void initialize() {
            if (_initialized || availableTags == null || availableTags.Count == 0) return;

            foreach (var availableTag in availableTags) {
                availableTagByName[availableTag.identifier] = availableTag;
            }

            _initialized = false;
        }
    }
}
