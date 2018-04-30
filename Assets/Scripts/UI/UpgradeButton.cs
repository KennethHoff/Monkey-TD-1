using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {
    public bool Left;

    public bool hovering;

    public void ButtonPointerEnter() {
        hovering = true;
        GameControl.GameController.controllerObject.upgradeUnderCursor = this;
    }

    public void ButtonPointerExit() {
        if (GameControl.GameController.controllerObject.upgradeUnderCursor == this) {
            GameControl.GameController.controllerObject.upgradeUnderCursor = null;
        }
        hovering = false;
    }
}
