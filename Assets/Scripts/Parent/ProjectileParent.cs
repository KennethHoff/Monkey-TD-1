using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParent : ParentController {
    protected override void LateUpdate() {
        base.LateUpdate();
        GameControl.DictionaryController.controllerObject.OnProjectileParentDestroyedGameObject(this);
    }
}