using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour {

    // Completed: Hide Tower in TowerSelectorUI if the same tower is selected as Template.
    [HideInInspector]
    public Vector3 startScale;

    [SerializeField]
    private GameControl.PlacementController.Towers towerToRepresent;
    
    private Tower.StandardTower towerScript;

    public enum UITowerStates {
        Clickable,
        NotClickable
    }

    public UITowerStates state;
    [SerializeField]
    private Sprite defaultSprite, greyedOutSprite;

    private Image imageRenderer;

    private bool hovering;

    private void Start() {
        state = UITowerStates.Clickable;
        TowerList towerDict = GameControl.DictionaryController.placementDictionary[towerToRepresent];
        towerScript = towerDict.towerPrefab;
        startScale = transform.localScale;
        imageRenderer = GetComponent<Image>();
    }

    private void Update() {
        if (towerScript.goldCost <= GameControl.InventoryController.controllerObject.gold) {
            state = UITowerStates.Clickable;
        }
        else {
            state = UITowerStates.NotClickable;
        }

        if (state == UITowerStates.Clickable) {
            imageRenderer.sprite = defaultSprite;
            imageRenderer.color = Color.white;
            if (hovering) {
                transform.localScale = startScale * .95f;
                if (Input.GetMouseButtonDown(0)) {
                    // Clicking - Instantiate new Tower Template : Make the image blink
                    GameControl.PlacementController.controllerObject.DestroyTowerTemplate();
                    GameControl.PlacementController.controllerObject.InstantiateTowerTemplate(towerScript.towerEnum);
                }
            }
        }
        else if (state == UITowerStates.NotClickable) {
            imageRenderer.sprite = greyedOutSprite;
            imageRenderer.color = Color.red;
        }
    }

    public void ButtonPointerEnter() {
        // Hovering -- waiting for clicking : Increase size of Sprite to show the player he is hovering : Show Tower Stat Panel
        hovering = true;
    }

    public void ButtonPointerExit() {
        transform.localScale = startScale;
        hovering = false;
    }
    public void ButtonPressed() {
        transform.localScale = startScale;
        hovering = false;
    }
}
