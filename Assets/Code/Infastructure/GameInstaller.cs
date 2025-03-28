using System;
using Code.Infastructure.Services.GameStater;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameInstaller : MonoInstaller, IInitializable, IDisposable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();
            Container.Bind<IGameStater>().To<GameStater>().AsSingle();
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}