using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using UnityEngine;

namespace Tower { 

    public class TackShooter : NovaTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(generalStats.projectileObject, transform.position, transform.rotation, generalStats.projectileSpawnPoints);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = generalStats.firingRange * 1.2f;
            }

            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}