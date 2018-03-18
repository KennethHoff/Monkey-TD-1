using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerList {

    public Tower.StandardTower towerPrefab;
    public Tower.ParentController towerParentPrefab;
    public TemplateScript templatePrefab;
    public TowerSelector UIButton;

    public TowerList(Tower.StandardTower _towerPrefab, Tower.ParentController _towerParentPrefab, TemplateScript _templatePrefab, TowerSelector _UIButton) {
        towerPrefab = _towerPrefab;
        towerParentPrefab = _towerParentPrefab;
        templatePrefab = _templatePrefab;
        UIButton = _UIButton;
    }
}