using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveButton : MonoBehaviour {

    private Button button;
    private enum ButtonStates {
        StartWave,
        activateFF,
        deactivateFF,
        lockedFF
    }
    [SerializeField]
    private ButtonStates state = ButtonStates.StartWave;

    private Image imageRenderer;
    [SerializeField]
    private Sprite startSprite, activateFFSprite, deactivateFFSprite, lockedFFSprite;

    private void Start() {
        imageRenderer = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            ButtonClicked();
        }

        if ((GameControl.WaveSpawner.controllerObject.state == GameControl.WaveSpawner.SpawnState.RoundEnded) || (GameControl.WaveSpawner.controllerObject.state == GameControl.WaveSpawner.SpawnState.GameStart)) {
            ChangeState(ButtonStates.StartWave);
        }
    }

    private void ButtonClicked() {
        if (state == ButtonStates.StartWave) {
            ChangeState(ButtonStates.activateFF);
            GameControl.WaveSpawner.controllerObject.StartNextWave();
        }
        else if (state == ButtonStates.activateFF) { // Click to speed up
            GameControl.GameController.controllerObject.fastForward = true;
            ChangeState(ButtonStates.deactivateFF);
        }
        else if (state == ButtonStates.deactivateFF) { // Click to Slow down
            GameControl.GameController.controllerObject.fastForward = false;
            ChangeState(ButtonStates.activateFF);
        }
    }
    private void ChangeState(ButtonStates _state) {
        state = _state;

        switch (state) {
            case ButtonStates.StartWave:
                // Start game... Change sprite to SpeedUp
                imageRenderer.sprite = startSprite;
                break;
            case ButtonStates.activateFF:
                // Speed up wave... Change Sprite to SlowDown
                imageRenderer.sprite = activateFFSprite;
                break;
            case ButtonStates.deactivateFF:
                // Slow down wave... Change Sprite to SpeedUp
                imageRenderer.sprite = deactivateFFSprite;
                break;
            case ButtonStates.lockedFF:
                imageRenderer.sprite = lockedFFSprite; 
                break;
        }
    }
}
