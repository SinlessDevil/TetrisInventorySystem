using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Code.Utilities.Attributes;

namespace Utilities.Editor
{
    public abstract class IdSelectorAttributeDrawer<TAttribute, TData> : OdinAttributeDrawer<TAttribute, string>
        where TAttribute : IdSelectorAttribute
        where TData : UnityEngine.Object
    {
        private TData[] _data;
        private string[] _weaponsId;

        protected override void Initialize()
        {
            base.Initialize();

            _data = LoadData();
            _weaponsId = GetIds(_data);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            string selectedValue = ValueEntry.SmartValue;
            string propertyName = !string.IsNullOrEmpty(Attribute.OverrideName) 
                ? Attribute.OverrideName 
                : GetPropertyName();
            
            int index = SirenixEditorFields.Dropdown(
                rect: rect,
                label: propertyName,
                selected: _weaponsId.IndexOf(selectedValue),
                itemNames: _weaponsId);

            if (index == -1)
                return;

            ValueEntry.SmartValue = _weaponsId[index];
            
            if(!Attribute.HasGameObjectField)
                return;

            GUI.enabled = false;
            rect = EditorGUILayout.GetControlRect();
            
            SirenixEditorFields.UnityObjectField(
                rect: rect,
                label: "Data",
                value: _data[index],
                objectType: typeof(TData),
                allowSceneObjects: false);
            
            GUI.enabled = true;
        }

        protected abstract string[] GetIds(TData[] data);
        
        protected abstract string GetPropertyName();
        
        private TData[] LoadData()
        {
            return AssetUtilities.GetAllAssetsOfType<TData>().ToArray();
        }
    }
}