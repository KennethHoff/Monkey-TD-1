using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Input;

namespace GameControl { 
    public class TargetHUD : MonoBehaviour {

        private StringFast stringFast = new StringFast();

        public Image TowerIconImage;
        public TextMeshProUGUI TowerNameText;
        public TextMeshProUGUI PopsText;
        public TextMeshProUGUI SellText;
        public TextMeshProUGUI TargettingModeText;
        public Button previousTargettingModeButton;
        public Button nextTargettingModeButton;
        
        private UIController UIC;
        [Header("Left:")]
        public Image LeftUpgrade_Previous_Image;
        public TextMeshProUGUI LeftUpgrade_Previous_Text;

        [Space(4)]
        public Image LeftUpgrade_Next_Image;
        public TextMeshProUGUI LeftUpgrade_Next_Name;
        public TextMeshProUGUI LeftUpgrade_Next_Cost;

        [Space(2.5f)]
        public Image LeftUpgrade_Upgrade1;
        public Image LeftUpgrade_Upgrade2;
        public Image LeftUpgrade_Upgrade3;
        public Image LeftUpgrade_Upgrade4;

        [Header("Right:")]
        public Image RightUpgrade_Previous_Image;
        public TextMeshProUGUI RightUpgrade_Previous_Text;

        [Space(2.5f)]
        public Image RightUpgrade_Next_Image;
        public TextMeshProUGUI RightUpgrade_Next_Name;
        public TextMeshProUGUI RightUpgrade_Next_Cost;

        [Space(2.5f)]
        public Image RightUpgrade_Upgrade1;
        public Image RightUpgrade_Upgrade2;
        public Image RightUpgrade_Upgrade3;
        public Image RightUpgrade_Upgrade4;

        Tower.StandardTower targettedTower;
        TowerUpgradePath upgradePaths;

        private void Start() {
            UIC = GetComponent<UIController>();
            ResetUI();
        }

        private void LateUpdate() {
            if (UIC.targettedTower != null) {

                if (Keyboard.current.commaKey.wasPressedThisFrame) {
                    UpgradeTower(true);
                }
                if (Keyboard.current.periodKey.wasPressedThisFrame) {
                    UpgradeTower(false);
                }
                
                targettedTower = UIC.targettedTower;
                var towerStats = targettedTower.GetStats<Tower.BaseTowerStats>();
                upgradePaths = towerStats.upgradePaths;
                var towerSprites = GameControl.DictionaryController.RetrieveTowerSpriteFromTowerSpriteDictionary_Enum(towerStats.towerEnum);


                var name = towerStats.towerEnum.ToString();

                var newName = name.Replace("_", " ");

                WriteTowerIcon(towerSprites.towerHUDIcons[towerSprites.currentTowerHUDIconSprite]);
                
                WriteTowerName(newName);
                WriteTowerPops(towerStats.towerPops);
                WriteTowerSell(Mathf.RoundToInt(towerStats.sellValue));
                WriteTargettingMode(towerStats.targettingState.ToString());

                if (upgradePaths != null) {

                    WriteUpgradeInfo();
                }
            }
        }

        public void WriteTowerIcon(Sprite _TowerIcon) {
            UIController.WriteSprite(TowerIconImage, _TowerIcon);
        }

        public void WriteTowerName(string _TowerName) {
            UIController.WriteText(TowerNameText, _TowerName);
        }

        public void WriteTowerPops(int _Pops) {
            UIController.WriteText(PopsText, _Pops.ToString());
        }

        public void WriteTowerSell(int _SellValue) {
            UIController.WriteText(SellText, _SellValue.ToString());
        }

        public void WriteTargettingMode(string _TargettingModeString) {
            UIController.WriteText(TargettingModeText, _TargettingModeString);
        }

        public void WriteUpgradeInfo() {
            var towerStats = targettedTower.GetStats<Tower.BaseTowerStats>();

            if (towerStats.currentLeftUpgrade != null) {
                UIController.WriteSprite(LeftUpgrade_Next_Image, towerStats.currentLeftUpgrade.upgradeIcon);
                UIController.WriteText(LeftUpgrade_Next_Name, towerStats.currentLeftUpgrade.upgradeName);
                UIController.WriteText(LeftUpgrade_Next_Cost, "$" + towerStats.currentLeftUpgrade.upgradeCost.ToString());
            }
            else {
                UIController.WriteSprite(LeftUpgrade_Next_Image, UIC.HighestTierTowerImage);
                UIController.WriteText(LeftUpgrade_Next_Name, null);
                UIController.WriteText(LeftUpgrade_Next_Cost, null);
            }

            if (towerStats.currentRightUpgrade != null) { 
                UIController.WriteSprite(RightUpgrade_Next_Image, towerStats.currentRightUpgrade.upgradeIcon);
                UIController.WriteText(RightUpgrade_Next_Name, towerStats.currentRightUpgrade.upgradeName);
                UIController.WriteText(RightUpgrade_Next_Cost, "$" + towerStats.currentRightUpgrade.upgradeCost.ToString());
            }
            else {
                UIController.WriteSprite(RightUpgrade_Next_Image, UIC.HighestTierTowerImage);
                UIController.WriteText(RightUpgrade_Next_Name, null);
                UIController.WriteText(RightUpgrade_Next_Cost, null);
            }

            WriteUpgradeBoughtIcons();
        }

        public void ShowPreviousUpgrade(bool _Left,  int _PreviousUpgradeIndex, TowerUpgrade[] _UpgradePath) {

            Image _PreviousUpgrade_Image;
            TMPro.TextMeshProUGUI _PreviousUpgrade_Text;
                
            if (_Left) { 
            _PreviousUpgrade_Image = LeftUpgrade_Previous_Image;
            _PreviousUpgrade_Text = LeftUpgrade_Previous_Text;
            }
            else {
                _PreviousUpgrade_Image = RightUpgrade_Previous_Image;
                _PreviousUpgrade_Text = RightUpgrade_Previous_Text;
            }
            if (_PreviousUpgradeIndex >= 0) {
                var _PreviousUpgrade = _UpgradePath[_PreviousUpgradeIndex];

                UIController.WriteSprite(_PreviousUpgrade_Image,    _PreviousUpgrade.upgradeIcon);
                UIController.WriteText  (_PreviousUpgrade_Text,     "Bought");
            }
            else {
                UIController.WriteSprite(_PreviousUpgrade_Image,    UIC.transparentImage);
                UIController.WriteText  (_PreviousUpgrade_Text,     null);
            }
        }

        public void WriteUpgradeBoughtIcons() {
            if (targettedTower != null) {
                var towerStats = targettedTower.GetStats<Tower.BaseTowerStats>();

                Sprite buyImage = UIC.UpgradeBuyImage;
                Sprite boughtImage = UIC.UpgradeBoughtImage;

                #region left

                if (towerStats.currentLeftUpgradeInt >= 1) {
                    LeftUpgrade_Upgrade1.sprite = boughtImage;
                }
                else {
                    LeftUpgrade_Upgrade1.sprite = buyImage;
                }

                if (towerStats.currentLeftUpgradeInt >= 2) {
                    LeftUpgrade_Upgrade2.sprite = boughtImage;
                }
                else {
                    LeftUpgrade_Upgrade2.sprite = buyImage;
                }

                if (towerStats.currentLeftUpgradeInt >= 3) {
                    LeftUpgrade_Upgrade3.sprite = boughtImage;
                }
                else {
                    LeftUpgrade_Upgrade3.sprite = buyImage;
                }

                if (towerStats.currentLeftUpgradeInt >= 4) {
                    LeftUpgrade_Upgrade4.sprite = boughtImage;
                }
                else {
                    LeftUpgrade_Upgrade4.sprite = buyImage;
                }

                #endregion

                #region right

                if (towerStats.currentRightUpgradeInt >= 1) {
                    RightUpgrade_Upgrade1.sprite = boughtImage;
                }
                else {
                    RightUpgrade_Upgrade1.sprite = buyImage;
                }

                if (towerStats.currentRightUpgradeInt >= 2) {
                    RightUpgrade_Upgrade2.sprite = boughtImage;
                }
                else {
                    RightUpgrade_Upgrade2.sprite = buyImage;
                }

                if (towerStats.currentRightUpgradeInt >= 3) {
                    RightUpgrade_Upgrade3.sprite = boughtImage;
                }
                else {
                    RightUpgrade_Upgrade3.sprite = buyImage;
                }

                if (towerStats.currentRightUpgradeInt >= 4) {
                    RightUpgrade_Upgrade4.sprite = boughtImage;
                }
                else {
                    RightUpgrade_Upgrade4.sprite = buyImage;
                }

                #endregion
            }


        }

        public void ChangeTargettingStateButton(bool _increase) {
            targettedTower.ChangeTargettingState(_increase);
        }

        public void SellTowerButton() {
            targettedTower.SellTower();
        }

        public void UpgradeTower(bool _Left) {
            targettedTower.UpgradeTower(_Left);
        }

        public void ShowTargettingArea() { // TODO: Add a targetting circle

        }

        public void ResetUI() {
            UIController.WriteSprite(RightUpgrade_Previous_Image, UIC.transparentImage);
            UIController.WriteText(RightUpgrade_Previous_Text, null);

            UIController.WriteSprite(RightUpgrade_Next_Image, UIC.transparentImage);
            UIController.WriteText(RightUpgrade_Next_Name, null);
            UIController.WriteText(RightUpgrade_Next_Cost, null);


            UIController.WriteSprite(LeftUpgrade_Previous_Image, UIC.transparentImage);
            UIController.WriteText(LeftUpgrade_Previous_Text, null);

            UIController.WriteSprite(LeftUpgrade_Next_Image, UIC.transparentImage);
            UIController.WriteText(LeftUpgrade_Next_Name, null);
            UIController.WriteText(LeftUpgrade_Next_Cost, null);



            WriteTowerName("Reset");
            WriteTowerPops(-1);
            WriteTowerSell(-1);
            WriteTargettingMode("Reset");
            WriteTowerIcon(UIC.transparentImage);
            WriteUpgradeBoughtIcons();

            Debug.Log("UI reset.");
        }
    }
}