using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Extensions
{
    public static class CameraExtensions
    {
        public static List<T> GetVisibleTargets<T>(Camera camera, List<T> targets, float visibilityOffset = 0f) where T : Component
        {
            List<T> visibleTargets = new List<T>();
    
            foreach (T target in targets)
            {
                if (IsTargetVisible(camera, target, visibilityOffset)) 
                    visibleTargets.Add(target);
            }

            return visibleTargets;
        }

        public static bool IsTargetVisible<T>(Camera camera, T target, float visibilityOffset = 0f) where T : Component
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(target.transform.position);
            
            return viewportPoint.x + visibilityOffset >= 0 && viewportPoint.x - visibilityOffset <= 1 &&
                   viewportPoint.y + visibilityOffset >= 0 && viewportPoint.y - visibilityOffset <= 1 &&
                   viewportPoint.z > 0;
        }
    }
}