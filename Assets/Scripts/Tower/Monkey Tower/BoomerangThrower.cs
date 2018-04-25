using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {
    public class BoomerangThrower : MonkeyTower {
        public override T GetStats<T>() {
            return boomerangThrowerTowerStats as T;
        }
        public BoomerangThrowerTowerStats boomerangThrowerTowerStats = new BoomerangThrowerTowerStats();

        protected override void Start() {
            boomerangThrowerTowerStats = new BoomerangThrowerTowerStats();
            Debug.Log(boomerangThrowerTowerStats.attackSpeed);
            base.Start();
        }
        public int numberOfSpins = 1;

        protected override void Shoot() {

            if (!(boomerangThrowerTowerStats.projectileObject is Projectile.Boomerang)) {
                Debug.LogError("Not shooting a boomerang!");
                return;
            }
            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.Boomerang projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 0.8f;
                projectile.totalSpins = numberOfSpins;
            }

            Debug.Log("Boomerang Thrower #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}