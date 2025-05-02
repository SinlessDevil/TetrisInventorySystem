using System;
using System.Collections.Generic;

namespace Code.Infrastructure.Services.PersistenceProgress.Player
{
    [Serializable]
    public class InventoryOpeningData
    {
        public List<int> BoughtIndexes = new List<int>();

        public void SetBought(int index)
        {
          //  Debug.Log($"Bougth index {index}");
            BoughtIndexes.Add(index);
        }

        public bool IsBought(int index) => BoughtIndexes.Contains(index);
    }
}