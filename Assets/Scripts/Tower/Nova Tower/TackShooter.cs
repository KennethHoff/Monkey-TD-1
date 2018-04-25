using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using UnityEngine;

namespace Tower { 

    public class TackShooter : NovaTower {
        public override T GetStats<T>() {
            return tackShooterTowerStats as T;
        }
        public TackShooterTowerStats tackShooterTowerStats = new TackShooterTowerStats();

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 1.2f;
            }

            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}