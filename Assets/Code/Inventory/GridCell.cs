using System;
using Code.Inventory.Items.Data;
using Newtonsoft.Json;

namespace Code.Inventory
{
    [Serializable]
    public class GridCell
    {
        [JsonProperty("x")] public int GridX;
        [JsonProperty("y")] public int GridY;
        [JsonProperty("i")] public Item Item;

        [JsonIgnore] public readonly GridCell[] Neighbors = new GridCell[4];
        
        [JsonIgnore] public bool IsFree => Item == null;
        [JsonIgnore] public bool IsOccupied => Item != null;
        
        public void Place(Item item) => Item = item;

        public void Clear() => Item = null;

        public bool HasSamePosition(GridCell with) => GridX == with.GridX && GridY == with.GridY;
    }
}