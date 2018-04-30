using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour {

    // Completed: Hide Tower in TowerSelectorUI if the same tower is selected as Template.
    [HideInInspector]
    public Vector3 startScale;

    [SerializeField]
    public GameControl.DictionaryController.Towers towerEnum;
    
    public Tower.StandardTower tower;

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
        tower = GameControl.DictionaryController.towerListDictionary[towerEnum].towerPrefab;
        startScale = transform.localScale;
        imageRenderer = GetComponent<Image>();
    }

    private void Update() {
        if (tower.GetStats<Tower.BaseTowerStats>().goldCost <= GameControl.InventoryController.controllerObject.gold) {
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
                    GameControl.PlacementController.controllerObject.InstantiateTowerTemplate(towerEnum);
                }
            }
            if (Input.GetKeyDown(tower.GetStats<Tower.BaseTowerStats>().hotkey)) {
                GameControl.PlacementController.controllerObject.DestroyTowerTemplate();
                GameControl.PlacementController.controllerObject.InstantiateTowerTemplate(towerEnum);
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
        GameControl.GameController.controllerObject.buttonUnderCursor = this;
    }

    public void ButtonPointerExit() {
        transform.localScale = startScale;
        if (GameControl.GameController.controllerObject.buttonUnderCursor == this) {
            GameControl.GameController.controllerObject.buttonUnderCursor = null;
        }
        hovering = false;
    }
    public void ButtonPressed() {
        transform.localScale = startScale;
        hovering = false;
    }
}
