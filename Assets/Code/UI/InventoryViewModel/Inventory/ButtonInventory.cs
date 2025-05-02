using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class ButtonInventory : MonoBehaviour
    {
        [SerializeField] private Button _buttonInventory;
        
        private IInventoryViewInitializer _inventoryViewInitializer;
        
        [Inject]
        public void Construct(IInventoryViewInitializer inventoryViewInitializer)
        {
            _inventoryViewInitializer = inventoryViewInitializer;
        }
        
        private void OnValidate()
        {
            if (_buttonInventory == null)
                _buttonInventory = GetComponent<Button>();
        }
        
        private void Start()
        {
            _buttonInventory.onClick.AddListener(OnCLickButton); 
        }
        
        private void OnDestroy()
        {
            _buttonInventory.onClick.RemoveListener(OnCLickButton); 
        }

        private void OnCLickButton()
        {
            if (_inventoryViewInitializer.HasOpenInventory)
            {
                _inventoryViewInitializer.CloseInventory();
            }
            else
            {
                _inventoryViewInitializer.OpenInventory();
            }
        }
    }
}