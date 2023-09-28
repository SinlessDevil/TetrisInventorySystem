using UnityEngine;
using UnityEditor;
using InventorySystem.Model.Items.StaticData;
using InventorySystem.Model.Items;

namespace Editors
{
    [CustomEditor(typeof(InventoryData))]
    public class InventoryDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InventoryData inventoryData = (InventoryData)target;

            inventoryData.Width = EditorGUILayout.IntField("Width", inventoryData.Width);
            inventoryData.Height = EditorGUILayout.IntField("Height", inventoryData.Height);

            float labelHeight = 30f;

            GUIStyle labelStyle = new(EditorStyles.boldLabel);
            labelStyle.fontSize = 18;

            Rect labelRect = EditorGUILayout.GetControlRect(GUILayout.Height(labelHeight));
            labelRect.x += (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) / 2;
            GUI.Label(labelRect, "Inventory Grid", labelStyle);

            GUILayout.Space(5);

            if (inventoryData.Inventory == null || inventoryData.Inventory.GetLength(0) != inventoryData.Width || inventoryData.Inventory.GetLength(1) != inventoryData.Height)
            {
                inventoryData.Inventory = new SlotData[inventoryData.Width, inventoryData.Height];
            }

            GUILayout.Space(5);

            for (int j = 0; j < inventoryData.Height; j++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int i = 0; i < inventoryData.Width; i++)
                {
                    EditorGUI.BeginChangeCheck();

                    if (inventoryData.Inventory[i, j] == null)
                    {
                        inventoryData.Inventory[i, j] = new SlotData();
                    }

                    GUILayout.BeginVertical();

                    inventoryData.Inventory[i, j].Foldout = EditorGUILayout.Foldout(inventoryData.Inventory[i, j].Foldout,
                        inventoryData.Inventory[i, j].ItemType + " : " +
                        inventoryData.Inventory[i, j].CurrentAmountItem, true);

                    if (inventoryData.Inventory[i, j].Foldout)
                    {
                        EditorGUI.indentLevel++;

                        GUILayout.Space(5);

                        GUILayout.BeginVertical();
                        GUILayout.Label("Item Type", GUILayout.Width(80));
                        inventoryData.Inventory[i, j].ItemType = (TypeItem)EditorGUILayout.EnumPopup(inventoryData.Inventory[i, j].ItemType, GUILayout.Width(80));
                        GUILayout.EndVertical();

                        GUILayout.Space(3);

                        GUILayout.BeginVertical();
                        GUILayout.Label("Amount", GUILayout.Width(40));
                        inventoryData.Inventory[i, j].CurrentAmountItem = EditorGUILayout.IntField(inventoryData.Inventory[i, j].CurrentAmountItem, GUILayout.Width(80));
                        GUILayout.EndVertical();

                        EditorGUI.indentLevel--;
                    }

                    GUILayout.EndVertical();
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}