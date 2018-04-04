using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class ThrowingStar : StandardProjectile {

        protected override void Start() {
            towerEnum = GameControl.PlacementController.Towers.NinjaMonkey;
            base.Start();
        }
    }
}