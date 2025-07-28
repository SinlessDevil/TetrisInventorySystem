namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryLevelDisplayer : InventoryComponentDisplayer
    {
        protected override void OnUpdateLevel()
        {
            int level = _persistenceProgressService.PlayerData.ResourceData.InventoryLevel;
            string text = $"Level: {level}";
            
            SetText(text);

            PlayGlowEffect();
        }
    }
}