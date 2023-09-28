using System;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void Destroy(this Transform transform)
        {
            UnityEngine.Object.Destroy(transform.gameObject);
        }
        public static void Activate(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }
        public static void Deactivate(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static float DistanceTo(this Transform sourceTransform, Transform targetTransform)
        {
            if (sourceTransform == null || targetTransform == null)
            {
                throw new ArgumentException("Invalid Transform.");
            }

            return Vector3.Distance(sourceTransform.position, targetTransform.position);
        }
    }
}