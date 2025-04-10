namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryLevelDisplayer : InventoryComponentDisplayer
    {
        public override void Initialize()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent += OnUpdateLevel;
            
            OnUpdateLevel();
        }

        public override void Dispose()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent -= OnUpdateLevel;
        }
        
        protected override void OnUpdateLevel()
        {
            var level = _persistenceProgressService.PlayerData.ResourceData.InventoryLevel;
            var text = $"Level: {level}";
            SetText(text);

            PlayGlowEffect();
        }
    }
}