namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryPointDisplayer : InventoryComponentDisplayer
    {
        public override void Initialize()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent += OnUpdateLevel;
            
            OnUpdateLevel();
        }

        public override void Dispose()
        {
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent -= OnUpdateLevel;
        }

        protected override void OnUpdateLevel()
        {
            var points = _persistenceProgressService.PlayerData.ResourceData.InventoryPoints;
            var text = $"Points: {points}";
            SetText(text);
            
            PlayGlowEffect();
        }
    }
}