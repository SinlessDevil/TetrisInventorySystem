using Code.Infrastructure.Services.StaticData;
using Code.Inventory;
using Code.Inventory.Items.Factory;
using Code.Inventory.Items.Provider;
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

            BindUIFactory();
            BindStaticData();
        }

        private void BindUIFactory()
        {
            Container.Bind<IItemFactory>().To<ItemFactory>().AsSingle();
        }
        
        private void BindStaticData()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IItemDataProvider>().To<ItemDataProvider>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().LoadData();
            Container.Resolve<IItemDataProvider>().LoadData();
            
            SceneManager.LoadScene(SceneName);
        }
    }
}