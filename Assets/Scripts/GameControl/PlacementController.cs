using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameControl {
    public class PlacementController : MonoBehaviour {

        public static PlacementController controllerObject;

        public LayerMask enemyLayer;
        public LayerMask blockadeLayer;
        public LayerMask waterLayer;
        public LayerMask towerLayer;
        
        public ProjectileParent ProjectileContainer;
        public TowerParent towerContainer;

        [ReadOnly]public DictionaryController.Towers selectedObjectInArray;
        
        private GameObject currentlySelectedTemplate;
        private GameObject currentlySelectedUIButton;

        void Awake() {
            controllerObject = this;
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(1) && selectedObjectInArray != DictionaryController.Towers.Undefined) {
                DestroyTowerTemplate();
            }

            if (GameControl.WaveSpawner.controllerObject.state == WaveSpawner.SpawnState.GameOver) {
                DestroyTowerTemplate();
            }
        }
        public void InstantiateTowerTemplate(DictionaryController.Towers _EnumIndex) {
            if (GameControl.WaveSpawner.controllerObject.state != WaveSpawner.SpawnState.GameOver) { 
                if (_EnumIndex == DictionaryController.Towers.Undefined) {
                    Debug.LogWarning("No Enum Selected.");
                    return;
                }

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TemplateScript template = DictionaryController.towerListDictionary[_EnumIndex].templatePrefab;
                TowerSelector button = DictionaryController.towerListDictionary[_EnumIndex].UIButton;
                currentlySelectedTemplate = Instantiate(GameControl.DictionaryController.towerListDictionary[_EnumIndex].templatePrefab.gameObject, mousePos, Quaternion.identity);
                currentlySelectedUIButton = button.gameObject;
                currentlySelectedUIButton.GetComponent<TowerSelector>().ButtonPressed();
                selectedObjectInArray = _EnumIndex;
            }
        }

        public void DestroyTowerTemplate() {
            Destroy(currentlySelectedTemplate);
            if (currentlySelectedUIButton != null) {
                selectedObjectInArray = DictionaryController.Towers.Undefined;
                currentlySelectedUIButton.gameObject.SetActive(true);
                currentlySelectedUIButton = null;
            }
        }

        public ParentController CreateTowerFamilyTree(Tower.StandardTower _tower, Vector3 _position, Quaternion _rotation) {

            ParentController parentContainer = Instantiate(PlacementController.controllerObject.towerContainer, _position, _rotation, GameController.controllerObject.mapController.towerParent.transform);
            parentContainer.name = "TowerParent_" + _tower.GetStats<Tower.BaseTowerStats>().towerEnum.ToString();
            Instantiate(_tower, _position, _rotation, parentContainer.transform);

            return parentContainer;
        }

        internal static void RemoveAllTowers() {
            foreach (Transform child in GameControl.GameController.controllerObject.mapController.towerParent) {
                Destroy(child.gameObject);
            }
        }
    }
}