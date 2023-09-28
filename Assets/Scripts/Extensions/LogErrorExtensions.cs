using UnityEngine;

namespace Extensions
{
    public static class LogErrorExtensions
    {
        public static void LogErrorIfComponentNull<T>(this T componnet)
        {
            if (componnet == null)
                Debug.LogError($"The {typeof(T)} Component is not assigned in the inspector!");
        }

        public static void LogErrorIfGameObjectNull<T>(this GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<T>(out var component))
                Debug.LogError($"The {typeof(T)} Component is not attached to {gameObject.name}!");
        }
    }
}