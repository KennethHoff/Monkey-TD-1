using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

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
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = (Vector2)mousePos;

        if (!(GameControl.GameController.WithinMapWorldPoint(mousePos, 0)) | Physics2D.OverlapCircle(transform.position, 0.125f, GameControl.PlacementController.controllerObject.blockadeLayer) || (Physics2D.OverlapCircle(transform.position, 0, GameControl.PlacementController.controllerObject.towerLayer)) || (Physics2D.OverlapCircle(transform.position, 0, GameControl.PlacementController.controllerObject.waterLayer))) {
            sprRenderer.color = Color.red;
        }
        else {
            sprRenderer.color = Color.white;

            if (Mouse.current.leftButton.wasPressedThisFrame) {
                Debug.Log("Pressed button with a template active");
                GameControl.PlacementController.controllerObject.CreateTowerFamilyTree(tower, transform.position, Quaternion.identity);

                GameControl.InventoryController.controllerObject.gold -= tower.GetStats<Tower.BaseTowerStats>().goldCost;

                if (!(Keyboard.current.leftShiftKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame)) {
                    if (!(tower.GetStats<Tower.BaseTowerStats>().goldCost > GameControl.InventoryController.controllerObject.gold)) {
                        GameControl.PlacementController.controllerObject.DestroyTowerTemplate();
                    }
                }
            }
        }
    }
}
