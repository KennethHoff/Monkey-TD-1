using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class DartMonkey : MonkeyTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            Projectile.StandardProjectile shotProjectile = CreateProjectile(projectile, transform.position, transform.rotation, transform.parent);

            shotProjectile.despawnDistance = firingRange * 2.0f;
            Debug.Log("Dart Monkey shot!");
            base.Shoot();
        }
    }
}