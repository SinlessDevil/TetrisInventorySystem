namespace Code.UI.InventoryViewModel.Inventory.Displayer
{
    public class InventoryPointDisplayer : InventoryComponentDisplayer
    {
        protected override void OnUpdateLevel()
        {
            int points = _persistenceProgressService.PlayerData.ResourceData.InventoryPoints;
            string text = $"Points: {points}";
            
            SetText(text);
            
            PlayGlowEffect();
        }
    }
}