using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
namespace GameControl {
    public class PlacementController : MonoBehaviour {

        public static PlacementController controllerObject;

        public enum Towers {
            Undefined,
            DartMonkey,
            TackShooter,
            SniperMonkey,
            NinjaMonkey
        }

        public Towers selectedObjectInArray;
        [Space(10f)]

        public TowerList TowerDict_DartMonkey;
        public TowerList TowerDict_TackShooter;
        public TowerList TowerDict_SniperMonkey;
        public TowerList TowerDict_NinjaMonkey;

        private GameObject currentlySelectedTemplate;
        private GameObject currentlySelectedUIButton;

        void Awake() {
            controllerObject = GetComponent<PlacementController>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(1) && selectedObjectInArray != Towers.Undefined) {
                DestroyTowerTemplate();
            }
        }
        public void InstantiateTowerTemplate(Towers enumIndex) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TemplateScript dictTemplate = DictionaryController.placementDictionary[enumIndex].templatePrefab;
            TowerSelector dictButton = DictionaryController.placementDictionary[enumIndex].UIButton;
            currentlySelectedTemplate = Instantiate(dictTemplate.gameObject, mousePos, Quaternion.identity);
            currentlySelectedUIButton = dictButton.gameObject;
            currentlySelectedUIButton.GetComponent<TowerSelector>().ButtonPressed();
            currentlySelectedUIButton.gameObject.SetActive(false);
            selectedObjectInArray = enumIndex;
        }

        public void DestroyTowerTemplate() {
            Destroy(currentlySelectedTemplate);
            if (currentlySelectedUIButton != null) {
                selectedObjectInArray = Towers.Undefined;
                currentlySelectedUIButton.gameObject.SetActive(true);
                currentlySelectedUIButton = null;
            }
        }
    }
}