using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "UI/UiRoot";
        
        private Transform _uiRoot;

        public UIFactory(IInstantiator instantiator) : base(instantiator) { }

        public Canvas UIRootCanvas { get; private set; }
        
        public void CreateUIRoot()
        {
            _uiRoot = Instantiate(UiRootPath).transform;
            UIRootCanvas = _uiRoot.GetComponent<Canvas>();
        }
    }
}
