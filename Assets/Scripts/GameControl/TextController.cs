using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameControl { 
    public class TextController : MonoBehaviour {

        public TextMeshProUGUI roundValueText;
        public TextMeshProUGUI lifeText;
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI difficultyText;

        public static TextController controllerObject;

        private void Awake() {
            controllerObject = GetComponent<TextController>();
        }
        private void LateUpdate() {

            WriteGoldText(Mathf.FloorToInt(InventoryController.controllerObject.gold).ToString());
            if (InventoryController.controllerObject.life > 0)
                WriteLifeText(InventoryController.controllerObject.life.ToString());
            else WriteLifeText("Game Over!");
            WriteRoundValueText(WaveSpawner.controllerObject.currentWave + " / " + WaveSpawner.controllerObject.totalWaves);
            WriteDifficultyText(GameController.controllerObject.difficulty.ToString());
        }

        private void WriteText(TextMeshProUGUI textRenderer, string textToRender) {
            textRenderer.text = textToRender;
        }

        public void WriteLifeText(string textToRender) {
            WriteText(lifeText, textToRender);
        }
        public void WriteGoldText(string textToRender) {
            WriteText(goldText, textToRender);
        }
        public void WriteRoundValueText(string textToRender) {
            WriteText(roundValueText, textToRender);
        }
        public void WriteDifficultyText(string textToRender) {
            WriteText(difficultyText, textToRender);
        }
    }
}