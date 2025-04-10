using Code.Infrastructure.Services.PersistenceProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryPointDisplayer : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private IPersistenceProgressService _persistenceProgressService;
        
        [Inject]
        public void Construct(IPersistenceProgressService persistenceProgressService)
        {
            _persistenceProgressService = persistenceProgressService;

            OnUpdatePoints();
        }
        
        public void Initialize()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent += OnUpdatePoints;
        }

        public void Dispose()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent -= OnUpdatePoints;
        }
        
        private void OnUpdatePoints()
        {
            var points = _persistenceProgressService.PlayerData.ResourceData.InventoryPoints;
            var text = $"Points: {points}";
            SetTextPoints(text);
        }
        
        private void SetTextPoints(string text)
        {
            _text.text = text;
        }
    }
}