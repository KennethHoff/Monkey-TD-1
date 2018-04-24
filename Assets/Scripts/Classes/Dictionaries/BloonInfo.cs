using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BloonInfo {

    public Bloon.StandardBloon bloonPrefab;

    [ReadOnly] public int RBE;
    [ReadOnly] public int toughness;
    [ReadOnly] public int familyTreeIndex;


    public BloonInfo(Bloon.StandardBloon _BloonPrefab) {
        bloonPrefab = _BloonPrefab;
        /*
        for (int i = 0; i < GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray.Length; i++) {
            if (GameControl.DictionaryController.controllerObject.BloonFamilyTreeArray[i] == bloonPrefab) {
                familyTreeIndex = i;
                break;
            }
        }
        */
    }
}