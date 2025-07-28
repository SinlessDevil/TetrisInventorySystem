using Code.Infrastructure.Factory;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.SaveLoad;
using Code.Infrastructure.Services.StaticData;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel.Items.Factory;
using Code.InventoryModel.Items.Provider;
using Code.InventoryModel.Services.InventoryDataProvider;
using Code.InventoryModel.Services.InventoryPlayer;
using Code.UI.InventoryViewModel.Factory;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using Services.Factories.Inventory;
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
            BindSaveLoad();
            BindProgressData();
            BindInventoryServices();
            BindStaticData();
        }

        private void BindFactory()
        {
            Container.BindInterfacesTo<ItemFactory>().AsSingle();
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<InventoryUIFactory>().AsSingle();
        }
        
        private void BindSaveLoad() =>
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();

        private void BindProgressData() =>
            Container.Bind<IPersistenceProgressService>().To<PersistenceProgressService>().AsSingle();
        
        private void BindInventoryServices()
        {
            Container.Bind<IInventoryExpandService>().To<InventoryExpandService>().AsSingle();
            Container.Bind<IInventorySaveInitializer>().To<InventorySaveInitializer>().AsSingle();
            Container.Bind<IInventoryPlayerSetUper>().To<InventoryPlayerSetUper>().AsSingle();
            
            Container.Bind<IItemDropService>().To<ItemDropService>().AsSingle();
            
            Container.Bind<IInventoryViewInitializer>().To<InventoryViewInitializer>().AsSingle();
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