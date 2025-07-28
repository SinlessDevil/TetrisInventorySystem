using System.Text;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Code.Infrastructure.Services.SaveLoad;
using Code.Infrastructure.Services.StaticData;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel;
using Code.InventoryModel.Services.InventoryPlayer;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using Services.Factories.Inventory;
using UnityEngine;

namespace Code.Infrastructure.Services.GameStater
{
    public class GameStarter : IGameStarter
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IPersistenceProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IUIFactory _uiFactory;
        private readonly IInventorySaveInitializer _inventorySaveInitializer;
        private readonly IInventoryPlayerSetUper _playerInventory;
        private readonly IInventoryExpandService _inventoryExpandService;
        public GameStarter(
            IPersistenceProgressService progressService,
            IStaticDataService staticDataService,
            ISaveLoadService saveLoadService, 
            IUIFactory uiFactory,
            IInventorySaveInitializer inventorySaveInitializer,
            IInventoryPlayerSetUper playerInventory, 
            IInventoryExpandService inventoryExpandService)
        {
            _progressService = progressService;
            _staticDataService = staticDataService;
            _saveLoadService = saveLoadService;
            _uiFactory = uiFactory;
            _inventorySaveInitializer = inventorySaveInitializer;
            _playerInventory = playerInventory;
            _inventoryExpandService = inventoryExpandService;
        }

        public void Initialize()
        {
            Debug.Log("GameStarter.Initialize");
            
            InitProgress();
            InitInventoryModel();
            DebugInventoryModel();
            InitUI();
        }
        
        private void InitInventoryModel()
        {
            TetrisInventoryData inventoryData = _progressService.PlayerData.InventoryData.PlayerInventory;
            IInventory inventory = new ExpandableInventory(new TetrisInventory(inventoryData), _inventoryExpandService);
            _playerInventory.SetInventory(inventory);
        }

        private void InitProgress()
        {
            _progressService.PlayerData = LoadProgress() ?? SetUpBaseProgress();   
        }

        private void DebugInventoryModel()
        {
            TetrisInventoryData inventory = _progressService.PlayerData.InventoryData.PlayerInventory;
            Debug.Log("===== Inventory State =====");

            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < inventory.Rows; y++)
            {
                string[] row = new string[inventory.Columns];

                for (int x = 0; x < inventory.Columns; x++)
                {
                    GridCell cell = inventory.Cells[y * inventory.Columns + x];
                    string cellContent = cell.Item != null ? cell.Item.Id : " Empty ";
                    row[x] = $"({x},{y}) {cellContent}";
                }

                sb.AppendLine(string.Join(" | ", row));
            }

            Debug.Log(sb.ToString());
        }
        
        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateGameHud();
        }
        
        private PlayerData LoadProgress()
        {
            Debug.Log("LoadProgress");
            
            return _saveLoadService.Load();
        }

        private PlayerData SetUpBaseProgress()
        {
            Debug.Log("InitializeProgress");

            PlayerData progress = new PlayerData(
                columns: InventorySize.Columns,
                rows: InventorySize.Rows);
            
            _progressService.PlayerData = progress;
            
            _inventorySaveInitializer.Initialize(
                initialItems: _staticDataService.Balance.Inventory.DefaultItems,
                defaultBorders: _staticDataService.Balance.Inventory.DefaultOpenedCells);
            
            return progress;
        }
    }
}