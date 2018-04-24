using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateScript : MonoBehaviour {

    [SerializeField]
    private GameControl.DictionaryController.Towers towerEnum;
    private ParentController parentObject;
    private Tower.StandardTower tower;

    private SpriteRenderer sprRenderer;

    private Vector2 mousePos;

    private void Start() {
        tower = GameControl.DictionaryController.towerListDictionary[towerEnum].towerPrefab;
        sprRenderer = GetComponent<SpriteRenderer>();
    }
    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector2)mousePos;

        if (!(GameControl.GameController.WithinMapWorldPoint(mousePos, 0))) {
            sprRenderer.color = Color.red;
        }
        else {
            if (Physics2D.OverlapCircle(transform.position, 0.125f, GameControl.PlacementController.controllerObject.blockadeLayer) || (Physics2D.OverlapCircle(transform.position, 0, GameControl.PlacementController.controllerObject.towerLayer)) || (Physics2D.OverlapCircle(transform.position, 0, GameControl.PlacementController.controllerObject.waterLayer))) {
                sprRenderer.color = Color.red;
            }
            else {
                sprRenderer.color = Color.white;

                if (Input.GetMouseButtonDown(0)) {

                    GameControl.PlacementController.controllerObject.CreateTowerFamilyTree(tower, transform.position, Quaternion.identity);
                    GameControl.InventoryController.controllerObject.gold -= tower.generalStats.goldCost;

                    if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) || (tower.generalStats.goldCost > GameControl.InventoryController.controllerObject.gold)) {
                        GameControl.PlacementController.controllerObject.DestroyTowerTemplate();
                    }
                }
            }
        }
    }
}
