using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuButton : MonoBehaviour {

    private Button button;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ButtonClicked() {
        GameControl.GameController.controllerObject.RestartGame();
    }
}
