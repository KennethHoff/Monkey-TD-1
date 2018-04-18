using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BloonInfo {

    public Bloon.StandardBloon bloonPrefab;
    public int rBE;
    public int toughness;
    public int familyTreeIndex;


    public BloonInfo(Bloon.StandardBloon _bloonPrefab) {
        bloonPrefab = _bloonPrefab;

        for (int i = 0; i < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length; i++) {
            if (GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i] == bloonPrefab) {
                familyTreeIndex = i;
                break;
            }
        }
    }
}