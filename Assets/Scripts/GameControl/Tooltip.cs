using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameControl { 
    public class Tooltip : MonoBehaviour {

        private GameControl.UIController UIC;
        private GameControl.GameController GC;

        public GameObject tower_tooltipContainer;
        public TextMeshProUGUI tower_tooltipName;
        public TextMeshProUGUI tower_tooltipCost;
        public TextMeshProUGUI tower_tooltipHotkey;
        public TextMeshProUGUI tower_tooltipDescription;

        [Space(5)]
        public GameObject upgrade_tooltipContainer;
        public TextMeshProUGUI upgrade_tooltipName;
        public TextMeshProUGUI upgrade_tooltipCost;
        public TextMeshProUGUI upgrade_tooltipHotkey;
        public TextMeshProUGUI upgrade_tooltipDescription;

        private void Start() {
            UIC = GetComponent<UIController>();
            GC = GetComponent<GameController>();
        }

        private void LateUpdate() {

            if (GC.upgradeUnderCursor != null ) {
                upgrade_tooltipContainer.SetActive(true);
                var _Left = GC.upgradeUnderCursor.Left;
                TowerUpgrade upgrade;
                string hotkeyString;
                
                if (_Left) {
                    upgrade = UIC.targettedTower.GetStats<Tower.BaseTowerStats>().currentLeftUpgrade;
                    hotkeyString = ",";
                }
                else {
                    upgrade = UIC.targettedTower.GetStats<Tower.BaseTowerStats>().currentRightUpgrade;
                    hotkeyString = ".";
                }

                if (upgrade != null) {
                    upgrade_tooltipName.text = upgrade.upgradeName;
                    upgrade_tooltipCost.text = "$" + upgrade.upgradeCost.ToString();

                    string tooltip = upgrade.upgradeDescription;
                    string newTooltip = tooltip.Replace("<N>", "\n");

                    upgrade_tooltipDescription.text = newTooltip;
                    upgrade_tooltipHotkey.text = "Hotkey: " + hotkeyString;
                }
                else {
                    ResetUpgradeTooltip();
                }
            }
            else {
                ResetUpgradeTooltip();
            }

            if (GC.buttonUnderCursor != null) {
                tower_tooltipContainer.SetActive(true);
                var button = GC.buttonUnderCursor;
                var tower = button.tower;
                var towerStats = tower.GetStats<Tower.BaseTowerStats>();

                if (towerStats.towerName == null || towerStats.towerDescription == null) {
                    ResetTowerTooltip();
                    Debug.LogWarning("No name or description set.");
                    return;
                }

                tower_tooltipName.text = towerStats.towerName;
                tower_tooltipCost.text = "$" + towerStats.goldCost.ToString();
                tower_tooltipDescription.text = towerStats.towerDescription;
                if (towerStats.hotkey != KeyCode.None) { 
                    tower_tooltipHotkey.text = "Hotkey: " + towerStats.hotkey.ToString();
                }
                else {
                    tower_tooltipHotkey.text = "No Hotkey";
                }
            }
            else {
                ResetTowerTooltip();
            }
        }

        public void ResetTowerTooltip() {
            tower_tooltipName.text = null;
            tower_tooltipCost.text = null;
            tower_tooltipDescription.text = null;
            tower_tooltipHotkey.text = null;

            tower_tooltipContainer.SetActive(false);
        }

        public void ResetUpgradeTooltip() {
            upgrade_tooltipName.text = null;
            upgrade_tooltipCost.text = null;
            upgrade_tooltipDescription.text = null;
            tower_tooltipHotkey.text = null;

            upgrade_tooltipContainer.SetActive(false);
        }
    }
}