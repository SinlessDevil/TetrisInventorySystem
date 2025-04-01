using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.InventoryModel.Items.Data
{
    [Serializable]
    public class InventoryPlacement
    {
        public InventoryPlacement(bool[,] space)
        {
            Space = space;
        }
        
        public InventoryPlacement(int size)
        {
            if (size % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(size), $"should be non even, was {size}");
            
            Space = new bool[3, 3];
            Space[1, 1] = true;
        }

        public int RootPositionX;
        public int RootPositionY;

        [OnValueChanged(nameof(ValidateSpace))]
        [TableMatrix(
            HorizontalTitle = "Custom Cell Drawing",
            DrawElementMethod = nameof(DrawColoredEnumElement),
            ResizableColumns = false,
            RowHeight = 16,
            SquareCells = true,
            Transpose = true)]
        public bool[,] Space;

        private static bool DrawColoredEnumElement(Rect rect, bool value)
        {
#if UNITY_EDITOR
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            UnityEditor.EditorGUI.DrawRect(rect.Padding(1), value ? new Color(0.1f, 0.8f, 0.2f) : new Color(0, 0, 0, 0.5f));
#endif
            return value;
        }
        
        public void SetRootPosition(int x, int y)
        {
            RootPositionX = x; // max 6 // 3 // 
            RootPositionY = y; // max 5 // 2 // 
        }
        
        [Button]
        public List<int> GetIndexShifts(int inventoryHeight)
        {
            List<int> shifts = new List<int>();

            int rowsCount = Space.GetLength(0);
            int columnsCount = Space.GetLength(1);
            
            int centerIndex = rowsCount / 2;

            //UnityEngine.Debug.Log($"rowsCount {rowsCount}, columnsCount {columnsCount}");

            for (int column = 0; column < columnsCount ; column++)
            {
                for (int row = 0; row < rowsCount; row++)
                {
                    bool isKeepSpace = Space[row, column];
                    
                    if(!isKeepSpace)
                        continue;

                    int rowShift = row - centerIndex;
                    int columnShift = column - centerIndex;

                    int shift = rowShift + columnShift * inventoryHeight;
                    shifts.Add(shift);
                    
                    //UnityEngine.Debug.Log($"r {row}, c {column} => {shift}");
                }
            }

            return shifts;
        }
        
        public void ValidateSpace()
        {
            Space[1, 1] = true;
        }
    }
}