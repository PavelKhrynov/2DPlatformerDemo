using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Model.Definitions.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryIdAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.Instance.Items.ItemsForEditor;
            var ids = new List<string>();
            foreach (var def in defs)
            {
                ids.Add(def.Id);
            }

            int index = Mathf.Max(ids.IndexOf(property.stringValue), 0);
            EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}
