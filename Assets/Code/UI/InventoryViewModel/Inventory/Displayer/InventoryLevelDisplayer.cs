using Code.Infrastructure.Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryLevelDisplayer : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private IPersistenceProgressService _persistenceProgressService;
        
        [Inject]
        public void Construct(IPersistenceProgressService persistenceProgressService)
        {
            _persistenceProgressService = persistenceProgressService;

            OnUpdateLevel();
        }
        
        public void Initialize()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent += OnUpdateLevel;
        }

        public void Dispose()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent -= OnUpdateLevel;
        }
        
        private void OnUpdateLevel()
        {
            var level = _persistenceProgressService.PlayerData.ResourceData.InventoryLevel;
            var text = $"Level: {level}";
            SetTextLevels(text);
        }
        
        private void SetTextLevels(string text)
        {
            _text.text = text;
        }
    }
}