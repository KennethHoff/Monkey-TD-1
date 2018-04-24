using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GameControl {

    [CustomPropertyDrawer(typeof(TowerUpgrade))]
    public class TowerUpgradeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // base.OnGUI(position, property, label);
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect iconRect           = new Rect(position.x,                              position.y + 5,                             position.width * 0.5f - 5,  position.height/2.5f);

            Rect nameRect           = new Rect(position.x + position.width * 0.5f,      position.y + 5,                             position.width * 0.35f,     position.height/2.5f);

            Rect costRect           = new Rect(position.x + position.width * 0.85f + 5, position.y + 5,                             position.width * 0.15f - 5, position.height/2.5f);

            Rect descriptionRect    = new Rect(position.x,                              position.y + 5 + position.height/2.5f+2.5f, position.width,             position.height/2.5f);

            SerializedProperty iconProperty = property.FindPropertyRelative("upgradeIcon");
            SerializedProperty nameProperty = property.FindPropertyRelative("upgradeName");
            SerializedProperty costProperty = property.FindPropertyRelative("upgradeCost");
            SerializedProperty descriptionPropery = property.FindPropertyRelative("upgradeDescription");

            EditorGUIUtility.labelWidth = 35;
            EditorGUI.PropertyField(iconRect, iconProperty, new GUIContent("Icon"));

            EditorGUIUtility.labelWidth = 40;
            EditorGUI.PropertyField(nameRect, nameProperty, new GUIContent("Name"));

            EditorGUIUtility.labelWidth = 35;
            EditorGUI.PropertyField(costRect, costProperty, new GUIContent("Cost"));

            EditorGUIUtility.labelWidth = 70;
            EditorGUI.PropertyField(descriptionRect, descriptionPropery, new GUIContent("Description"));

            EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label)*2.5f;
        }
    }
}
