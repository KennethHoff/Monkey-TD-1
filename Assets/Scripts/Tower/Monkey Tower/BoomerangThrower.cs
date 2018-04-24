using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class BoomerangThrower : MonkeyTower {

        public int numberOfSpins = 1;

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            if (!(generalStats.projectileObject is Projectile.Boomerang)) {
                Debug.LogError("Not shooting a boomerang!");
                return;
            }
            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(generalStats.projectileObject, transform.position, transform.rotation, generalStats.projectileSpawnPoints);
            
            foreach (Projectile.Boomerang projectile in shotProjectileList) {
                projectile.despawnDistance = generalStats.firingRange * 0.8f;
                projectile.totalSpins = numberOfSpins;
            }

            Debug.Log("Boomerang Thrower #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}