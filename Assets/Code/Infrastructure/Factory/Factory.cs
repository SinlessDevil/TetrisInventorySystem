using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public abstract class Factory
    {
        private readonly IInstantiator _instantiator;

        protected Factory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        protected GameObject Instantiate(string resourcePath)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath);
            return TryMoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(string resourcePath, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath, parent);
            return TryMoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(string resourcePath, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(resourcePath, position, rotation, parent);
            return TryMoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(GameObject prefab)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab);
            return TryMoveToCurrentScene(gameObject);
        }

        protected GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab, position, rotation, parent);
            return TryMoveToCurrentScene(gameObject);
        }
        
        protected GameObject Instantiate(GameObject prefab, Transform parent) => 
            _instantiator.InstantiatePrefab(prefab, parent);

        private GameObject TryMoveToCurrentScene(GameObject gameObject)
        {
            if(gameObject.transform.parent == null)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            
            return gameObject;
        }
    }
}