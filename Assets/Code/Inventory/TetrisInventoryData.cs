using System.Collections.Generic;
using Code.Inventory.Items.Data;
using Newtonsoft.Json;

namespace Code.Inventory
{
    public class TetrisInventoryData
    {
        [JsonProperty("g")] public List<GridCell> Cells;
        [JsonProperty("i")] public List<Item> Items;
        [JsonProperty("c")] public int Columns;
        [JsonProperty("r")] public int Rows;

        public TetrisInventoryData(int columns, int rows)
        {
            Rows = rows;
            Columns = columns;
            Cells = new List<GridCell>();
            Items = new List<Item>();

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Cells.Add(new GridCell
                    {
                        GridX = i,
                        GridY = j
                    });
                }
            }

            for (int i = 0; i < Cells.Count; i++)
            {
                GridCell cell = Cells[i];
                GridCell[] cellNeighbors = cell.Neighbors;

                cellNeighbors[0] = Cells.Find(x => x.GridX.Equals(cell.GridX + 0) && x.GridY.Equals(cell.GridY - 1));
                cellNeighbors[1] = Cells.Find(x => x.GridX.Equals(cell.GridX + 1) && x.GridY.Equals(cell.GridY + 0));
                cellNeighbors[2] = Cells.Find(x => x.GridX.Equals(cell.GridX + 0) && x.GridY.Equals(cell.GridY + 1));
                cellNeighbors[3] = Cells.Find(x => x.GridX.Equals(cell.GridX - 1) && x.GridY.Equals(cell.GridY + 0));
            }
        }
    }
}