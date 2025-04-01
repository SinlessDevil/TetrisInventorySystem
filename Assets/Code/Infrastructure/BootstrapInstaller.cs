using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.StaticData;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel.Items.Factory;
using Code.InventoryModel.Items.Provider;
using Code.InventoryModel.Services.InventoryDataProvider;
using Services.Factories.Inventory;
using Services.PersistenceProgress;
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

            BindFactory();
            BindStaticData();
            BindProgressData();
        }

        private void BindFactory()
        {
            Container.Bind<IItemFactory>().To<ItemFactory>().AsSingle();
        }

        private void BindProgressData()
        {
            Container.Bind<IPersistenceProgressService>().To<PersistenceProgressService>().AsSingle();
            Container.Bind<IInventoryExpandService>().To<InventoryExpandService>().AsSingle();
            Container.Bind<IInventorySaveInitializer>().To<InventorySaveInitializer>().AsSingle();
        }
        
        private void BindStaticData()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IItemDataProvider>().To<ItemDataProvider>().AsSingle();
            Container.Bind<IInventoryDataProvider>().To<InventoryDataProvider>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().LoadData();
            Container.Resolve<IItemDataProvider>().LoadData();
            Container.Resolve<IInventoryDataProvider>().LoadData();
            
            SceneManager.LoadScene(SceneName);
        }
    }
}