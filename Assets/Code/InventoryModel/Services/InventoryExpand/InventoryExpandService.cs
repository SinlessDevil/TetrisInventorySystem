using System;
using Code.Infrastructure.Services.PersistenceProgress;
using Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Code.Infrastructure.Services.PlayerExperience;
using Code.InventoryModel.Data;
using Code.InventoryModel.Services.InventoryDataProvider;

namespace Code.Inventory.Services.InventoryExpand
{
    public class InventoryExpandService : IInventoryExpandService
    {
        private readonly IPersistenceProgressService _progress;
        private readonly IPlayerExperienceService _playerExperienceService;
        private readonly IInventoryDataProvider _inventoryDataProvider;
        
        private InventoryBorders _defaultOpened;

        public InventoryExpandService(
            IPersistenceProgressService progress,
            IPlayerExperienceService playerExperienceService,
            IInventoryDataProvider inventoryDataProvider)
        {
            _progress = progress;
            _playerExperienceService = playerExperienceService;
            _inventoryDataProvider = inventoryDataProvider;
        }

        public InventoryBorders DefaultOpened => _defaultOpened;
        
        private InventoryOpeningData InventoryOpeningData => _progress.PlayerData.InventoryData.InventoryOpening;

        public void SetDefaultOpenedBorders(InventoryBorders defaultBorder)
        {
            _defaultOpened = defaultBorder
                ? defaultBorder
                : throw new ArgumentNullException(nameof(defaultBorder));
        }

        public bool IsOpened(int targetIndex)
        {
            InventoryOpeningData openingData = InventoryOpeningData;
            return openingData.IsBought(targetIndex);
        }

        public bool IsAvailableToBuy(int targetIndex)
        {
            int currentLevel = _playerExperienceService.GetCurrentLevel();
            InventoryExpandConfig expandConfig = _inventoryDataProvider.InventoryExpandForLevel(currentLevel);
            return expandConfig.Borders.IsAvailable(targetIndex);
        }

        public bool IsEnoughPoints(int _) => _progress.PlayerData.ResourceData.InventoryPoints > 0;

        public void Open(int targetIndex)
        {
            if (!IsEnoughPoints(targetIndex))
                throw new InvalidOperationException($"Is not enough points {targetIndex}");
            
            if(!IsAvailableToBuy(targetIndex))
                throw new InvalidOperationException($"Is not available to bought {targetIndex}");
            
            InventoryOpeningData.SetBought(targetIndex);

            _progress.PlayerData.ResourceData.InventoryPoints--;
            _progress.PlayerData.ResourceData.SetInventoryPoints(_progress.PlayerData.ResourceData.InventoryPoints);
        }

        public int GetLevelForAvailability(int targetIndex)
        {
            foreach (InventoryExpandConfig expandConfig in _inventoryDataProvider.AllInventoryExpand)
            {
                if(!expandConfig.Borders.IsAvailable(targetIndex))
                    continue;

                return expandConfig.Level;
            }
            
            return 99;
        }
    }
}