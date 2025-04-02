using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class UIFactory : IUIFactory
    {
        private readonly IInstantiator _instantiator;
        private const string UiRootPath = "UI/UiRoot";
        
        private Canvas _uiRoot;

        public UIFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public Canvas UIRootCanvas => _uiRoot;
        
        public void CreateUiRoot()
        {
            var gameObject = _instantiator.InstantiatePrefabResource(UiRootPath, null);
            _uiRoot = gameObject.GetComponent<Canvas>();
            TryMoveToCurrentScene(gameObject);
        }
        
        private GameObject TryMoveToCurrentScene(GameObject gameObject)
        {
            if(gameObject.transform.parent == null)
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            
            return gameObject;
        }
    }
}
