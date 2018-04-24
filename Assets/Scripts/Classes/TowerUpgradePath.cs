using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TowerUpgradePath {

    [Range(0, 4)]
    public int currentLeftUpgrade;

    [Range(0, 4)]
    public int currentRightUpgrade;
    
    public TowerUpgrade[] leftUpgradePath = new TowerUpgrade[4];
    
    public TowerUpgrade[] rightUpgradePath = new TowerUpgrade[4];
}
