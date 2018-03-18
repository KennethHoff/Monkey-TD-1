using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class DartMonkeyDart : StandardProjectile {

        protected override void Start() {
            towerEnum = GameControl.PlacementController.Towers.DartMonkey;
            base.Start();
        }
    }
}