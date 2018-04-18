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

        [SerializeField, Header("Tack Shooter Specifics:")]
        protected int tacksToFire;

        protected override void Shoot() {
            
            List<Projectile.StandardProjectile> shotProjectileList = new List<Projectile.StandardProjectile>();

            for (int i = 0; i < projectileSpawnPoints.Length; i++) {
                shotProjectileList.Add(CreateProjectile(projectileToFire, projectileSpawnPoints[i].position, projectileSpawnPoints[i].rotation, transform.parent));
            }

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = firingRange * 1.2f;
            }
            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}