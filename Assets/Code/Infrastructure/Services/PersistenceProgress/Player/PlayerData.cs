using System;
using Code.InventoryModel;
using JetBrains.Annotations;
using Services.PersistenceProgress.Player;
using UnityEngine;

namespace Code.Infrastructure.Services.PersistenceProgress.Player
{
    [Serializable]
    public class PlayerData
    {
        public InventoryData InventoryData;
        public ResourceData ResourceData = new ResourceData();
        
        [UsedImplicitly]
        public PlayerData()
        {
            
        }
        
        public PlayerData(int columns, int rows)
        {
            InventoryData = new InventoryData(new TetrisInventoryData(columns, rows));
            Debug.Log("PlayerData.InventoryData " + InventoryData);
        }
    }
}