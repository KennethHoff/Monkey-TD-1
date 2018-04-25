using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TowerUpgradePath {

    public TowerUpgradePath() {
        leftUpgradePath = new TowerUpgrade[4];
        rightUpgradePath = new TowerUpgrade[4];
    }

    [Range(0, 4)]
    public int currentLeftUpgrade;

    [Range(0, 4)]
    public int currentRightUpgrade;

    public TowerUpgrade[] leftUpgradePath;

    public TowerUpgrade[] rightUpgradePath;
}
