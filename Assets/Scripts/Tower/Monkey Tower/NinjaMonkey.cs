using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class NinjaMonkey : MonkeyTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectiles(projectileToFire, transform.position, transform.rotation, transform.parent, 1);
            
            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = firingRange * 2.0f;
            }

            Debug.Log("Ninja Monkey shot!");
            base.Shoot();
        }
    }
}