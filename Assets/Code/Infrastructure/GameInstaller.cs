using System;
using Code.Infrastructure.Services.GameStater;
using Zenject;

namespace Code.Infrastructure
{
    public class GameInstaller : MonoInstaller, IInitializable, IDisposable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();
            Container.Bind<IGameStarter>().To<GameStarter>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IGameStarter>().Initialize();
        }

        public void Dispose()
        {
            
        }
    }
}