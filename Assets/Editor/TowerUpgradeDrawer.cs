using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GameControl {

    [CustomPropertyDrawer(typeof(TowerUpgrade))]
    public class TowerUpgradeDrawer : PropertyDrawer {

        private float relativeHeight = 4;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // base.OnGUI(position, property, label);
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float ySpacing = 7.5f;
            float positionHeight = position.height / relativeHeight;


            Rect nameRect           = new Rect(position.x,                              position.y + 5,                                     position.width * 0.6f - 5,  positionHeight);

            Rect iconRect           = new Rect(position.x + position.width * 0.6f,      position.y + 5,                                     position.width * 0.4f,      positionHeight);


            Rect enumRect           = new Rect(position.x,                              position.y + positionHeight + ySpacing,             position.width * 0.75f,     positionHeight);

            Rect costRect           = new Rect(position.x + position.width * 0.75f + 5, position.y + positionHeight + ySpacing,             position.width * 0.25f - 5, positionHeight);


            Rect descriptionRect    = new Rect(position.x,                              position.y + 2 * positionHeight + ySpacing*1.5f,    position.width,             positionHeight);

            SerializedProperty iconProperty         = property.FindPropertyRelative("upgradeIcon");
            SerializedProperty nameProperty         = property.FindPropertyRelative("upgradeName");
            SerializedProperty enumProperty         = property.FindPropertyRelative("upgradeEnum");
            SerializedProperty costProperty         = property.FindPropertyRelative("upgradeCost");
            SerializedProperty descriptionPropery   = property.FindPropertyRelative("upgradeDescription");

            float labelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = 40;
            EditorGUI.PropertyField(iconRect, iconProperty, new GUIContent("Icon"));
            
            EditorGUI.PropertyField(nameRect, nameProperty, new GUIContent("Name"));
            
            EditorGUI.PropertyField(costRect, costProperty, new GUIContent("Cost"));

            EditorGUIUtility.labelWidth = 75;
            EditorGUI.PropertyField(descriptionRect, descriptionPropery, new GUIContent("Description"));
            
            EditorGUI.PropertyField(enumRect, enumProperty, new GUIContent("Enum"));

            EditorGUIUtility.labelWidth = labelWidth;

            EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label) * relativeHeight;
        }
    }
}
