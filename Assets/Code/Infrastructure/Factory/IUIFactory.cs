using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IUIFactory
    {
        Canvas UIRootCanvas { get; }
        void CreateUiRoot();
    }
}