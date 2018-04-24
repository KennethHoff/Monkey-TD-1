using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Projectile {
    public class Tack : StandardProjectile {

        protected override void Start() {
            base.Start();
        }
        protected override void FixedUpdate() {
            base.FixedUpdate();
            DestroyAtDespawnDistance();
        }
    }
}