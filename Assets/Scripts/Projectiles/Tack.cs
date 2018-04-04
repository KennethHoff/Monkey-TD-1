using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class Tack : StandardProjectile {

        protected override void Start() {
            towerEnum = GameControl.PlacementController.Towers.TackShooter;
            base.Start();
        }
    }
}