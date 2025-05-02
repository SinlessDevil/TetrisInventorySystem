using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Code.InventoryModel.Data
{
    [CreateAssetMenu(menuName = "StaticData/Inventory/InventoryBorders", fileName = "InventoryBorders", order = 801)]
    public class InventoryBorders : SerializedScriptableObject
    {
        [OdinSerialize]
        [OnValueChanged(nameof(ValidateSpace))]
        [TableMatrix(
            HorizontalTitle = "Custom Cell Drawing",
            DrawElementMethod = nameof(DrawColoredEnumElement),
            ResizableColumns = false,
            RowHeight = 16,
            SquareCells = true,
            Transpose = true)]
        public bool[,] Space;

        public InventoryBorders() => ValidateSpace();

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

        public bool IsAvailable(int index)
        {
            int currentIndex = 0;
            
            for (int j = 0; j < Space.GetLength(1); j++)
            {
                for (int i = 0; i < Space.GetLength(0); i++)
                {
                    if(index == currentIndex)
                        return Space[i,j];

                    currentIndex++;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index), $"was {index}");
        }

        public IEnumerable<int> GetAvailableIndexes()
        {
            int currentIndex = 0;
            for (int j = 0; j < Space.GetLength(1); j++)
            {
                for (int i = 0; i < Space.GetLength(0); i++)
                {
                    bool isAvailable = Space[i,j];
                    if(isAvailable)
                    {
                        yield return currentIndex;
                    }

                    currentIndex++;
                }
            }
        }
        
        private void OnValidate()
        {
            ValidateSpace();
        }

        public void ValidateSpace()
        {
            var columns = InventorySize.Columns;
            var row = InventorySize.Rows;
            Space ??= new bool[columns, row];
        }

    }
}