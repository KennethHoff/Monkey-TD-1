using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class NinjaMonkeyThrowingStar : StandardProjectile {

        protected override void Start() {
            towerEnum = GameControl.PlacementController.Towers.NinjaMonkey;
            base.Start();
        }
    }
}