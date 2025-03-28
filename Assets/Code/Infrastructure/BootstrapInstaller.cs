using Code.Infrastructure.Services.StaticData;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        private const string SceneName = "Game";
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
            
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().Load();

            SceneManager.LoadScene(SceneName);
        }
    }
}