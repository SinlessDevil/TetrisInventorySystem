using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyAfterDelay(this GameObject gameObject, float delay)
        {
            Object.Destroy(gameObject, delay);
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static GameObject FindChildByName(this GameObject gameObject, string name)
        {
            Transform transform = gameObject.transform.Find(name);
            return transform != null ? transform.gameObject : null;
        }

        public static void DestroyAllChildren(this GameObject gameObject)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(gameObject.transform.GetChild(i).gameObject);
            }
        }

        public static void SetRenderersEnabled(this GameObject gameObject, bool enabled)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = enabled;
            }
        }

        public static void ToggleChildObjects(this GameObject parentObject, bool activate)
        {
            int childCount = parentObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject childObject = parentObject.transform.GetChild(i).gameObject;
                childObject.SetActive(activate);
            }
        }
    }
}