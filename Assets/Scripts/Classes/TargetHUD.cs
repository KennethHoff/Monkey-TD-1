using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

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
        TowerUpgradePath upgradePath;

        private void Start() {
            UIC = GetComponent<UIController>();
            ResetUI();
        }

        private void LateUpdate() {
            if (UIC.targettedTower != null) {
                targettedTower = UIC.targettedTower;
                upgradePath = targettedTower.generalStats.upgradePaths;
                var name = UIC.targettedTower.generalStats.towerEnum.ToString();

                var newName = name.Replace("_", " ");
                

                WriteTowerIcon(targettedTower.generalStats.towerIcon);
                WriteTowerName(newName);
                WriteTowerPops(targettedTower.generalStats.towerPops);
                WriteTowerSell(Mathf.RoundToInt(targettedTower.generalStats.sellValue));
                WriteTargettingMode(targettedTower.generalStats.targettingState.ToString());

                WriteUpgradeInfo(true, targettedTower.generalStats.upgradePaths.currentLeftUpgrade);

                WriteUpgradeInfo(false, targettedTower.generalStats.upgradePaths.currentRightUpgrade);

                //  ShowTargettingArea();
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

        public void WriteUpgradeInfo(bool _Left, int _currentUpgradeIndex) {

            TowerUpgrade[] _UpgradePath;
            Image _NextUpgrade_Image;
            TMPro.TextMeshProUGUI _NextUpgrade_Name;
            TMPro.TextMeshProUGUI _NextUpgrade_Cost;

            if (_Left) { 
                _UpgradePath = upgradePath.leftUpgradePath;
                _NextUpgrade_Image = LeftUpgrade_Next_Image;
                _NextUpgrade_Name = LeftUpgrade_Next_Name;
                _NextUpgrade_Cost = LeftUpgrade_Next_Cost;
            }
            else {
                _UpgradePath = upgradePath.rightUpgradePath;
                _NextUpgrade_Image = RightUpgrade_Next_Image;
                _NextUpgrade_Name = RightUpgrade_Next_Name;
                _NextUpgrade_Cost = RightUpgrade_Next_Cost;
            }

            TowerUpgrade _CurrentUpgrade = null;

            if (_currentUpgradeIndex < _UpgradePath.Length) {
                _CurrentUpgrade = _UpgradePath[_currentUpgradeIndex];
            }

            int _PreviousUpgradeInt = _currentUpgradeIndex - 1;

            ShowPreviousUpgrade(_Left, _PreviousUpgradeInt, _UpgradePath);

            if (_CurrentUpgrade != null) {
                UIController.WriteSprite(_NextUpgrade_Image,    _CurrentUpgrade.upgradeIcon);
                UIController.WriteText  (_NextUpgrade_Name,     _CurrentUpgrade.upgradeName);
                UIController.WriteText  (_NextUpgrade_Cost,     "$" + _CurrentUpgrade.upgradeCost.ToString());
            }
            else {
                UIController.WriteSprite(_NextUpgrade_Image,    UIC.HighestTierTowerImage);
                UIController.WriteText  (_NextUpgrade_Name,     null);
                UIController.WriteText  (_NextUpgrade_Cost,     null);
            }

            WriteUpgradeBoughtIcons(_Left, _currentUpgradeIndex);
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

        public void WriteUpgradeBoughtIcons(bool _Left,  int _CurrentUpgradeIndex) {

            Image Upgrade1;
            Image Upgrade2;
            Image Upgrade3;
            Image Upgrade4;

            if (_Left) {
                Upgrade1 = LeftUpgrade_Upgrade1;
                Upgrade2 = LeftUpgrade_Upgrade2;
                Upgrade3 = LeftUpgrade_Upgrade3;
                Upgrade4 = LeftUpgrade_Upgrade4;
            }
            else {
                Upgrade1 = RightUpgrade_Upgrade1;
                Upgrade2 = RightUpgrade_Upgrade2;
                Upgrade3 = RightUpgrade_Upgrade3;
                Upgrade4 = RightUpgrade_Upgrade4;
            }

            if (_CurrentUpgradeIndex >= 1) {
                Upgrade1.sprite = UIC.UpgradeBoughtImage; 
            }
            if (_CurrentUpgradeIndex >= 2) {
                Upgrade2.sprite = UIC.UpgradeBoughtImage;
            }
            if (_CurrentUpgradeIndex >= 3) {
                Upgrade3.sprite = UIC.UpgradeBoughtImage;
            }
            if (_CurrentUpgradeIndex >= 4) {
                Upgrade4.sprite = UIC.UpgradeBoughtImage;
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
        }
    }
}