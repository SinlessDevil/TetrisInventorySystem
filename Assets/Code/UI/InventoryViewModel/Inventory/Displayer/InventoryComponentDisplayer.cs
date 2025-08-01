using Code.Infrastructure.Services.PersistenceProgress;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public abstract class InventoryComponentDisplayer : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Image _glow;

        protected IPersistenceProgressService _persistenceProgressService;
        
        [Inject]
        public void Construct(IPersistenceProgressService persistenceProgressService)
        {
            _persistenceProgressService = persistenceProgressService;
        }

        public virtual void Initialize()
        {
            Subscribe();
            
            OnUpdateLevel();
        }

        public virtual void Dispose()
        {
            Unsubscribe();
        }

        protected virtual void Subscribe()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent += OnUpdateLevel;
        }
        
        protected virtual void Unsubscribe()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent -= OnUpdateLevel;
        }

        protected abstract void OnUpdateLevel();
        
        protected void SetText(string text)
        {
            _text.text = text;
        }

        protected void PlayGlowEffect()
        {
            _glow.DOFade(1f, 0.25f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _glow.DOFade(0f, 0.25f)
                        .SetEase(Ease.Linear);
                });
        }
    }
}