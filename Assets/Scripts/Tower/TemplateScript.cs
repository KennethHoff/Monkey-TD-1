﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateScript : MonoBehaviour {

    [SerializeField]
    private GameControl.PlacementController.Towers towerEnum;
    private Tower.ParentController parentObject; // COMPLETED: Make it use the Dictionary instead of dragging.
    private Tower.StandardTower tower;

    private Vector2 mousePos;
    [SerializeField]
    private LayerMask environmentLayer, towerLayer;

    // Update is called once per frame

    private void Start() {
        parentObject = GameControl.DictionaryController.placementDictionary[towerEnum].towerParentPrefab;
        tower = GameControl.DictionaryController.placementDictionary[towerEnum].towerPrefab;
    }
    void Update () {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePos.x, mousePos.y);

        if (GameControl.GameController.WithinMapWorldPoint(mousePos, 0)) {
            if (!(Physics2D.OverlapCircle(transform.position, 0.125f, environmentLayer) || (Physics2D.OverlapCircle(transform.position, 0, towerLayer) ))) {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;

                if (Input.GetMouseButtonDown(0)) {

                    GameControl.GameController.CreateFamilyTree(parentObject.gameObject, transform.position, Quaternion.identity, GameControl.GameController.towerParent);
                    GameControl.InventoryController.controllerObject.gold -= tower.goldCost;

                    if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) || (tower.goldCost > GameControl.InventoryController.controllerObject.gold)) {
                        GameControl.PlacementController.controllerObject.DestroyTowerTemplate();
                    }
                }
            }
            else gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}