using StaticTags;
using System.Collections.Generic;
using UnityEngine;

namespace WorldPhysics
{
    public class WorldConstraint : MonoBehaviour
    {
        [Header("Transform of parent object")]
        private Transform _planet;

        private Transform _cachedTransform;

        private void Start()
        {
            _cachedTransform = GetComponent<Transform>();
            _planet = GameObject.FindGameObjectWithTag(Tags.Planet).transform;
        }

        private void FixedUpdate()
        {
            WorldAttraction();
        }

        private void WorldAttraction()
        {
            Quaternion rotation =
                Quaternion.FromToRotation(-transform.up, _planet.position - _cachedTransform.position);
            _cachedTransform.rotation = rotation * transform.rotation;
        }
    }
}
