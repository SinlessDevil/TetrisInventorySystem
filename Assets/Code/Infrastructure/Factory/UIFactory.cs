using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class UIFactory : Factory, IUIFactory
    {
        private const string UiRootPath = "UI/UiRoot";
        private const string GameHudPath = "UI/GameHud";
        
        public UIFactory(IInstantiator instantiator) : base(instantiator) { }

        public Canvas UIRootCanvas { get; private set; }
        public Canvas GameHudCanvas { get; private set; }
        
        public void CreateUIRoot()
        {
            var uiRoot = Instantiate(UiRootPath).transform;
            UIRootCanvas = uiRoot.GetComponent<Canvas>();
        }
        
        public void CreateGameHud()
        {
            var gamehud = Instantiate(GameHudPath).transform;
            GameHudCanvas = gamehud.GetComponent<Canvas>();
        }
    }
}
