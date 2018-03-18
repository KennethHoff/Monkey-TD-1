using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class TackShooterTack : StandardProjectile { // TODO: Make all projectiles spawn under a parent that stays on the tower. This way it is easier to check in a Dictionary ( I do *NOT* want one set of Projectiles to hit one bloon multiple times. (or any of that bloon's children(recursively))

        protected override void Start() {
            towerEnum = GameControl.PlacementController.Towers.TackShooter;
            base.Start();
        }
    }
}