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
            BoomerangThrower,
            NinjaMonkey,
            BombTower,
            IceTower,
            GlueGunner,
            MonkeyBuccaneer,
            MonkeyAce,
            SuperMonkey,
            MonkeyApprentice,
            MonkeyVillage,
            BananaFarm,
            MortarTower,
            DartlingGun,
            SpikeFactory,
            HeliPilot,
            MonkeyEngineer,
            Bloonchipper,
            MonkeySub
        }

        #region TowerLists
        public TowerList TowerDict_DartMonkey;
        public TowerList TowerDict_TackShooter;
        public TowerList TowerDict_SniperMonkey;
        public TowerList TowerDict_BoomerangThrower;
        public TowerList TowerDict_NinjaMonkey;
        // public TowerList TowerDict_BombTower;
        // public TowerList TowerDict_IceTower;
        // public TowerList TowerDict_GlueGunner;
        // public TowerList TowerDict_MonkeyBuccaneer;
        // public TowerList TowerDict_MonkeyAce;
        // public TowerList TowerDict_SuperMonkey;
        // public TowerList TowerDict_MonkeyApprentice;
        // public TowerList TowerDict_MonkeyVillage;
        // public TowerList TowerDict_BananaFarm;
        // public TowerList TowerDict_MortarTower;
        // public TowerList TowerDict_DartlingGun;
        // public TowerList TowerDict_SpikeFactory;
        // public TowerList TowerDict_HeliPilot;
        // public TowerList TowerDict_MonkeyEngineer;
        // public TowerList TowerDict_Bloonchipper;
        // public TowerList TowerDict_MonkeySub;

        #endregion

        public Towers selectedObjectInArray;
        [Space(10f)]

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