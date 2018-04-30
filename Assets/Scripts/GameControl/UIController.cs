using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace GameControl { 
    public class UIController : MonoBehaviour {

        private bool hovering = false;

        public Sprite transparentImage;
        public Sprite defaultImage;
        public Sprite HighestTierTowerImage;
        public Sprite UpgradeBoughtImage;
        public Sprite UpgradeBuyImage;
        [Space(5)]
        
        public GameObject BottomHUD_TowerSelected;
        public GameObject BottomHUD_Default;

        [Header("Inventory:")]
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI lifeText;

        [Header("Default HUD")]
        public TextMeshProUGUI roundValueText;
        public TextMeshProUGUI difficultyText;

        [Space(5)]
        public TextMeshProUGUI GameOverText;

        public static UIController controllerObject;

        [Space(5)]
        [ReadOnly] public Tower.StandardTower targettedTower;

        private void Awake() {
            controllerObject = GetComponent<UIController>();
        }
        private void Start() {
            SetGameOverText(false);
        }

        private void LateUpdate() {
            if (targettedTower != null) {
                ToggleBottomHUD(true);
            }
            else {
                ToggleBottomHUD(false);
                WriteRoundValueText(WaveSpawner.controllerObject.currentWave + " / " + WaveSpawner.controllerObject.totalWaves);
                WriteDifficultyText(GameController.controllerObject.difficulty.ToString());
            }
            WriteGoldText(Mathf.FloorToInt(InventoryController.controllerObject.gold).ToString());

            if (InventoryController.controllerObject.life > 0) {
                WriteLifeText(InventoryController.controllerObject.life.ToString());
            }
            else {
                WriteLifeText("0");
                GameControl.GameController.controllerObject.EndGame();
            }

        }


        public static void WriteText(TextMeshProUGUI _TextRenderer, string _TextToRender) {
            _TextRenderer.text = _TextToRender;
        }

        public static void WriteImage(Image _ImageRenderer, Image _ImageToRender) {
            WriteSprite(_ImageRenderer, _ImageToRender.sprite);
        }
        public static void WriteSprite(Image _ImageRenderer, Sprite _SpriteToRender) {
            if (_SpriteToRender == null) { 
                _SpriteToRender = controllerObject.defaultImage;
            }
            _ImageRenderer.sprite = _SpriteToRender;
        }

        public void WriteLifeText(string _TextToRender) {
                WriteText(lifeText, _TextToRender);
        }
        public void WriteGoldText(string _TextToRender) {
                WriteText(goldText, _TextToRender);
        }
        public void WriteRoundValueText(string _TextToRender) {
                WriteText(roundValueText, _TextToRender);
        }
        public void WriteDifficultyText(string _TextToRender) {
                WriteText(difficultyText, _TextToRender);
        }
        private void ToggleBottomHUD(bool _TowerSelected) {
            if (_TowerSelected) {
                BottomHUD_TowerSelected.SetActive(true);
                BottomHUD_Default.SetActive(false);
            }
            else {
                BottomHUD_TowerSelected.SetActive(false);
                BottomHUD_Default.SetActive(true);
            }
        }

        public void SetGameOverText(bool _GameOver) {
            if (_GameOver) {
                GameOverText.text = "GAME OVER";
            }
            else {
                GameOverText.text = null;
            }
        }
    }
}