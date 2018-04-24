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
        }
        public void InstantiateTowerTemplate(DictionaryController.Towers enumIndex) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TemplateScript dictTemplate = DictionaryController.towerListDictionary[enumIndex].templatePrefab;
            TowerSelector dictButton = DictionaryController.towerListDictionary[enumIndex].UIButton;
            currentlySelectedTemplate = Instantiate(dictTemplate.gameObject, mousePos, Quaternion.identity);
            currentlySelectedUIButton = dictButton.gameObject;
            currentlySelectedUIButton.GetComponent<TowerSelector>().ButtonPressed();
            currentlySelectedUIButton.gameObject.SetActive(false);
            selectedObjectInArray = enumIndex;
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

            ParentController parentContainer = Instantiate(PlacementController.controllerObject.towerContainer, _position, _rotation, GameController.towerParent.transform);
            parentContainer.name = "TowerParent_" + _tower.generalStats.towerEnum.ToString();
            Instantiate(_tower, _position, _rotation, parentContainer.transform);

            return parentContainer;
        }
    }
}