using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileParent : ParentController {
    protected override void NoChildren() {
        base.NoChildren();
        GameControl.DictionaryController.controllerObject.OnProjectileParentDestroyed(this);
    }
}