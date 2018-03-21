using UnityEngine;
using UnityEditor;
using System.Collections;


// Gave up - too much of a hassle for little-to-no gain

// that said.. still working on it :| #ItMustBeDone

namespace GameControl {

    [CustomPropertyDrawer(typeof(Wave), true)]
    public class WaveDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            

        SerializedProperty m_WaveTypeList = property.FindPropertyRelative("waveTypeList");
            float leftMargin;
            float rightMargin;

            float leftBoxMargin = Screen.width / 40;
            float rightBoxMargin = Screen.width / 20;
            leftMargin = rightMargin = Screen.width / 10;

            float boxHeight = 150;
            float distanceBetweenBoxes = 32;

            if (m_WaveTypeList.arraySize >= 0) {
                for (int i = 0; i < m_WaveTypeList.arraySize; i++) {

                    float currX = position.x + leftMargin;
                    float currY = position.y + 10 + (i * 150) + distanceBetweenBoxes * i;

                    float currHeight = boxHeight;
                    float currWidth  = Screen.width - leftMargin - rightMargin;

                    float removeSetWidth = 100;

                    GUI.Box(new Rect(currX, currY, currWidth, currHeight), string.Concat("Set:", i + 1));
                    
                    Rect RemoveSetRect = new Rect(currX + currWidth - removeSetWidth, currY + 0 * (currHeight / 5), removeSetWidth, currHeight / 7.5f);
                    Rect ToggleSetRect = new Rect(currX, currY + 0 * (currHeight / 5), removeSetWidth, currHeight / 7.5f);

                    SerializedProperty prop = m_WaveTypeList.GetArrayElementAtIndex(i);
                    
                    Rect bloonEnumRect = new Rect(currX + leftBoxMargin, currY + 1 * (currHeight / 5), currWidth-rightBoxMargin*2, currHeight / 7.5f);
                    SerializedProperty bloonEnumProperty = prop.FindPropertyRelative("bloonEnum");
                    GUIContent bloonEnumLabel = new GUIContent("Bloon");
                    
                    Rect regrowthRect = new Rect(currX + leftBoxMargin, currY + 2 * (currHeight / 5), currWidth - rightBoxMargin * 2, currHeight / 7.5f);
                    SerializedProperty regrowthProperty = prop.FindPropertyRelative("regrowth");
                    GUIContent regrowthLabel = new GUIContent("Regrowth");
                    
                    Rect camoRect = new Rect(currX + leftBoxMargin + Screen.width / 2, currY + 2 * (currHeight / 5), currWidth - rightBoxMargin * 2, currHeight / 7.5f);
                    SerializedProperty camoProperty = prop.FindPropertyRelative("camo");
                    GUIContent camoLabel = new GUIContent("Camo");
                    
                    Rect amountRect = new Rect(currX + leftBoxMargin, currY + 3 * (currHeight / 5), currWidth - rightBoxMargin * 2, currHeight / 7.5f);
                    SerializedProperty amountProperty = prop.FindPropertyRelative("amount");
                    GUIContent amountLabel = new GUIContent("Amount");
                    
                    Rect intervalRect = new Rect(currX + leftBoxMargin, currY + 4 * (currHeight / 5), currWidth - rightBoxMargin * 2, currHeight / 7.5f);
                    SerializedProperty intervalProperty = prop.FindPropertyRelative("interval");
                    GUIContent intervalLabel = new GUIContent("Interval");

                    
                    Rect conditionalRect = new Rect(currX + leftBoxMargin, currY + 5 * (currHeight / 5), currWidth - rightBoxMargin * 2, currHeight / 7.5f);

                    SerializedProperty delayProperty = prop.FindPropertyRelative("delay");
                    GUIContent delayLabel = new GUIContent("Delay");

                    GUIContent addNewSetLabel = new GUIContent("Add New set");


                    EditorGUI.BeginProperty(position, label, property);
                    {
                        EditorGUIUtility.labelWidth = Screen.width/10+50;
                        
                            if (GUI.Button(RemoveSetRect, new GUIContent(string.Concat("Delete: ", i+1)))) {
                                m_WaveTypeList.DeleteArrayElementAtIndex(i);
                            }

                            if (GUI.Button(ToggleSetRect, new GUIContent(string.Concat("Toggle: ", i+1)))) {
                                Debug.Log("Toggled set " + (i + 1));
                            }

                            EditorGUI.PropertyField(bloonEnumRect, bloonEnumProperty, bloonEnumLabel);
                            EditorGUI.PropertyField(amountRect, amountProperty, amountLabel);
                            camoProperty.boolValue = EditorGUI.ToggleLeft(camoRect, camoLabel, camoProperty.boolValue);
                            regrowthProperty.boolValue = EditorGUI.ToggleLeft(regrowthRect, regrowthLabel, regrowthProperty.boolValue);
                            EditorGUI.PropertyField(intervalRect, intervalProperty, intervalLabel);

                            if (i + 1 < m_WaveTypeList.arraySize) {

                                GUI.Box(new Rect(position.x + currWidth / 4, (currHeight + 5 * boxHeight / 5 + 2.5f), Screen.width - position.x - currWidth / 2, 25), GUIContent.none);
                                EditorGUI.PropertyField(conditionalRect, delayProperty, delayLabel);
                            }
                            else {
                                if (GUI.Button(conditionalRect, addNewSetLabel)) {
                                    m_WaveTypeList.arraySize++;
                                    // Add a new set to the array, and set its values to their Default.
                                    Debug.Log("Added a new Set.");
                                }
                            }
                    }
                    EditorGUI.EndProperty();
                }
            }
            else {
                if (GUI.Button(new Rect(position.x + leftMargin + leftBoxMargin, position.y + 10 + distanceBetweenBoxes * (boxHeight / 5), Screen.width - leftMargin - rightMargin - rightBoxMargin * 2, boxHeight / 7.5f), new GUIContent("Add New set"))) {
                    m_WaveTypeList.arraySize++;
                    // Add a new set to the array, and set its values to their Default.
                    Debug.Log("Added a new Set.");
                }
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            // return base.GetPropertyHeight(property, label);
            return 180 * property.FindPropertyRelative("waveTypeList").arraySize;
        }
    }
    /*
        [CustomEditor(typeof(WaveSpawner)), CanEditMultipleObjects]
        public class WaveSpawnerDrawer : Editor {
            SerializedProperty m_totalBloonsThisWave;
            SerializedProperty m_remainingBloonsThisWave;
            SerializedProperty m_bloonsKilledThisWave;
            SerializedProperty m_bloonsReachedFinalDestinationThisWave;

            SerializedProperty m_totalRBEThisWave;
            SerializedProperty m_RBERemainingThisWave;
            SerializedProperty m_RBEKilledThisWave;
            SerializedProperty m_RBEReachedFinalDestinationThisWave;
            SerializedProperty m_RBERegeneratedThisWave;

            SerializedProperty m_BloonsOnScreen;

            SerializedProperty m_WaveList;

            private void OnEnable() {
                m_totalBloonsThisWave = serializedObject.FindProperty("totalBloonsThisWave");
                m_remainingBloonsThisWave = serializedObject.FindProperty("remainingBloonsThisWave");
                m_bloonsKilledThisWave = serializedObject.FindProperty("bloonsKilledThisWave");
                m_bloonsReachedFinalDestinationThisWave = serializedObject.FindProperty("bloonsReachedFinalDestinationThisWave");

                m_totalRBEThisWave = serializedObject.FindProperty("totalRBEThisWave");
                m_RBEKilledThisWave = serializedObject.FindProperty("RBEKilledThisWave");
                m_RBERemainingThisWave = serializedObject.FindProperty("RBERemainingThisWave");
                m_RBEReachedFinalDestinationThisWave = serializedObject.FindProperty("RBEReachedFinalDestinationThisWave");
                m_RBERegeneratedThisWave = serializedObject.FindProperty("RBERegeneratedThisWave");

                m_BloonsOnScreen = serializedObject.FindProperty("bloonsOnScreen");

                m_WaveList = serializedObject.FindProperty("waves");
            }

        }
    */
}