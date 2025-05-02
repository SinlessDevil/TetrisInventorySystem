using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IUIFactory
    {
        Canvas UIRootCanvas { get; }
        Canvas GameHudCanvas { get; }
        void CreateUIRoot();
        void CreateGameHud();
    }
}