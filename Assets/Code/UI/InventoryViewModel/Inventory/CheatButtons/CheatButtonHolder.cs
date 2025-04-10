using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.SaveLoad;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.InventoryViewModel.Inventory.CheatButtons
{
    public class CheatButtonHolder : MonoBehaviour
    {
        [Space(10)] [Header("Buttons")] 
        [SerializeField] private Button _buttonToggler;
        [SerializeField] private Button _buttonInventoryLevel;
        [SerializeField] private Button _buttonInventoryPoints;
        [SerializeField] private Button _buttonInventorySave;
        [Space(10)] [Header("Additional")] 
        [SerializeField] private RectTransform _centerRect;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _isPanelVisible = false;
        
        private IPersistenceProgressService _persistenceProgressService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        public void Construct(
            IPersistenceProgressService persistenceProgressService,
            ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _persistenceProgressService = persistenceProgressService;
        }
        
        public void Initialize()
        {
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public CanvasGroup CanvasGroup => _canvasGroup;
        
        private void Subscribe()
        {
            _buttonToggler.onClick.AddListener(OnClickToggler);
            _buttonInventoryLevel.onClick.AddListener(OnClickInventoryLevel);
            _buttonInventoryPoints.onClick.AddListener(OnClickInventoryPoints);
            _buttonInventorySave.onClick.AddListener(OnClickInventorySave);
        }

        private void Unsubscribe()
        {
            _buttonToggler.onClick.RemoveListener(OnClickToggler);
            _buttonInventoryLevel.onClick.RemoveListener(OnClickInventoryLevel);
            _buttonInventoryPoints.onClick.RemoveListener(OnClickInventoryPoints);
            _buttonInventorySave.onClick.RemoveListener(OnClickInventorySave);
        }
        
        private void OnClickToggler()
        {
            float targetX = _isPanelVisible ? 300f : 0f;

            _centerRect.DOAnchorPosX(targetX, 0.25f).SetEase(Ease.InOutCubic);

            _isPanelVisible = !_isPanelVisible;
        }
        
        private void OnClickInventoryLevel()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryLevel++;
            _persistenceProgressService.PlayerData.ResourceData.SetInventoryLevel(
                _persistenceProgressService.PlayerData.ResourceData.InventoryLevel);
        }

        private void OnClickInventoryPoints()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPoints++;
            _persistenceProgressService.PlayerData.ResourceData.SetInventoryPoints(
                _persistenceProgressService.PlayerData.ResourceData.InventoryPoints);
        }

        private void OnClickInventorySave()
        {
            _saveLoadService.SaveProgress();
        }
    }
}