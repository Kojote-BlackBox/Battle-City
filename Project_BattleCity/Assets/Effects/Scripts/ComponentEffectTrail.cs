using System.Collections.Generic;
using UnityEngine;

namespace Effect.Trail
{
    public class ComponentEffectTrail : MonoBehaviour
    {
        #region trails
        [Header("Trail Settings")]
        public int trailSegments;
        public float trailTime;
        public GameObject prefabTrail;
        [HideInInspector] public Vector3 directionTrail;
        #endregion

        #region spawn
        private float _spawnInterval;
        private float _spawnTimer;
        private bool _enabled;
        #endregion

        #region reusability
        private List<GameObject> _trailObjectsInUse;
        private Queue<GameObject> _trailObjectsNotInUse;
        #endregion

        private void Start()
        {
            _spawnInterval = trailTime / trailSegments;
            _trailObjectsInUse = new List<GameObject>();
            _trailObjectsNotInUse = new Queue<GameObject>();

            for (var i = 0; i < trailSegments; i++)
            {
                var trailInstance = Instantiate(prefabTrail, transform.position, Quaternion.identity);

                _trailObjectsNotInUse.Enqueue(trailInstance);
            }

            SetEnabled(false);
        }

        private void Update()
        {
            if (!_enabled) return;

            _spawnTimer += Time.deltaTime;

            if (!(_spawnTimer >= _spawnInterval)) return;

            var trailInstanceUnused = _trailObjectsNotInUse.Dequeue();

            if (trailInstanceUnused == null) return;

            var trailObjectUnused = trailInstanceUnused.GetComponent<TrailObject>();

            var angle = Mathf.Atan2(directionTrail.y, directionTrail.x) * Mathf.Rad2Deg;
            trailObjectUnused.Initiate(trailTime, null, transform.position, Quaternion.AngleAxis(angle, Vector3.forward), this);

            _trailObjectsInUse.Add(trailInstanceUnused);

            _spawnTimer = 0;
        }

        public void RemoveTrailObject(GameObject obj)
        {
            _trailObjectsInUse.Remove(obj);
            _trailObjectsNotInUse.Enqueue(obj);
        }

        public void SetEnabled(bool enable)
        {
            if (_enabled == enable) return;

            _enabled = enable;

            if (enable)
            {
                _spawnTimer = _spawnInterval;
            }
        }
    }
}
