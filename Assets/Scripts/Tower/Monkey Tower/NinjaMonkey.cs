using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using UnityEngine;

namespace Tower {

    public class NinjaMonkey : MonkeyTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(generalStats.projectileObject, transform.position, transform.rotation, generalStats.projectileSpawnPoints);
            
            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = generalStats.firingRange * 10f;
            }

            Debug.Log("Ninja Monkey #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}